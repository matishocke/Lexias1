using Dapr.Workflow;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities;
using Lexias.Services.OrderAPI.DaprWorkflow.External;
using Shared.Dtos.Notification;
using Shared.Dtos.OrderDto;
using Shared.Dtos.PaymentDto;
using Shared.Dtos.WarehouseDto;
using Shared.Enum;
using Shared.IntegrationEvents;

namespace Lexias.Services.OrderAPI.DaprWorkflow
{
    //input of type OrderDto and produces an output of type OrderResultDto.
    public class OrderWorkflow : Workflow<OrderDto, OrderResultDto>
    {
        //RunAsync// method is the core logic of the workflow
        //WorkflowContext: Provides contextual information about the workflow, like workflow metadata and state.
        //OrderDto This is the input data for the workflow
        public override async Task<OrderResultDto> RunAsync(WorkflowContext workflowContext, OrderDto orderDto)
        {

            // Step 1: Set the order status to Confirmed
            orderDto.Status = OrderStatus.Received;
            orderDto.OrderId = workflowContext.InstanceId; //we create an Id for orderDto 


            // Generate OrderItemIds if not already present
            foreach (var item in orderDto.OrderItemsList)
            {
                item.OrderItemId = Guid.NewGuid().ToString();
            }



            //Notify
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderDto.OrderId} received from {orderDto.CustomerId}.",
                orderDto));



            // Step 2: Create Order in Database
            var createOrderResult = await workflowContext.CallActivityAsync<OrderResultDto>(
                nameof(CreateOrderActivity),
                orderDto);


            // If creating the order fails, delete it and return with an error message
            if (createOrderResult.OrderStatus == OrderStatus.Cancelled)
            {
                // Call DeleteOrderActivity to remove the failed order from the database
                await workflowContext.CallActivityAsync(
                    nameof(DeleteOrderActivity),
                    orderDto.OrderId);

                // Notify of order cancellation
                await workflowContext.CallActivityAsync(
                    nameof(NotifyActivity),
                    new Notification($"Order {orderDto.OrderId} creation failed and was deleted.", orderDto));

                // Return the OrderResult being Cancelledd
                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = createOrderResult.Message
                };
            }




            





            //Warehouse 
            //// Step 3: Reserve Items (Sending List of OrderItems)
            orderDto.Status = OrderStatus.ReservingInventory;

            // Notify that inventory reservation is being initiated
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Initiating inventory reservation for Order {orderDto.OrderId}.", orderDto));



            var orderItemsInsideOrderDto = orderDto.OrderItemsList;

            //Send to activity that sends to WarehouseAPI 
            await workflowContext.CallActivityAsync(nameof(ReserveItemsActivity), orderItemsInsideOrderDto);

            //Notify
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Waiting for reservation confirmation for Order {orderDto.OrderId}.", 
                orderDto));

            //ReserveItemsActivity has Event is going to bring ItemsReservedResultEvent

            // Step 4: Wait for Items Reserved Event (Response)
            //this will take the data from WorkflowController because WorkflowController is listening to other Services
            ItemsReservedResultEvent reservationResult = null;

            int maxRetries = 3;
            int attempt = 0;
            while (attempt < maxRetries)
            {
                attempt++;
                try
                {
                    reservationResult = await workflowContext.WaitForExternalEventAsync<ItemsReservedResultEvent>(
                        WorkflowChannelEvents.ItemReservedEvent,
                        TimeSpan.FromMinutes(10)); // Timeout for each attempt

                    // Break out of the loop if reservation succeeds
                    if (reservationResult.State == ResultState.Succeeded)
                    {
                        orderDto.TotalAmount = reservationResult.TotalAmount;      // Update TotalAmount in OrderDto from the reservation result
                        break;
                    }

                    // Log the failed reservation attempt
                    await workflowContext.CallActivityAsync(
                        nameof(NotifyActivity),
                        new Notification($"Reservation attempt {attempt} failed for Order {orderDto.OrderId}. Retrying...", orderDto));
                }
                catch (TimeoutException)
                {
                    await workflowContext.CallActivityAsync(
                        nameof(NotifyActivity),
                        new Notification($"Timeout during reservation attempt {attempt} for Order {orderDto.OrderId}.", orderDto));
                }

                // Wait before retrying (exponential backoff)
                await Task.Delay(TimeSpan.FromSeconds(5 * attempt));
            }



            // Check final reservation result
            if (reservationResult == null || reservationResult.State == ResultState.Failed)
            {
                await workflowContext.CallActivityAsync(
                    nameof(NotifyActivity),
                    new Notification($"Reservation failed for Order {orderDto.OrderId} after {maxRetries} attempts.",
                    orderDto));

                // Compensating action: delete order
                await workflowContext.CallActivityAsync(nameof(DeleteOrderActivity), orderDto.OrderId);

                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = "Inventory reservation failed after retries."
                };
            }


            // If reservation succeeded so Update the status to
            orderDto.Status = OrderStatus.InventoryReserved;

            // Log the updated TotalAmount
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Inventory reservation successful for Order {orderDto.OrderId}." +
                $" TotalAmount: {orderDto.TotalAmount}",
                orderDto));










            //Payment
            // Step 5: Process Payment
            orderDto.Status = OrderStatus.CheckingPayment;
            var paymentDto = new PaymentDto { Amount = orderDto.TotalAmount, OrderId = orderDto.OrderId};
            await workflowContext.CallActivityAsync(nameof(ProcessPaymentActivity), paymentDto);


            //Notify
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Waiting for payment for Order {orderDto.OrderId}.", orderDto));

            // Step 6: Wait for Payment Result
            var paymentResult = await workflowContext.WaitForExternalEventAsync<PaymentProcessedResultEvent>(
                WorkflowChannelEvents.PaymentEvent,
                TimeSpan.FromDays(1));


            //Failed Scenario
            if (paymentResult.State == ResultState.Failed)
            {
                //Failed CreateOrder delete database activity ------- DeleteOrderActivity
                //Failed Reservation Call activity --------
                // Payment failed, unreserve items ----------
                await workflowContext.CallActivityAsync(nameof(UnReserveItemsActivity), orderItemsInsideOrderDto);
                await workflowContext.CallActivityAsync(nameof(DeleteOrderActivity), orderDto.OrderId);
                await workflowContext.CallActivityAsync(nameof(NotifyActivity),
                    new Notification($"Payment failed for Order {orderDto.OrderId}. Unreserving items.", orderDto));

                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = "Payment failed"
                };
            }

            // Step 7: Finalize Order
            orderDto.Status = OrderStatus.Confirmed;
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderDto.OrderId} successfully completed.", orderDto));

            return new OrderResultDto { OrderId = orderDto.OrderId, OrderStatus = OrderStatus.Confirmed };
        }
    }
}







////List of OrderItems 
//var orderItemsInsideOrder = orderDto.OrderItemsList.Select(item => new OrderItemDto
//{
//    ProductId = item.ProductId,
//    Quantity = item.Quantity,
//    ItemType = item.ItemType
//}).ToList();

//// Step XX3XX: Reserve Items (Sending List of OrderItems)
//var orderItemsToReserve = new InventoryRequestDto(orderItemsInsideOrder);

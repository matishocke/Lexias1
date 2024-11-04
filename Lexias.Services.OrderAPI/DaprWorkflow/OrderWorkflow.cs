using Dapr.Workflow;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities;
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
        public override async Task<OrderResultDto> RunAsync(WorkflowContext context, OrderDto orderDto)
        {
            // Step 1: Set the order status to Confirmed
            orderDto.Status = OrderStatus.Confirmed;
            
            //Notify
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderDto.OrderId} received from {orderDto.CustomerDto.Name}.", orderDto));



            // Step 2: Reserve Items (sending parts of OrderItems)
            var itemsToReserve = new InventoryRequestDto(orderDto.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                ItemType = item.ItemType
            }).ToList());

            await context.CallActivityAsync(nameof(ReserveItemsActivity), itemsToReserve);

            //Notify
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Waiting for reservation confirmation for Order {orderDto.OrderId}.", orderDto));



            // Step 3: Wait for Items Reserved Event
            var reservationResult = await context.WaitForExternalEventAsync<ItemsReservedResultEvent>(
                "ItemReservedEvent", TimeSpan.FromDays(1));

            if (reservationResult.State == ResultState.Failed)
            {
                // Reservation failed
                await context.CallActivityAsync(nameof(NotifyActivity),
                    new Notification($"Reservation failed for Order {orderDto.OrderId}.", orderDto));

                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = "Reservation failed"
                };
            }


            // Step 4: Process Payment
            orderDto.Status = OrderStatus.CheckingPayment;
            var paymentDto = new PaymentDto { Amount = orderDto.TotalAmount };
            await context.CallActivityAsync(nameof(ProcessPaymentActivity), paymentDto);
            
            
            //Notify
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Waiting for payment for Order {orderDto.OrderId}.", orderDto));

            // Step 5: Wait for Payment Result
            var paymentResult = await context.WaitForExternalEventAsync<PaymentProcessedResultEvent>(
                "PaymentEvent", TimeSpan.FromDays(1));

            if (paymentResult.State == ResultState.Failed)
            {
                // Payment failed, unreserve items
                await context.CallActivityAsync(nameof(UnReserveItemsActivity), itemsToReserve);
                await context.CallActivityAsync(nameof(NotifyActivity),
                    new Notification($"Payment failed for Order {orderDto.OrderId}. Unreserving items.", orderDto));

                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = "Payment failed"
                };
            }

            // Step 6: Finalize Order
            orderDto.Status = OrderStatus.Confirmed;
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderDto.OrderId} successfully completed.", orderDto));

            return new OrderResultDto { OrderId = orderDto.OrderId, OrderStatus = OrderStatus.Confirmed };
        }
    }
}

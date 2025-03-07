﻿using Dapr.Workflow;
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
                new Notification($"Step2.O: Order {orderDto.OrderId} received from Customer {orderDto.CustomerId}.",
                orderDto));



            // Step 2: Create Order in Database
            var createOrderResult = await workflowContext.CallActivityAsync<OrderResultDto>(
                nameof(CreateOrderActivity),
                orderDto);


            //// Failed If creating the order fails, delete it and return with an error message
            if (createOrderResult.OrderStatus == OrderStatus.Cancelled)
                return await HandleOrderCreationFailure(workflowContext, orderDto, createOrderResult);









            //Warehouse 
            //// Step 3: Reserve Items (Sending List of OrderItems)
            orderDto.Status = OrderStatus.ReservingInventory;

            // Notify that inventory reservation is being initiated
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Step3.O: Initiating inventory reservation for Order {orderDto.OrderId}.", orderDto));



            var orderItemsInsideOrderDto = orderDto.OrderItemsList;

            //Send to activity that sends to WarehouseAPI 
            await workflowContext.CallActivityAsync(nameof(ReserveItemsActivity), orderItemsInsideOrderDto);

            //Notify
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Step4.O: Waiting for reservation confirmation for Order {orderDto.OrderId}.", 
                orderDto));

            //ReserveItemsActivity has Event is going to bring ItemsReservedResultEvent

            //*********************
            // Step 4: Wait for Items Reserved Event (Response)
            //this will take the data from WorkflowController because WorkflowController is listening to other Services
            var reservationResult = await workflowContext.WaitForExternalEventAsync<ItemsReservedResultEvent>(
                        WorkflowChannelEvents.ItemReservedEvent,
                        TimeSpan.FromMinutes(10)); // Timeout for the event


            //Failed Handle reservation failure
            if (reservationResult == null || reservationResult.State == ResultState.Failed)
                return await HandleReservationFailure(workflowContext, orderDto, reservationResult);


            // Handle reservation success
            orderDto.TotalAmount = reservationResult.TotalAmount;
            orderDto.Status = OrderStatus.InventoryReserved;

            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Step5.O: Inventory reservation successful for Order {orderDto.OrderId}." +
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
                new Notification($"Step6.O: Waiting for payment for Order {orderDto.OrderId}.", orderDto));

            // Step 6: Wait for Payment Result
            var paymentResult = await workflowContext.WaitForExternalEventAsync<PaymentProcessedResultEvent>(
                WorkflowChannelEvents.PaymentEvent,
                TimeSpan.FromDays(1));


            ////Failed Scenario
            if (paymentResult.State == ResultState.Failed)
                return await HandlePaymentFailure(workflowContext, orderDto);




            // Step 7: Finalize Order
            orderDto.Status = OrderStatus.Confirmed;
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Step7.O: Order {orderDto.OrderId} successfully completed.", orderDto));

            return new OrderResultDto { OrderId = orderDto.OrderId, OrderStatus = OrderStatus.Confirmed };
        }









        // Handles failure during order creation
        private async Task<OrderResultDto> HandleOrderCreationFailure(
            WorkflowContext workflowContext, 
            OrderDto orderDto, 
            OrderResultDto createOrderResult)
        {
            await workflowContext.CallActivityAsync(nameof(DeleteOrderActivity), orderDto.OrderId);

            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderDto.OrderId} creation failed and was deleted.", orderDto));

            return new OrderResultDto
            {
                OrderId = orderDto.OrderId,
                OrderStatus = OrderStatus.Cancelled,
                Message = createOrderResult.Message
            };
        }

        

        // Handles failure during inventory reservation
        private async Task<OrderResultDto> HandleReservationFailure(
            WorkflowContext workflowContext,
            OrderDto orderDto, 
            ItemsReservedResultEvent reservationResult)
        {
            // Notify about the reservation failure
            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Inventory reservation failed for Order {orderDto.OrderId}: " +
                $"{reservationResult?.Reason}", 
                orderDto));

            // Call UnReserveItemsActivity to inform WarehouseAPI
            await workflowContext.CallActivityAsync(
                nameof(UnReserveItemsActivity),
                orderDto.OrderItemsList);

            // Compensating action: delete the order from the database
            await workflowContext.CallActivityAsync(
                nameof(DeleteOrderActivity),
                orderDto.OrderId);

            return new OrderResultDto
            {
                OrderId = orderDto.OrderId,
                OrderStatus = OrderStatus.Cancelled,
                Message = reservationResult?.Reason ?? "Inventory reservation timeout."
            };
        }




        // Handles failure during payment processing
        private async Task<OrderResultDto> HandlePaymentFailure(
            WorkflowContext workflowContext,
            OrderDto orderDto)
        {
            await workflowContext.CallActivityAsync(nameof(UnReserveItemsActivity), orderDto.OrderItemsList);
            await workflowContext.CallActivityAsync(nameof(DeleteOrderActivity), orderDto.OrderId);

            await workflowContext.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Payment failed for Order {orderDto.OrderId}. Unreserving items.", orderDto));

            return new OrderResultDto
            {
                OrderId = orderDto.OrderId,
                OrderStatus = OrderStatus.Cancelled,
                Message = "Payment failed"
            };
        }
    }
}






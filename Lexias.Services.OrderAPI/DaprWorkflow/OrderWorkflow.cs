using Dapr.Workflow;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities;
using Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities;
using Shared.Dtos.OrderDto;
using Shared.Enum;
using Shared.IntegrationEvents;

namespace Lexias.Services.OrderAPI.DaprWorkflow
{
    public class OrderWorkflow : Workflow<OrderDto, OrderResultDto>
    {
        private ILogger<OrderWorkflow> _logger;


        // Parameterless constructor required by Dapr
        public OrderWorkflow() { }


        // Optional constructor with logger for testing or future expansion
        public OrderWorkflow(ILogger<OrderWorkflow> logger)
        {
            _logger = logger;
        }







        public override async Task<OrderResultDto> RunAsync(WorkflowContext context, OrderDto orderDto)
        {
            _logger.LogInformation($"Order workflow started for OrderId: {orderDto.OrderId}");

            try
            {
                // Step 1: Create Order
                var createOrderResult = await context.CallActivityAsync<CreateOrderActivity>(nameof(CreateOrderActivity), orderDto);
                if (!createOrderResult.IsSuccessful)
                {
                    _logger.LogError($"Failed to create order for OrderId: {orderDto.OrderId}");
                    return createOrderResult;
                }

                // Step 2: Reserve Items
                await context.CallActivityAsync<ReserveItemsActivity>(nameof(ReserveItemsActivity), orderDto);
                var itemsReservedEvent = await context.WaitForExternalEventAsync<ItemsReservedResultEvent>("ItemsReservedEvent", TimeSpan.FromMinutes(5));
                if (!itemsReservedEvent.IsSuccessful)
                {
                    _logger.LogError($"Item reservation failed for OrderId: {orderDto.OrderId}");
                    await context.CallActivityAsync<BackStockItemsActivity>(nameof(BackStockItemsActivity), orderDto); // Compensate
                    return new OrderResultDto(orderDto.OrderId, OrderStatus.InsufficientInventory, false, "Item reservation failed");
                }

                // Step 3: Process Payment
                await context.CallActivityAsync<ProcessPaymentActivity>(nameof(ProcessPaymentActivity), orderDto);
                var paymentEvent = await context.WaitForExternalEventAsync<PaymentProcessedResultEvent>("PaymentEvent", TimeSpan.FromMinutes(5));
                if (!paymentEvent.IsSuccessful)
                {
                    _logger.LogError($"Payment failed for OrderId: {orderDto.OrderId}");
                    await context.CallActivityAsync<IssueRefundActivity>(nameof(IssueRefundActivity), orderDto); // Compensate
                    return new OrderResultDto(orderDto.OrderId, OrderStatus.PaymentFailed, false, "Payment failed");
                }

                // Step 4: Ship Items
                await context.CallActivityAsync<ShipItemsActivity>(nameof(ShipItemsActivity), orderDto);
                var itemsShippedEvent = await context.WaitForExternalEventAsync<ItemsShippedResultEvent>("ItemsShippedEvent", TimeSpan.FromMinutes(5));
                if (!itemsShippedEvent.IsSuccessful)
                {
                    _logger.LogError($"Shipping failed for OrderId: {orderDto.OrderId}");
                    await context.CallActivityAsync<UnReserveItemsActivity>(nameof(UnReserveItemsActivity), orderDto); // Compensate
                    return new OrderResultDto(orderDto.OrderId, OrderStatus.ShippingItemsFailed, false, "Shipping failed");
                }

                // Step 5: Complete Order
                await context.CallActivityAsync<CompleteOrderActivity>(nameof(CompleteOrderActivity), orderDto);
                _logger.LogInformation("Order workflow completed successfully.");
                return new OrderResultDto(orderDto.OrderId, OrderStatus.Completed, true, "Order completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Order workflow encountered an error.");
                return new OrderResultDto(orderDto.OrderId, OrderStatus.Error, false, "Workflow failed");
            }
        }
    }
}

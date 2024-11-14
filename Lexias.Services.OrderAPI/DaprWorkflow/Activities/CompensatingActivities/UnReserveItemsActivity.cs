using Dapr.Client;
using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Dtos.OrderDto;
using Shared.Enum;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    public class UnReserveItemsActivity : WorkflowActivity<List<OrderItemDto>, object?>
    {

        private readonly DaprClient _daprClient;
        private readonly ILogger<UnReserveItemsActivity> _logger;

        public UnReserveItemsActivity(DaprClient daprClient, ILogger<UnReserveItemsActivity> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }




        public override async Task<object?> RunAsync(WorkflowActivityContext context, List<OrderItemDto> itemsToUnreserve)
        {
            _logger.LogInformation($"Unreserving items for OrderId: {context.InstanceId}");

            // Create an unreserve event to send to Warehouse
            var unreserveItemsEvent = new ItemsReservationFailedEvent
            {
                CorrelationId = context.InstanceId,
                OrderItems = itemsToUnreserve,
                State = ResultState.Failed,
                Reason = "Payment failed, unreserving items."
            };

            // Publish the unreserve items event again to WarehouseAPI itemController 
            await _daprClient.PublishEventAsync(
                WarehouseChannel.Channel,
                WarehouseChannel.Topics.ReservationFailed,
                unreserveItemsEvent);

            return null;
        }
    }
}

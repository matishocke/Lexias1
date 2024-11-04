using Dapr.Client;
using Dapr.Workflow;
using Shared.Dtos.WarehouseDto;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class ReserveItemsActivity : WorkflowActivity<InventoryRequestDto, object?>
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ReserveItemsActivity> _logger;

        public ReserveItemsActivity(DaprClient daprClient, ILogger<ReserveItemsActivity> logger)
        {
            _logger = logger;
            _daprClient = daprClient;
        }



        public override async Task<object?> RunAsync(WorkflowActivityContext context, InventoryRequestDto itemToReserve)
        {
            _logger.LogInformation($"Attempting to reserve items for Order: {context.InstanceId}");

            var reserveItemEvent = new ReserveItemsEvent 
            { 
                CorrelationId = context.InstanceId, 
                OrderItems = itemToReserve.ItemsRequested  //sender hele OrderItemS(quantity productid)
            };

            await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                                WarehouseChannel.Topics.Reservation,
                                                reserveItemEvent);

            return null;
        }
    }
}

using Dapr.Client;
using Dapr.Workflow;
using Shared.Dtos.OrderDto;
using Shared.Dtos.WarehouseDto;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class ReserveItemsActivity : WorkflowActivity<List<OrderItemDto>, object?>
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ReserveItemsActivity> _logger;

        public ReserveItemsActivity(DaprClient daprClient, ILogger<ReserveItemsActivity> logger)
        {
            _logger = logger;
            _daprClient = daprClient;
        }


        // itemToReserve = List<OrderItem>
        public override async Task<object?> RunAsync(WorkflowActivityContext context, List<OrderItemDto> itemsToReserve)
        {
            _logger.LogInformation($"Attempting to reserve items for Order: {context.InstanceId}");

            var reserveItemEvent = new ReserveItemsEvent 
            { 
                CorrelationId = context.InstanceId, 
                OrderItemsList = itemsToReserve  //sender hele List<OrderItemS(quantity productid)>
            };


            //sending now to the message Bus with use of dapr
            await _daprClient.PublishEventAsync(WarehouseChannel.Channel,
                                                WarehouseChannel.Topics.Reservation,
                                                reserveItemEvent);

            return null;
        }
    }
}

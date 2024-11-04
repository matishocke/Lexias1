using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Dtos.WarehouseDto;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    public class UnReserveItemsActivity : WorkflowActivity<InventoryRequestDto, object?>
    {
        private readonly ILogger<UnReserveItemsActivity> _logger;

        public UnReserveItemsActivity(ILogger<UnReserveItemsActivity> logger)
        {
            _logger = logger;
        }

        public override async Task<object?> RunAsync(WorkflowActivityContext context, InventoryRequestDto request)
        {
            _logger.LogInformation($"Unreserving items for OrderId: {context.InstanceId}");
            await Task.CompletedTask;
            return null;
        }
    }
}

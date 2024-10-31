using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class ReserveItemsActivity : WorkflowActivity<Order, OrderResult>
    {
        private readonly AppDbContext _dbContext;

        public ReserveItemsActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);

            if (existingOrder == null)
                return new OrderResult(order.OrderId, OrderStatus.InsufficientInventory, false, "Order not found");

            // Simulate inventory check and reservation
            existingOrder.OrderStatus = OrderStatus.SufficientInventory;
            await _dbContext.SaveChangesAsync();

            return new OrderResult(existingOrder.OrderId, existingOrder.OrderStatus, true, "Items reserved successfully");
        }
    }
}

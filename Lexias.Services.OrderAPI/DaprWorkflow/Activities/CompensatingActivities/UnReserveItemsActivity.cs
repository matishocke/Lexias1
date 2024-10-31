using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    public class UnReserveItemsActivity : WorkflowActivity<Order, OrderResult>
    {
        private readonly AppDbContext _dbContext;

        public UnReserveItemsActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
        {
            // Logic to unreserve items
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.OrderStatus = OrderStatus.Unreserved;  // Example status update
                _dbContext.Orders.Update(existingOrder);
                await _dbContext.SaveChangesAsync();
            }

            return new OrderResult(order.OrderId, OrderStatus.Unreserved, true, "Items have been unreserved.");
        }
    }
}

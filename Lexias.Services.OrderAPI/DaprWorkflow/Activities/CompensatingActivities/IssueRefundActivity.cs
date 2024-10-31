using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    public class IssueRefundActivity : WorkflowActivity<Order, OrderResult>
    {
        private readonly AppDbContext _dbContext;

        public IssueRefundActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
        {
            // Logic to issue a refund
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);
            if (existingOrder != null)
            {
                existingOrder.OrderStatus = OrderStatus.Refunded;  // Example status update
                _dbContext.Orders.Update(existingOrder);
                await _dbContext.SaveChangesAsync();
            }

            return new OrderResult(order.OrderId, OrderStatus.Refunded, true, "Refund has been issued.");
        }
    }
}

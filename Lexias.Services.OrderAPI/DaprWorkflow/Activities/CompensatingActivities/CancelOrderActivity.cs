using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    public class CancelOrderActivity : WorkflowActivity<Order, OrderResult>
    {
        private readonly AppDbContext _dbContext;

        public CancelOrderActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);

            if (existingOrder == null)
                return new OrderResult(order.OrderId, OrderStatus.Error, false, "Order not found for cancellation");

            // Update the order status to canceled
            existingOrder.OrderStatus = OrderStatus.Error;
            await _dbContext.SaveChangesAsync();

            return new OrderResult(existingOrder.OrderId, OrderStatus.Error, true, "Order canceled successfully");
        }
    }
}

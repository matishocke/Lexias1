using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class ProcessPaymentActivity : WorkflowActivity<Order, OrderResult>
    {
        private readonly AppDbContext _dbContext;

        public ProcessPaymentActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);

            if (existingOrder == null)
                return new OrderResult(order.OrderId, OrderStatus.PaymentFailed, false, "Order not found");

            // Simulate payment processing
            existingOrder.OrderStatus = OrderStatus.PaymentSuccess;
            await _dbContext.SaveChangesAsync();

            return new OrderResult(existingOrder.OrderId, existingOrder.OrderStatus, true, "Payment processed successfully");
        }
    }
}

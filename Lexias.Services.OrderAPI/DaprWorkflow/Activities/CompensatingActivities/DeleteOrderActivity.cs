using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    //string is orderID as input
    public class DeleteOrderActivity : WorkflowActivity<string, OrderResult>
    {
        private readonly AppDbContext _dbContext;

        public DeleteOrderActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }





        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, string orderId)
        {
            try
            {
                // Find the order by OrderId
                var order = await _dbContext.Orders.FindAsync(orderId);

                if (order == null)
                {
                    return new OrderResult
                    {
                        OrderId = orderId,
                        OrderStatus = OrderStatus.Cancelled,
                        Message = "Order not found, could not delete."
                    };
                }

                // Remove the order from the database
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();

                // Return success result
                return new OrderResult
                {
                    OrderId = orderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = "Order deleted successfully."
                };
            }
            catch (Exception ex)
            {
                // Handle error by returning a failed result
                return new OrderResult
                {
                    OrderId = orderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = $"Failed to delete order: {ex.Message}"
                };
            }
        }
    }
}



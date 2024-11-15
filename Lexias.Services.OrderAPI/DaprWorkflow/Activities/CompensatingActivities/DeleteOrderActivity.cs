using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data.Repository;
using Lexias.Services.OrderAPI.Data.Repository.IRepository;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    //string is orderID as input
    public class DeleteOrderActivity : WorkflowActivity<string, OrderResult>
    {
        private readonly IOrderRepository _db;

        public DeleteOrderActivity(IOrderRepository orderRepository)
        {
            _db = orderRepository;
        }






        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, string orderId)
        {
            try
            {
                // Delete order from the database using repository
                await _db.DeleteOrderAsync(orderId);

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



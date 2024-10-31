using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class CompleteOrderActivity : WorkflowActivity<Order, object?>
    {
        private readonly AppDbContext _dbContext;

        public CompleteOrderActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<object?> RunAsync(WorkflowActivityContext context, Order order)
        {
            // Check if the order already exists in the database
            var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);

            if (existingOrder == null)
            {
                // Add new order if it doesn't exist
                await _dbContext.Orders.AddAsync(order);
            }
            else
            {
                // Update the existing order if found
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.TotalAmount = order.TotalAmount;
                existingOrder.OrderItems = order.OrderItems;
                existingOrder.Customer = order.Customer;
                existingOrder.OrderDate = order.OrderDate;

                _dbContext.Orders.Update(existingOrder);
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            return null;
        }
    }
}

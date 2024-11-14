using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities.CompensatingActivities
{
    ///when an order is canceled.


    //public class BackStockItemsActivity : WorkflowActivity<Order, OrderResult>
    //{
    //    private readonly AppDbContext _dbContext;

    //    public BackStockItemsActivity(AppDbContext dbContext)
    //    {
    //        _dbContext = dbContext;
    //    }

    //    public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
    //    {
    //        // Logic to "backstock" items here
    //        var existingOrder = await _dbContext.Orders.FindAsync(order.OrderId);
    //        if (existingOrder != null)
    //        {
    //            existingOrder.OrderStatus = OrderStatus.Restocked;  // Example status update
    //            _dbContext.Orders.Update(existingOrder);
    //            await _dbContext.SaveChangesAsync();
    //        }

    //        return new OrderResult(order.OrderId, OrderStatus.Restocked, true, "Items have been restocked.");
    //    }
    //}
}

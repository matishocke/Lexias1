using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Enum;

//namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
//{
//    public class CreateOrderActivity : WorkflowActivity<Order, OrderResult>
//    {
//        private readonly AppDbContext _dbContext;

//        public CreateOrderActivity(AppDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public override async Task<OrderResult> RunAsync(WorkflowActivityContext context, Order order)
//        {
//            // Add order to database
//            _dbContext.Orders.Add(order);
//            await _dbContext.SaveChangesAsync();

//            // Return a result indicating success
//            return new OrderResult(order.OrderId, OrderStatus.Created, true, "Order created successfully");
//        }
//    }
//}

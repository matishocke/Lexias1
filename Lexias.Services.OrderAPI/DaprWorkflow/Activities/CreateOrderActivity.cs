using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data.Repository.IRepository;
using Lexias.Services.OrderAPI.Models;
using Shared.Dtos.OrderDto;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class CreateOrderActivity : WorkflowActivity<OrderDto, OrderResultDto>
    {
        private readonly IOrderRepository _db;

        public CreateOrderActivity(IOrderRepository orderRepository)
        {
            _db = orderRepository;
        }
     
        public override async Task<OrderResultDto> RunAsync(WorkflowActivityContext context, OrderDto orderDto)  //orderdto data from controller But the ID the id never get created by user
        {
            try
            {
                if (orderDto.OrderItemsList == null || !orderDto.OrderItemsList.Any())
                {
                    throw new ArgumentException("Order must have at least one item.");
                }

                

                // Convert OrderDto to Order inside the activity
                var order = new Order
                {
                    OrderId = orderDto.OrderId,  
                    OrderItemsList = orderDto.OrderItemsList.Select(item => new OrderItem
                    {
                        OrderItemId = string.IsNullOrEmpty(item.OrderItemId) ? Guid.NewGuid().ToString() : item.OrderItemId, // Generate if missing   // Use the OrderItemId from OrderDto we created in workflow
                        OrderId = orderDto.OrderId,      // Set the OrderId on each OrderItem
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    }).ToList(),
                    OrderDate = DateTime.UtcNow,
                    CustomerId = orderDto.CustomerId,
                    TotalAmount = orderDto.TotalAmount,
                    OrderStatus = orderDto.Status
                };

                // Add order to database using repository
                await _db.AddOrderAsync(order);



                // Return success result
                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Pending,
                    Message = "Order created in database successfully."
                };
            }
            catch (Exception ex)
            {
                // Handle error by returning a failed result
                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = $"Failed to create order in database: {ex.Message}"
                };
            }
        }
    }
}

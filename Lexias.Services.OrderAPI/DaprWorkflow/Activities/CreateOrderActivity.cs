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
                //Calculate the totalPrice of the order
                decimal totalAmount = 0;
                foreach (var item in orderDto.OrderItemsList)
                {
                    totalAmount = totalAmount + item.Quantity * item.Price;
                }

                // Convert OrderDto to Order inside the activity
                var order = new Order
                {
                    OrderId = orderDto.OrderId,  
                    OrderItemsList = orderDto.OrderItemsList.Select(item => new OrderItem
                    {
                        OrderItemId = item.OrderItemId,  // Use the OrderItemId from OrderDto we created in workflow
                        OrderId = orderDto.OrderId,      // Set the OrderId on each OrderItem
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                    }).ToList(),
                    OrderDate = DateTime.UtcNow,
                    CustomerId = orderDto.CustomerId,
                    TotalAmount = totalAmount,
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

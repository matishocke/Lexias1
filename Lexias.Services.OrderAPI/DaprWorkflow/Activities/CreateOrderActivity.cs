using Dapr.Workflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Shared.Dtos.OrderDto;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class CreateOrderActivity : WorkflowActivity<OrderDto, OrderResultDto>
    {
        private readonly AppDbContext _dbContext;

        public CreateOrderActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }




        public override async Task<OrderResultDto> RunAsync(WorkflowActivityContext context, OrderDto orderDto)
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
                    OrderId = Guid.NewGuid().ToString(),
                    OrderItemsList = orderDto.OrderItemsList.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ItemType = item.ItemType
                    }).ToList(),
                    OrderDate = DateTime.UtcNow,
                    Customer = new Customer
                    {
                        CustomerId = orderDto.CustomerDto.CustomerId,
                        Name = orderDto.CustomerDto.Name,
                        Email = orderDto.CustomerDto.Email,
                        Address = orderDto.CustomerDto.Address,
                        PhoneNumber = orderDto.CustomerDto.PhoneNumber
                    },
                    TotalAmount = totalAmount,
                    OrderStatus = orderDto.Status
                };

                // Add order to database
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                // Return success result
                return new OrderResultDto
                {
                    OrderId = order.OrderId,
                    OrderStatus = OrderStatus.Pending,
                    Message = "Order created successfully."
                };
            }
            catch (Exception ex)
            {
                // Handle error by returning a failed result
                return new OrderResultDto
                {
                    OrderId = orderDto.OrderId,
                    OrderStatus = OrderStatus.Cancelled,
                    Message = $"Failed to create order: {ex.Message}"
                };
            }
        }
    }
}

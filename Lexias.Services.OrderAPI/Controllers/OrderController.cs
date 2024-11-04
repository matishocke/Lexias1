using Dapr.Client;
using Lexias.Services.OrderAPI.DaprWorkflow;
using Lexias.Services.OrderAPI.Data;
using Lexias.Services.OrderAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.OrderDto;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly AppDbContext _context;

        public OrderController(DaprClient daprClient, AppDbContext context)
        {
            _daprClient = daprClient;
            _context = context;
        }



        //If Create Order workflow starts
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            // Convert OrderDto to Order domain model
            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderItems = orderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
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
                TotalAmount = orderDto.TotalAmount,
                OrderStatus = OrderStatus.Pending
            };

            // Add order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Start the workflow with the mapped Order object
            var instanceId = Guid.NewGuid().ToString();
            var workflowComponentName = "dapr";
            var workflowName = nameof(OrderWorkflow);

            var startResponse =
                await _daprClient.StartWorkflowAsync(workflowComponentName, workflowName, instanceId, order);

            return Ok(startResponse);
        }




        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            // Retrieve the order details from the database
            var order = await _context.Orders.Include(o => o.OrderItems)
                                             .Include(o => o.Customer)
                                             .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound($"Order with ID {orderId} not found.");
            }

            // Convert the Order to OrderDto to return to the client
            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ItemType = item.ItemType
                }).ToList(),
                OrderDate = order.OrderDate,
                CustomerDto = new CustomerDto
                {
                    CustomerId = order.Customer.CustomerId,
                    Name = order.Customer.Name,
                    Email = order.Customer.Email,
                    Address = order.Customer.Address,
                    PhoneNumber = order.Customer.PhoneNumber
                },
                Status = order.OrderStatus,
                TotalAmount = order.TotalAmount
            };

            return Ok(orderDto);
        }
    }
















    //[HttpPost]
    //public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    //{
    //    _logger.LogInformation($"Starting workflow for order ID: {orderDto.OrderId}");

    //    List<OrderItem> orderItemMapp = orderDto.OrderItemsDto.Select(x => new OrderItem
    //    {
    //        ItemType = x.ItemType,
    //        Quantity = x.Quantity,
    //        Price = x.Price,
    //        Size = x.Size,
    //        Color = x.Color
    //    }).ToList();

    //    Customer customerMapp = new Customer
    //    {
    //        Name = orderDto.CustomerDto.Name,
    //        Email = orderDto.CustomerDto.Email
    //    };

    //    Order orderMapp = new Order
    //    {
    //        OrderId = orderDto.OrderId,
    //        OrderItems = orderItemMapp,
    //        OrderDate = orderDto.OrderDate,
    //        Customer = customerMapp,
    //        OrderStatus = orderDto.OrderStatus,
    //        TotalAmount = orderDto.TotalAmount
    //    };





    //    try
    //    {
    //        var workflowInstanceId = await _daprClient.StartWorkflowAsync(
    //            _workflowComponentName,
    //            _workflowName,
    //            orderMapp.OrderId,
    //            orderMapp);

    //        _logger.LogInformation($"Workflow started with instance ID: {workflowInstanceId}");
    //        return Ok(new { OrderId = orderDto.OrderId, WorkflowInstanceId = workflowInstanceId });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Failed to start workflow for order ID: {orderDto.OrderId}");
    //        return StatusCode(500, "Failed to start order workflow.");
    //    }
    //}
}


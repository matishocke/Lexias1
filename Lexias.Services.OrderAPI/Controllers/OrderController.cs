using Dapr.Client;
using Lexias.Services.OrderAPI.DaprWorkflow;
using Lexias.Services.OrderAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.OrderDto;

namespace Lexias.Services.OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<OrderController> _logger;
        private readonly string _workflowComponentName = "dapr";
        private readonly string _workflowName = nameof(OrderWorkflow);

        public OrderController(DaprClient daprClient, ILogger<OrderController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            _logger.LogInformation($"Starting workflow for order ID: {orderDto.OrderId}");

            List<OrderItem> orderItemMapp = orderDto.OrderItemsDto.Select(x => new OrderItem
            {
                ItemType = x.ItemType,
                Quantity = x.Quantity,
                Price = x.Price,
                Size = x.Size,
                Color = x.Color
            }).ToList();

            Customer customerMapp = new Customer
            {
                Name = orderDto.CustomerDto.Name,
                Email = orderDto.CustomerDto.Email
            };

            Order orderMapp = new Order
            {
                OrderId = orderDto.OrderId,
                OrderItems = orderItemMapp,
                OrderDate = orderDto.OrderDate,
                Customer = customerMapp,
                OrderStatus = orderDto.OrderStatus,
                TotalAmount = orderDto.TotalAmount
            };





            try
            {
                var workflowInstanceId = await _daprClient.StartWorkflowAsync(
                    _workflowComponentName,
                    _workflowName,
                    orderMapp.OrderId,
                    orderMapp);

                _logger.LogInformation($"Workflow started with instance ID: {workflowInstanceId}");
                return Ok(new { OrderId = orderDto.OrderId, WorkflowInstanceId = workflowInstanceId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to start workflow for order ID: {orderDto.OrderId}");
                return StatusCode(500, "Failed to start order workflow.");
            }
        }
    }
}

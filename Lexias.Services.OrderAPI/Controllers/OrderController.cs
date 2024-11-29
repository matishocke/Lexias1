using Dapr.Client;
using Lexias.Services.OrderAPI.DaprWorkflow;
using Lexias.Services.OrderAPI.Data.Repository.IRepository;
using Lexias.Services.OrderAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.OrderDto;

namespace Lexias.Services.OrderAPI.Controllers
{
    [Route("api/order")] // denne skal skiftes 
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _db; //bring data


        public OrderController(DaprClient daprClient, ILogger<OrderController> logger, IOrderRepository orderRepository)
        {
            _daprClient = daprClient;
            _logger = logger;
            _db = orderRepository;
        }


        //Create
        //If Create Order workflow starts
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {

            var instanceId = Guid.NewGuid().ToString();
            var workflowComponentName =
                "dapr"; // alternatively, this could be the name of a workflow component defined in yaml
            var workflowName = nameof(OrderWorkflow); //"MyWorkflowDefinition";
            try
            {


                // Start the workflow with the OrderDto object
                var startResponse =
                     await _daprClient.StartWorkflowAsync(workflowComponentName, workflowName, instanceId, orderDto);
                _logger.LogInformation($"Workflow started: WorkflowId={startResponse.InstanceId}");

                return Ok(startResponse);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

            
        }



        // Get Orders
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                // Fetch all orders using the repository
                var orders = await _db.GetAllOrdersAsync();

                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found.");
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching orders: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching orders.");
            }
        }



        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            try
            {
                var order = await _db.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { Message = $"Order with ID {orderId} not found." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order with ID {orderId}: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the order.");
            }
        }

    }
}


using Dapr.Client;
using Lexias.Services.OrderAPI.DaprWorkflow;
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

        public OrderController(DaprClient daprClient, ILogger<OrderController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
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

            // Start the workflow with the OrderDto object
            var startResponse =
                 await _daprClient.StartWorkflowAsync(workflowComponentName, workflowName, instanceId, orderDto);


            _logger.LogInformation($"Workflow started: WorkflowId={startResponse.InstanceId}");

            return Ok(startResponse);
        }
    }
}


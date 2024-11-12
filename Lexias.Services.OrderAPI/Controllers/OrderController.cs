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
        public OrderController(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }



        //If Create Order workflow starts
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            // Start the workflow with the OrderDto object
            var startResponse =
                await _daprClient.StartWorkflowAsync("dapr", nameof(OrderWorkflow), Guid.NewGuid().ToString(), orderDto);

            return Ok(startResponse);
        }
    }
}


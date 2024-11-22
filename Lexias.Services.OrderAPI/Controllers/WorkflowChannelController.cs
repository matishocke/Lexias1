using Dapr;
using Dapr.Client;
using Lexias.Services.OrderAPI.DaprWorkflow.External;
using Microsoft.AspNetCore.Mvc;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.OrderAPI.Controllers
{
    [Route("api/workflowChannel")]
    [ApiController]
    public class WorkflowChannelController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<WorkflowChannelController> _logger;
        private readonly string _workflowComponentName = "dapr";

        public WorkflowChannelController(DaprClient daprClient, ILogger<WorkflowChannelController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }





        [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentResult)]
        [HttpPost("payment-result")]
        public async Task<IActionResult> PaymentResult([FromBody] PaymentProcessedResultEvent paymentProcessedResultEvent)
        {
            _logger.LogInformation($"Payment response received: Id: {paymentProcessedResultEvent.CorrelationId}, " +
                $"Amount: {paymentProcessedResultEvent.Amount}, State: {paymentProcessedResultEvent.State}");


            await _daprClient.RaiseWorkflowEventAsync
                (paymentProcessedResultEvent.CorrelationId,
                _workflowComponentName,
                WorkflowChannelEvents.PaymentEvent,
                paymentProcessedResultEvent);


            _logger.LogInformation("Payment response sent to workflow");
            return Ok();
        }





        [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult)]
        [HttpPost("reservation-result")]
        public async Task<IActionResult> ReservationResult([FromBody] ItemsReservedResultEvent itemsReservedResultEvent)
        {
            _logger.LogInformation($"Reservation response received: ID: {itemsReservedResultEvent.CorrelationId}");


            await _daprClient.RaiseWorkflowEventAsync
                (itemsReservedResultEvent.CorrelationId,
                _workflowComponentName,
                WorkflowChannelEvents.ItemReservedEvent,
                itemsReservedResultEvent);
            
            
            _logger.LogInformation("Reservation response sent to workflow");
            return Ok();
        }
    }
}

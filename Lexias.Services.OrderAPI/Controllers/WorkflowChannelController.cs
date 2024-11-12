using Dapr;
using Dapr.Client;
using Lexias.Services.OrderAPI.DaprWorkflow.External;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Enum;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.OrderAPI.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [HttpPost]
        public async Task<IActionResult> PaymentResult([FromBody] PaymentProcessedResultEvent paymentResponse)
        {
            _logger.LogInformation($"Payment response received: Id: {paymentResponse.CorrelationId}, " +
                $"Amount: {paymentResponse.Amount}, State: {paymentResponse.State}");


            await _daprClient.RaiseWorkflowEventAsync
                (paymentResponse.CorrelationId,
                _workflowComponentName,
                WorkflowChannelEvents.PaymentEvent,
                paymentResponse);


            _logger.LogInformation("Payment response sent to workflow");
            return Ok();
        }





        [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult)]
        [HttpPost]
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










    //[Route("api/[controller]")]
    //[ApiController]
    //public class WorkflowChannelController : ControllerBase
    //{
    //    private readonly DaprClient _daprClient;
    //    private readonly ILogger<WorkflowChannelController> _logger;
    //    private readonly string _workflowComponentName = "dapr";



    //    public WorkflowChannelController(DaprClient daprClient, ILogger<WorkflowChannelController> logger)
    //    {
    //        _daprClient = daprClient;
    //        _logger = logger;
    //    }






    //    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentResult)]
    //    [HttpPost("paymentresult")]
    //    public async Task<IActionResult> HandlePaymentResult([FromBody] PaymentProcessedResultEvent paymentEvent)
    //    {
    //        _logger.LogInformation($"Received payment result for OrderId: {paymentEvent.CorrelationId}");

    //        var statusEvent = paymentEvent.IsSuccessful
    //            ? OrderStatus.PaymentSuccess
    //            : OrderStatus.PaymentFailed;

    //        // Update Workflow State with Payment Result
    //        await _daprClient.RaiseWorkflowEventAsync(

    //            paymentEvent.CorrelationId,
    //            _workflowComponentName,
    //            eventName: "PaymentEvent",
    //            eventData: paymentEvent);

    //        return Ok();
    //    }




    //    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult)]
    //    [HttpPost("itemsreserveresult")]
    //    public async Task<IActionResult> HandleItemsReserveResult([FromBody] ItemsReservedResultEvent itemsReservedEvent)
    //    {
    //        _logger.LogInformation($"Received items reservation result for OrderId: {itemsReservedEvent.CorrelationId}");

    //        var statusEvent = itemsReservedEvent.IsSuccessful
    //            ? OrderStatus.SufficientInventory
    //            : OrderStatus.InsufficientInventory;

    //        // Update Workflow State with Items Reservation Result
    //        await _daprClient.RaiseWorkflowEventAsync(
    //            itemsReservedEvent.CorrelationId,
    //            _workflowComponentName,
    //            eventName: "ItemsReservedEvent",
    //            eventData: itemsReservedEvent);

    //        return Ok();
    //    }



    //    [Topic(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsShippedResult)]
    //    [HttpPost("itemsshippedresult")]
    //    public async Task<IActionResult> HandleItemsShippedResult([FromBody] ItemsShippedResultEvent itemsShippedEvent)
    //    {
    //        _logger.LogInformation($"Received items shipped result for OrderId: {itemsShippedEvent.CorrelationId}");

    //        var statusEvent = itemsShippedEvent.IsSuccessful
    //            ? OrderStatus.ShippingItems
    //            : OrderStatus.ShippingItemsFailed;

    //        // Update Workflow State with Items Shipped Result
    //        await _daprClient.RaiseWorkflowEventAsync(
    //            itemsShippedEvent.CorrelationId,
    //            _workflowComponentName,
    //            eventName: "ItemsShippedEvent",
    //            eventData: itemsShippedEvent);

    //        return Ok();
    //    }
    //}
}

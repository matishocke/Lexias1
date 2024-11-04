using Dapr;
using Dapr.Client;
using Lexias.Services.PaymentAPI.Data;
using Lexias.Services.PaymentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Enum;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.PaymentAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContextPayment _context;
        private readonly DaprClient _daprClient;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(DaprClient daprClient, ILogger<PaymentController> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        [Topic(PaymentChannel.Channel, PaymentChannel.Topics.Payment)]
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentEvent paymentRequest)
        {
            _logger.LogInformation("Processing payment: {CorrelationId}, Amount: {Amount}", paymentRequest.CorrelationId, paymentRequest.Amount);

            // Save payment details to database
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid().ToString(),
                OrderId = paymentRequest.CorrelationId,
                Amount = paymentRequest.Amount,
                PaymentDate = DateTime.UtcNow,
                Status = PaymentStatus.Completed // Assuming payment succeeds
            };

            _context.Orders.Add(payment);
            await _context.SaveChangesAsync();

            // For this example, assume payment always succeeds
            var paymentResponse = new PaymentProcessedResultEvent
            {
                CorrelationId = paymentRequest.CorrelationId,
                Amount = paymentRequest.Amount,
                State = ResultState.Succeeded
            };

            // Publish the payment result event to the workflow channel
            await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.PaymentResult, paymentResponse);

            _logger.LogInformation("Payment processed: {CorrelationId}, {Amount}, {State}", paymentResponse.CorrelationId, paymentResponse.Amount, paymentResponse.State);
            return Ok(paymentResponse);
        }
    }
}

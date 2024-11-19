using Dapr.Client;
using Dapr.Workflow;
using Shared.Dtos.PaymentDto;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class ProcessPaymentActivity : WorkflowActivity<PaymentDto, object?>
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ProcessPaymentActivity> _logger;

        public ProcessPaymentActivity(DaprClient daprClient, ILogger<ProcessPaymentActivity> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }


        public override async Task<object?> RunAsync(WorkflowActivityContext context, PaymentDto paymentDto)
        {
            // Log that the payment is being processed
            _logger.LogInformation($"Processing payment for OrderId: {paymentDto.OrderId}, Amount: {paymentDto.Amount}");

            // Create payment request event
            var processPaymentEvent = new ProcessPaymentEvent
            {
                CorrelationId = context.InstanceId,
                Amount = paymentDto.Amount,
                OrderId = paymentDto.OrderId
            };

            // Publish the payment request event
            await _daprClient.PublishEventAsync(
                PaymentChannel.Channel,
                PaymentChannel.Topics.Payment,
                processPaymentEvent);

            return null;
        }
    }
}

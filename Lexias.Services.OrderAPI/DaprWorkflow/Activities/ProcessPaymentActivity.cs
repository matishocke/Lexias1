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

            try
            {
                // Simulate successful payment
                var processPaymentEvent = new ProcessPaymentEvent
                {
                    CorrelationId = context.InstanceId,
                    Amount = paymentDto.Amount,
                    OrderId = paymentDto.OrderId
                };

                await _daprClient.PublishEventAsync(
                    PaymentChannel.Channel,
                    PaymentChannel.Topics.Payment,
                    processPaymentEvent);

                _logger.LogInformation($"Payment processed successfully for OrderId: {paymentDto.OrderId}");
            }
            catch (Exception ex)
            {
                // Log and rethrow if needed, even if you assume success
                _logger.LogError("Unexpected error during payment processing: {ErrorMessage}", ex.Message);
                throw;
            }

            return null; // Success is always assumed
        }
    }
}

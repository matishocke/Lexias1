using Dapr.Workflow;
using Shared.Dtos.PaymentDto;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    public class ProcessPaymentActivity : WorkflowActivity<PaymentDto, object?>
    {
        private readonly ILogger<ProcessPaymentActivity> _logger;

        public ProcessPaymentActivity(ILogger<ProcessPaymentActivity> logger)
        {
            _logger = logger;
        }

        public override async Task<object?> RunAsync(WorkflowActivityContext context, PaymentDto paymentDto)
        {
            _logger.LogInformation($"Processing payment for OrderId: {context.InstanceId}, Amount: {paymentDto.Amount}");
            await Task.CompletedTask;
            return null;
        }
    }
}

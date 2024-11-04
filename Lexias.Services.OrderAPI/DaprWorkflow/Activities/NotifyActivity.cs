using Dapr.Workflow;
using Shared.Dtos.Notification;

namespace Lexias.Services.OrderAPI.DaprWorkflow.Activities
{
    //input of type Notification
    //output is null
    public class NotifyActivity : WorkflowActivity<Notification, object?>
    {
        private readonly ILogger<NotifyActivity> _logger;

        public NotifyActivity(ILogger<NotifyActivity> logger)
        {
            _logger = logger;
        }


        //WorkflowActivityContext: it provides information like the current state or metadata about the workflow instance running this activity.
        //Notification: This is the input data
        public override async Task<object?> RunAsync(WorkflowActivityContext context, Notification notification)
        {
            _logger.LogInformation(notification.Message);
            await Task.CompletedTask;
            return null;
        }
    }
}

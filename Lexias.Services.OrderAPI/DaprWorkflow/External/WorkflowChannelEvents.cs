namespace Lexias.Services.OrderAPI.DaprWorkflow.External
{
    //This Events are for those Waiting
    public class WorkflowChannelEvents
    {
        public static readonly string PaymentEvent = "PaymentEvent";
        public static readonly string ItemReservedEvent = "ItemReservedEvent";
        public static readonly string ItemShippedEvent = "ItemShippedEvent";
    }
}

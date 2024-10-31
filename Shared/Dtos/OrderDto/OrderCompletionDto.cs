namespace Shared.Dtos.OrderDto
{
    public class OrderCompletionDto
    {
        public string OrderId { get; set; }
        public DateTime CompletionDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; } // If the order fails, provide a reason
    }
}

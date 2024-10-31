namespace Shared.Dtos.OrderDto
{
    public class PaymentResultDto
    {
        public string OrderId { get; set; }
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; } // To indicate why payment might have failed
    }
}

namespace Shared.Dtos.OrderDto
{
    public class PaymentRequestDto
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public CustomerDto Customer { get; set; }
    }
}

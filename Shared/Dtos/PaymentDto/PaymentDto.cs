using Shared.Enum;
using System.Text.Json.Serialization;


namespace Shared.Dtos.PaymentDto
{
    public class PaymentDto
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
    }

}

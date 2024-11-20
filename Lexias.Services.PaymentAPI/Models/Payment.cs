using Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace Lexias.Services.PaymentAPI.Models
{
    public class Payment
    {
        [Key]
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }

}


using Shared.Enum;

namespace Lexias.Services.OrderAPI.Models
{
    public class OrderResult
    {
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Message { get; set; }
    }
}

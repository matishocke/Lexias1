using Castle.Core.Resource;
using Shared.Enum;

namespace Lexias.Services.OrderAPI.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
    }
}

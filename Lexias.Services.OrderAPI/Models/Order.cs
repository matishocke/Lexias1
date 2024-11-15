using Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace Lexias.Services.OrderAPI.Models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }
        public List<OrderItem> OrderItemsList { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
    }
}

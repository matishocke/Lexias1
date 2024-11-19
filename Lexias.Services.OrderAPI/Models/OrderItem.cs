
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lexias.Services.OrderAPI.Models
{
    public class OrderItem
    {
        [Key]
        public string OrderItemId { get; set; }

        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public Order? Order { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}


using Shared.Enum;

namespace Lexias.Services.OrderAPI.Models
{
    public class OrderItem
    {
        public ItemType ItemType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // Price per item
        public string Size { get; set; }    // For clothing, size is important
        public string Color { get; set; }
    }
}

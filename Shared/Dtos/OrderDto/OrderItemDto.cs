using Shared.Enum;

namespace Shared.Dtos.OrderDto

{
    public class OrderItemDto
    {
        public ItemType ItemType { get; set; }   // Enum like Computer, Monitor, etc.
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // Price per item
        public string Size { get; set; }    // For clothing, size is important
        public string Color { get; set; }
    }
}

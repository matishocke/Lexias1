namespace Shared.Dtos.OrderDto
{
    public class InventoryResultDto
    {
        public bool IsSufficientInventory { get; set; }
        public List<OrderItemDto> ItemsInStock { get; set; } // Optionally include items available
    }
}

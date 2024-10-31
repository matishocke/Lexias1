using Shared.Enum;

namespace Shared.Dtos.OrderDto

{
    public class OrderDto
    {
        public string OrderId { get; set; }
        public List<OrderItemDto> OrderItemsDto { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerDto CustomerDto { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

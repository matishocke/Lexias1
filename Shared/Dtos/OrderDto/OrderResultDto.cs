using Shared.Enum;


namespace Shared.Dtos.OrderDto
{
    public class OrderResultDto
    {
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Message { get; set; }
    }
}

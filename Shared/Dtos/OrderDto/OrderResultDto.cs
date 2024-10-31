using Shared.Enum;

namespace Shared.Dtos.OrderDto

{
    public class OrderResultDto
    {
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }  // Any additional information or error message

        public OrderResultDto(string orderId, OrderStatus orderStatus, bool isSuccessful, string message = "")
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}

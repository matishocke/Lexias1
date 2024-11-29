
using Order = Shared.Dtos.OrderDto.OrderDto;


namespace Shared.Dtos.Notification
{
    public class Notification
    {
        public string Message { get; set; }
        public Order Order { get; set; }

        public Notification(string message, Order order)
        {
            Message = message;
            Order = order;
        }
    }
}

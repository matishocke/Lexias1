using UmbracoLexiasWeb.Models.Dtos.OrderDto;

namespace UmbracoLexiasWeb.Services.IService
{
    public interface IGatewayOrderService
    {
        Task<List<OrderDto>?> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(string orderId);
        Task<OrderDto?> GetOrderByCustomerId(string customerId);
        Task CreateOrderAsync(OrderDto orderDto);
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(string orderId);

    }
}

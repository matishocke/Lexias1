using UmbracoLexiasWeb.Models.Dtos.OrderDto;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Services
{
    public class GatewayOrderService : IGatewayOrderService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GatewayOrderService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }





        //All Orders
        public async Task<List<OrderDto>?> GetAllOrdersAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                var orders = await client.GetFromJsonAsync<List<OrderDto>>("/gateway/Orders");
                return orders;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching orders: {ex.Message}");
            }
        }


        //Order By Id
        public async Task<OrderDto?> GetOrderByIdAsync(string orderId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                var order = await client.GetFromJsonAsync<OrderDto>($"/gateway/Orders/{orderId}");
                return order;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching order with ID {orderId}: {ex.Message}");
            }
        }



        //Order by Customer
        public async Task<OrderDto?> GetOrderByCustomerId(string customerId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                return await client.GetFromJsonAsync<OrderDto>($"gateway/Orders/Customer/{customerId}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to fetch order by customer ID: {ex.Message}");
            }
        }

        //Create
        public async Task CreateOrderAsync(OrderDto orderDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                var response = await client.PostAsJsonAsync("/gateway/Orders", orderDto);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating order: {message}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error creating order: {ex.Message}");
            }
        }

        //Update
        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                var response = await client.PutAsJsonAsync($"/gateway/Orders/{orderDto.OrderId}", orderDto);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error updating order: {message}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error updating order: {ex.Message}");
            }
        }

        //Delete
        public async Task DeleteOrderAsync(string orderId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                var response = await client.DeleteAsync($"/gateway/Orders/{orderId}");

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error deleting order: {message}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error deleting order: {ex.Message}");
            }
        }
    }
}

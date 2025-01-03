using UmbracoLexiasWeb.Models.Dtos.ProductDto;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Services
{
    public class GatewayProductService : IGatewayProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GatewayProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }





        //All Products
        public async Task<List<ProductDto>?> GetAllProductsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GatewayAPI");
                var orders = await client.GetFromJsonAsync<List<ProductDto>>("/gateway/Products");
                return orders;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching orders: {ex.Message}");
            }
        }
    }
}

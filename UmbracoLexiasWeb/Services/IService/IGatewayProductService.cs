using UmbracoLexiasWeb.Models.Dtos.ProductDto;

namespace UmbracoLexiasWeb.Services.IService
{
    public interface IGatewayProductService
    {
        Task<List<ProductDto>?> GetAllProductsAsync();
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Controller
{
    public class ProductSurfaceController : SurfaceController
    {
        private readonly IGatewayProductService _gatewayProductService;
        public ProductSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, 
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IGatewayProductService gatewayProductService) 
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _gatewayProductService = gatewayProductService;
        }


        // GET: Retrieve and display all orders
        public async Task<IActionResult> ProductIndex()
        {
            try
            {
                var products = await _gatewayProductService.GetAllProductsAsync();
                return View("ProductIndex", products); // Ensure you have a corresponding view named "OrderIndex"
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error fetching products: {ex.Message}";
                return RedirectToCurrentUmbracoPage();
            }
        }
    }
}

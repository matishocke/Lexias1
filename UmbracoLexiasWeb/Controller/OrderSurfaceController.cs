using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoLexiasWeb.Models.Dtos.OrderDto;
using UmbracoLexiasWeb.Models.ViewModels.OrderViewModel;
using UmbracoLexiasWeb.Services.IService;

namespace UmbracoLexiasWeb.Controller
{
    public class OrderSurfaceController : SurfaceController
    {
        private readonly IGatewayOrderService _gatewayOrderService;
        public OrderSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory, 
            ServiceContext services,
            AppCaches appCaches, 
            IProfilingLogger profilingLogger, 
            IPublishedUrlProvider publishedUrlProvider, 
            IGatewayOrderService gatewayOrderService) 
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _gatewayOrderService = gatewayOrderService;
        }


        //// GET: Retrieve and display all orders
        //public async Task<IActionResult> OrderIndex()
        //{
        //    try
        //    {
        //        var orders = await _gatewayOrderService.GetAllOrdersAsync();
        //        return View("OrderIndex", orders); // Ensure you have a corresponding view named "OrderIndex"
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Error fetching orders: {ex.Message}";
        //        return RedirectToCurrentUmbracoPage();
        //    }
        //}

        //// GET: Retrieve order details by ID
        //public async Task<IActionResult> OrderDetails(string orderId)
        //{
        //    try
        //    {
        //        var order = await _gatewayOrderService.GetOrderByIdAsync(orderId);
        //        if (order == null)
        //        {
        //            TempData["Error"] = "Order not found.";
        //            return RedirectToCurrentUmbracoPage();
        //        }

        //        return View("OrderDetails", order); // Ensure you have a corresponding view named "OrderDetails"
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = $"Error fetching order details: {ex.Message}";
        //        return RedirectToCurrentUmbracoPage();
        //    }
        //}



        [HttpPost]
        public IActionResult SearchOrder(string searchOrderId)
        {
            List<OrderViewModel> orderList = new List<OrderViewModel>();

            try
            {
                if (!string.IsNullOrEmpty(searchOrderId))
                {
                    // Fetch the specific order
                    var orderDto = _gatewayOrderService.GetOrderByIdAsync(searchOrderId).Result;
                    if (orderDto != null)
                    {
                        orderList.Add(new OrderViewModel
                        {
                            OrderId = orderDto.OrderId,
                            CustomerId = orderDto.CustomerId,
                            OrderDate = orderDto.OrderDate,
                            TotalAmount = orderDto.TotalAmount,
                            Status = orderDto.Status.ToString(),
                            OrderItemsList = orderDto.OrderItemsList.Select(item => new OrderItemViewModel
                            {
                                OrderItemId = item.OrderItemId,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity
                            }).ToList()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error occurred while searching for the order: {ex.Message}";
            }

            TempData["OrderList"] = JsonConvert.SerializeObject(orderList);
            return RedirectToCurrentUmbracoPage();
        }






        // POST: Create a new order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid order data.";
                return CurrentUmbracoPage();
            }

            try
            {
                await _gatewayOrderService.CreateOrderAsync(orderDto);
                TempData["Success"] = "Order created successfully!";
                return RedirectToAction("OrderIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating order: {ex.Message}";
                return CurrentUmbracoPage();
            }
        }

        // POST: Update an existing order
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid order data.";
                return CurrentUmbracoPage();
            }

            try
            {
                await _gatewayOrderService.UpdateOrderAsync(orderDto);
                TempData["Success"] = "Order updated successfully!";
                return RedirectToAction("OrderIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating order: {ex.Message}";
                return CurrentUmbracoPage();
            }
        }

        // POST: Delete an order by ID
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                TempData["Error"] = "Invalid order ID.";
                return CurrentUmbracoPage();
            }

            try
            {
                await _gatewayOrderService.DeleteOrderAsync(orderId);
                TempData["Success"] = "Order deleted successfully!";
                return RedirectToAction("OrderIndex");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting order: {ex.Message}";
                return CurrentUmbracoPage();
            }
        }

    }
}

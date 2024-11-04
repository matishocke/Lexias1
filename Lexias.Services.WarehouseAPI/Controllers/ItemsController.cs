using Dapr;
using Dapr.Client;
using Lexias.Services.WarehouseAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Enum;
using Shared.IntegrationEvents;
using Shared.Queues;

namespace Lexias.Services.WarehouseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ItemsController> _logger;
        private readonly AppDbContextWarehouse _context;

        public ItemsController(DaprClient daprClient, ILogger<ItemsController> logger, AppDbContextWarehouse context)
        {
            _daprClient = daprClient;
            _logger = logger;
            _context = context;
        }






        [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Reservation)]
        [HttpPost]
        public async Task<IActionResult> ReserveItems([FromBody] ReserveItemsEvent reserveItemsRequest)
        {
            _logger.LogInformation($"Inventory request received: {reserveItemsRequest.CorrelationId}");

            // Check if all items are available in the inventory
            bool itemsAvailable = true;
            foreach (var item in reserveItemsRequest.Items)
            {
                var product = _context.Orders.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    itemsAvailable = false;
                    break;
                }
            }

            if (!itemsAvailable)
            {
                var reservationFailedResponse = new ItemsReservationFailedEvent
                {
                    CorrelationId = reserveItemsRequest.CorrelationId,
                    State = ResultState.Failed,
                    Items = reserveItemsRequest.Items,
                    Reason = "Insufficient inventory"
                };

                await _daprClient.PublishEventAsync(WarehouseChannel.Channel, WarehouseChannel.Topics.ReservationFailed, reservationFailedResponse);
                _logger.LogInformation($"Reservation failed for Order: {reservationFailedResponse.CorrelationId}, Reason: {reservationFailedResponse.Reason}");
                return BadRequest(reservationFailedResponse);
            }

            // Update inventory for each item
            foreach (var item in reserveItemsRequest.Items)
            {
                var product = _context.Orders.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    _context.Orders.Update(product);
                }
            }

            await _context.SaveChangesAsync();

            // Publish successful reservation event
            var itemsReservedResponse = new ItemsReservedResultEvent
            {
                CorrelationId = reserveItemsRequest.CorrelationId,
                State = ResultState.Succeeded
            };

            await _daprClient.PublishEventAsync(WorkflowChannel.Channel, WorkflowChannel.Topics.ItemsReserveResult, itemsReservedResponse);
            _logger.LogInformation($"Items reserved: {itemsReservedResponse.CorrelationId}, State: {itemsReservedResponse.State}");
            return Ok(itemsReservedResponse);
        }





        [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.ReservationFailed)]
        [HttpPost]
        public async Task<IActionResult> UnreserveItems([FromBody] ItemsReservationFailedEvent unreserveItemsRequest)
        {
            _logger.LogInformation($"Unreserve request received: {unreserveItemsRequest.CorrelationId}");

            // Logic to unreserve items in the inventory
            foreach (var item in unreserveItemsRequest.Items)
            {
                var product = _context.Orders.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    _context.Orders.Update(product);
                }
                _logger.LogInformation($"Unreserving item: {item.ItemType}, Quantity: {item.Quantity}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

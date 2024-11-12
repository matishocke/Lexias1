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




        //Lytter
        //[Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Reservation)] attribute is essentially making
        //this endpoint a //Listener// for events on that channel and topic.
        [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.Reservation)]
        [HttpPost]
        public async Task<IActionResult> ReserveItems([FromBody] ReserveItemsEvent reserveItemsEvent) //[FromBody] with this we will take data from the endpoint to use them right here 
        {
            _logger.LogInformation($"Inventory request received: {reserveItemsEvent.CorrelationId}");

            //+ Check if all items are available in the inventory
            bool itemsAvailable = true;


            foreach (var item in reserveItemsEvent.OrderItemsList)
            {
                //Check this Product exist
                var product = _context.Orders.FirstOrDefault(p => p.ProductId == item.ProductId);

                //Check are there enough of these Product
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    itemsAvailable = false;
                    break;
                }
            }


            //Failed
            //- If item is not available
            if (!itemsAvailable)
            {
                var reservationFailedResponse = new ItemsReservationFailedEvent
                {
                    CorrelationId = reserveItemsEvent.CorrelationId,
                    State = ResultState.Failed,
                    OrderItems = reserveItemsEvent.OrderItemsList,
                    Reason = "Insufficient inventory"
                };

                //Sender //now from here we will publish the data back
                await _daprClient.PublishEventAsync(
                    WarehouseChannel.Channel,
                    WarehouseChannel.Topics.ReservationFailed,
                    reservationFailedResponse);

                _logger.LogInformation($"Reservation failed for Order: {reservationFailedResponse.CorrelationId}, Reason: {reservationFailedResponse.Reason}");
                return BadRequest(reservationFailedResponse);
            }


            //Success
            // Update inventory for each item
            foreach (var item in reserveItemsEvent.OrderItemsList)
            {
                var product = _context.Orders.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    //ensures we never end up with negative stock (Math.Max)
                    product.StockQuantity = Math.Max(0, product.StockQuantity - item.Quantity);
                    _context.Orders.Update(product);
                }
            }

            await _context.SaveChangesAsync();



            // Publish successful reservation event
            var itemsReservedResultEvent = new ItemsReservedResultEvent
            {
                CorrelationId = reserveItemsEvent.CorrelationId,
                State = ResultState.Succeeded
            };
            
            //Sender //now from here we will publish the data back
            await _daprClient.PublishEventAsync(
                WorkflowChannel.Channel,
                WorkflowChannel.Topics.ItemsReserveResult, 
                itemsReservedResultEvent);
            
            
            
            _logger.LogInformation($"Items reserved: {itemsReservedResultEvent.CorrelationId}, State: {itemsReservedResultEvent.State}");
            return Ok(itemsReservedResultEvent);
        }







        [Topic(WarehouseChannel.Channel, WarehouseChannel.Topics.ReservationFailed)]
        [HttpPost]
        public async Task<IActionResult> UnreserveItems([FromBody] ItemsReservationFailedEvent unreserveItemsRequest)
        {
            _logger.LogInformation($"Unreserve request received: {unreserveItemsRequest.CorrelationId}");

            // Logic to unreserve items in the inventory
            foreach (var item in unreserveItemsRequest.OrderItems)
            {
                var product = _context.Orders.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity = product.StockQuantity + item.Quantity;
                    _context.Orders.Update(product);
                }
                _logger.LogInformation($"Unreserving item: {item.ItemType}, Quantity: {item.Quantity}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

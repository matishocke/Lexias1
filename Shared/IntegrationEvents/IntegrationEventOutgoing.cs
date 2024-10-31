using Shared.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IntegrationEvents
{
    public record IntegrationEventOutgoing
    {
        public string CorrelationId { get; set; } = string.Empty;
    }

    public record ProcessPaymentEvent : IntegrationEventOutgoing
    {
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
    }

    public record ReserveItemsEvent : IntegrationEventOutgoing
    {
        public List<OrderItemDto> Items { get; set; }
        public string OrderId { get; set; }
    }

    public record ShipItemsEvent : IntegrationEventOutgoing
    {
        public List<OrderItemDto> Items { get; set; }
        public string OrderId { get; set; }
    }
}

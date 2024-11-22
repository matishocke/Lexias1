using Shared.Dtos.OrderDto;
using Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IntegrationEvents
{
    //These work as Result/Response
    public abstract class IntegrationEventIncoming
    {
        public string CorrelationId { get; set; } = string.Empty;
        public ResultState State { get; set; } = ResultState.Failed;
    }




    //Result Scenarios
    public class PaymentProcessedResultEvent : IntegrationEventIncoming
    {
        public decimal Amount { get; set; }
    }

    public class ItemsReservedResultEvent : IntegrationEventIncoming
    {
        public decimal TotalAmount { get; set; }
    }




    //Failed Scenarios
    public class ItemsReservationFailedEvent : IntegrationEventIncoming
    {
        public List<OrderItemDto> OrderItems { get; set; } = new();
        public string Reason { get; set; } = string.Empty;
    }











    //    public abstract record IntegrationEventIncoming
    //    {
    //        public string CorrelationId { get; init; } = string.Empty;
    //        public ResultState State { get; init; } = ResultState.Failed;
    //    }

    //    public record PaymentProcessedResultEvent : IntegrationEventIncoming
    //    {
    //        public decimal Amount { get; set; }
    //        public bool IsSuccessful => State == ResultState.Succeeded;
    //    }

    //    public record ItemsReservedResultEvent : IntegrationEventIncoming
    //    {
    //        public bool IsSuccessful => State == ResultState.Succeeded;
    //    }

    //    public record ItemsShippedResultEvent : IntegrationEventIncoming
    //    {
    //        public bool IsSuccessful => State == ResultState.Succeeded;
    //    }

    //    public record OrderCompletedEvent : IntegrationEventIncoming { }

    //    public abstract record FailedEvent : IntegrationEventIncoming
    //    {
    //        public string Reason { get; set; } = string.Empty;
    //    }

    //    public record OrderFailedEvent : FailedEvent { }
    //    public record PaymentFailedEvent : FailedEvent { }
    //    public record ItemsReservationFailedEvent : FailedEvent { }
    //    public record ItemsShipmentFailedEvent : FailedEvent { }
}
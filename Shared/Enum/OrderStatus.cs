using System.Text.Json.Serialization;

namespace Shared.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending,
        Received,
        ReservingInventory,
        InventoryReserved,
        CheckingPayment,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }
}

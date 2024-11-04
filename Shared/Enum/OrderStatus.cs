using System.Text.Json.Serialization;

namespace Shared.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled,
        CheckingPayment
    }
}

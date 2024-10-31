using System.Text.Json.Serialization;

namespace Shared.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Received = 0,
        CheckingInventory = 1,
        SufficientInventory = 2,
        InsufficientInventory = 3,
        CheckingPayment = 4,
        PaymentSuccess = 5,
        PaymentFailed = 6,
        ShippingItems = 7,
        ShippingItemsFailed = 8,
        Error = 9,
        Restocked = 10,
        Refunded = 11,
        Unreserved = 12,
        Completed = 13,
        Created = 14
    }
}

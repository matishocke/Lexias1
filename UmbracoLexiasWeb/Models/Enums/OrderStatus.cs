namespace UmbracoLexiasWeb.Models.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Received = 1,
        ReservingInventory = 2,
        InventoryReserved = 3,
        CheckingPayment = 4,
        Confirmed = 5,
        Shipped = 6,
        Delivered = 7,
        Cancelled = 8
    }
}

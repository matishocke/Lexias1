namespace Lexias.Services.OrderAPI.Models
{
    public class Cart
    {
        public string CartId { get; set; }
        public string CustomerId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount => CartItems?.Sum(item => item.TotalPrice) ?? 0;
    }

}

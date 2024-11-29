namespace UmbracoLexiasWeb.Models.ViewModels.OrderViewModel
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public List<OrderItemViewModel> OrderItemsList { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

namespace Lexias.Services.WarehouseAPI.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } // Example: "Men", "Women", "Accessories", etc.
        public List<string> Sizes { get; set; } // Example: ["S", "M", "L", "XL"]
        public string Color { get; set; }
        public List<string> Images { get; set; }
    }

}

﻿
using Shared.Enum;

namespace Lexias.Services.OrderAPI.Models
{
    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ItemType ItemType { get; set; }
    }
}

﻿using System.Text.Json.Serialization;

namespace Shared.Dtos.OrderDto
{
    public class OrderItemDto
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

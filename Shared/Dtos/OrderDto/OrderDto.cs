﻿using Shared.Enum;
using System.Text.Json.Serialization;

namespace Shared.Dtos.OrderDto
{
    public class OrderDto
    {
        public string OrderId { get; set; }
        public List<OrderItemDto> OrderItemsList { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

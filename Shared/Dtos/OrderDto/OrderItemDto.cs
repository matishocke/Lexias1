using Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderDto
{
    public class OrderItemDto
    {
        public string OrderItemId { get; set; }
        public string OrderId { get; set; }  // Added this field to match the OrderItem model
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

using Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderDto
{
    public class OrderDto
    {
        public string OrderId { get; set; }

        //maybe make another controller later 
        public List<OrderItemDto> OrderItemsList { get; set; }
        //public string OrderItemId { get; set; }
        //public int QuantityOrderItemsDto { get; set; }
        //public double PriceOrderItemDto { get; set; }
        //public string ProductId { get; set; }

        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

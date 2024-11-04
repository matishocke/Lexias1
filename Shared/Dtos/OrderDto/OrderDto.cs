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
        public List<OrderItemDto> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerDto CustomerDto { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

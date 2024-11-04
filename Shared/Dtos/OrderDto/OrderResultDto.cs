using Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderDto
{
    public class OrderResultDto
    {
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Message { get; set; }
    }
}

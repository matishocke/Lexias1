using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.OrderDto
{
    public class CartDto
    {
        public string CartId { get; set; }
        public string CustomerId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }

}

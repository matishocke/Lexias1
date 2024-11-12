using Shared.Dtos.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.WarehouseDto
{
    //A List of OrderItems
    public class InventoryRequestDto
    {
        public List<OrderItemDto> ItemsRequested { get; set; }

        public InventoryRequestDto(List<OrderItemDto> itemsRequested)
        {
            ItemsRequested = itemsRequested;
        }
    }
}

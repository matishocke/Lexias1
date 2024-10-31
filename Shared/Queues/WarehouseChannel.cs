using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Queues
{
    public class WarehouseChannel
    {
        public const string Channel = "warehousechannel";
        public class Topics
        {
            public const string Reservation = "reservation";
            public const string Shipment = "shipment";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Queues
{
    // channel/topic acts like a mailbox that stores messages
    public class WarehouseChannel
    {
        public const string Channel = "warehousechannel";
        public class Topics
        {
            public const string Reservation = "reservation";
            public const string ReservationFailed = "reservationFailed";
            public const string Shipment = "shipment";
        }
    }
}

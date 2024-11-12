using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Queues
{   
    // channel/topic acts like a mailbox that stores messages
    public class PaymentChannel
    {
        public const string Channel = "paymentchannel";
        public class Topics
        {
            public const string Payment = "payment";
            public const string Refund = "refund";
        }
    }
}

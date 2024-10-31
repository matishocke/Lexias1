using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Queues
{
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

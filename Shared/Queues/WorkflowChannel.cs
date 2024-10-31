using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Queues
{
    public class WorkflowChannel
    {
        public const string Channel = "workflowchannel";
        public class Topics
        {
            public const string PaymentResult = "paymentresult";
            public const string ItemsReserveResult = "itemsreserveresult";
            public const string ItemsShippedResult = "itemsshippedresult";
        }
    }
}

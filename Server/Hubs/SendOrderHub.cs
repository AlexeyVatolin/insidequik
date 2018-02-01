using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs
{
    class SendOrderHub : Hub
    {
        void SendOrder(Common.Models.ClientOrder order)
        {
            long transId = Quik.QuikData.NewOrder(order);
        }
        void SendStopOrder(Common.Models.ClientStopOrder stopOrder)
        {
            long transId = Quik.QuikData.NewStopOrder(stopOrder);
        }
    }
}

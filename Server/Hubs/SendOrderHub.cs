using Microsoft.AspNet.SignalR;
using ServerCore.Hubs;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class SendOrderHub : BaseHub<ClientBase, IHubClientCallbacks>
    {
        public void SendOrder(Common.Models.ClientOrder order)
        {
            long userID = long.Parse(GetCurrentClient().ApplicationUserId);
            long transId = Quik.QuikData.NewOrder(order, userID);
        }
        public void SendStopOrder(Common.Models.ClientStopOrder stopOrder)
        {
            long userID = long.Parse(GetCurrentClient().ApplicationUserId);
            long transId = Quik.QuikData.NewStopOrder(stopOrder, userID);
        }
    }
}

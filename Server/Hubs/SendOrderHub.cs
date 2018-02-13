using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class SendOrderHub : Hub
    {
        public void SendOrder(Common.Models.ClientOrder order)
        {
            long userLongID = 777;//TODO:user id
            long transId = Quik.QuikData.NewOrder(order, userLongID);
        }
        public void SendStopOrder(Common.Models.ClientStopOrder stopOrder)
        {
            long userLongID = 777;//TODO:user id
            long transId = Quik.QuikData.NewStopOrder(stopOrder, userLongID);
        }
    }
}

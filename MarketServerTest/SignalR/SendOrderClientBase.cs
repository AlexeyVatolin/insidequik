using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest.SignalR
{
    public class SendOrderClientBase:ClientBase
    {
        protected async Task SendOrder(ClientOrder order)
        {
            SetHubName("OrdersHub");
            StartConnection();
            try
            {
                await Invoke("SendOrder", order);
            }
            catch { }
        }
        protected async Task SendStopOrder(ClientStopOrder stopOrder)
        {
            SetHubName("OrdersHub");
            StartConnection();
            try
            {
                await Invoke("SendStopOrder", stopOrder);
            }
            catch { }
        }
    }
}

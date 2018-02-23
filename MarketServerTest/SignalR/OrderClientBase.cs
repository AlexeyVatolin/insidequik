using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest.SignalR
{
    public class OrderClientBase:ClientBase
    {
        protected async Task GetOrders()
        {
            SetHubName("OrdersHub");
            StartConnection();
            try
            {
                await Invoke("InitializeOrders");
            }
            catch { }
        }
    }
}

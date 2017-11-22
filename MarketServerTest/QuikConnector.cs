using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    static class QuikConnector
    {
        private static Quik Quik;
        public static bool isConnected { get; private set; }

        public static bool Connect()
        {
            Quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            if (Quik != null && Quik.Service.IsConnected().Result)
            {
                isConnected = true;
                return true;
            }
            isConnected = false;
            return false;
        }

        public static OrderBook SubscribeToOrderBook()
        {
            return new OrderBook();
        }


    }
}

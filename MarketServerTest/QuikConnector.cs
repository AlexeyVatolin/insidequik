using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp;
using QuikSharp.DataStructures;
using System.Windows;

namespace MarketServerTest
{
    static class QuikConnector
    {
        private static Quik Quik;
        public static bool isConnected { get; private set; }
        private static  List<QuikSharp.DataStructures.Transaction.Order> list = new List<QuikSharp.DataStructures.Transaction.Order>();
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

        public static List<QuikSharp.DataStructures.Transaction.Order> GetBids()
        {
            return Quik.Orders.GetOrders().Result;
        }

        public static OrderBook SubscribeToOrderBook()
        {
            return new OrderBook();
        }


    }
}

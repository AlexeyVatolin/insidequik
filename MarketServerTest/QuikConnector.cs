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
        public delegate void OnQuoteDoDelegate(OrderBook quote);

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

        public static void SubscribeToOrderBook(string ticker, QuoteHandler quoteDoDelegate)
        {
            //TODO: разобраться что значит эта мифическая строка SPBFUT,TQBR..
            string classCode = Quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB", ticker).Result;
            if (!string.IsNullOrEmpty(classCode))
            {
                var tool = new Tool(Quik, ticker, classCode);
                if (!string.IsNullOrEmpty(tool.Name))
                {
                    Quik.OrderBook.Subscribe(tool.ClassCode, tool.SecurityCode).Wait();
                    bool isSubscribedToolOrderBook = Quik.OrderBook.IsSubscribed(tool.ClassCode, tool.SecurityCode).Result;
                    if (isSubscribedToolOrderBook)
                    {
                        Console.WriteLine("Успешно подписался на " + ticker); //TODO: убрать когда будет проверено
                        Quik.Events.OnQuote += quoteDoDelegate;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

        }
    }
}
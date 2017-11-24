using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp;
using QuikSharp.DataStructures;
using System.Windows;
using QuikSharp.DataStructures.Transaction;

namespace MarketServerTest
{
    static class QuikConnector
    {
        private static Quik Quik;
        public static bool isConnected { get; private set; }
        private static  List<Order> list = new List<Order>();
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

        public static List<Order> GetOrders()
        {
            return Quik.Orders.GetOrders().Result;
        }


        public static void SendBid(string ticker, decimal price, int qty, Operation operationType, bool marketPrice)
        {
            try
            {
                Tool tool = createTool(ticker);
                if (marketPrice == true)
                {
                    price = Math.Round(tool.LastPrice + tool.Step * 5, tool.PriceAccuracy);
                }
                long transactionID = NewOrder(Quik, tool, operationType, price, qty);
            }
            catch { }
        }
        static Tool createTool(string ticker)
        {
            string classCode = "";
            try
            {
                classCode = Quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB", ticker).Result;
            }
            catch
            {
                Console.WriteLine("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно");
            }
            return new Tool(Quik, ticker, classCode);

        }
        static long NewOrder(Quik _quik, Tool _tool, Operation operation, decimal price, int qty)
        {
            long res = 0;
            Order newOrder = new Order();
            newOrder.ClassCode = _tool.ClassCode;
            newOrder.SecCode = _tool.SecurityCode;
            newOrder.Operation = operation;
            newOrder.Price = price;
            newOrder.Quantity = qty;
            newOrder.Account = _tool.AccountID;
            try
            {
                res = _quik.Orders.CreateOrder(newOrder).Result;
                if (res<0) Console.WriteLine("Всё очень плохо...");
            }
            catch
            {
                Console.WriteLine("Неудачная попытка отправки заявки");
            }
            return res;
        }

        public static void SubscribeToOrderBook(string ticker, QuoteHandler quoteDoDelegate) { 
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
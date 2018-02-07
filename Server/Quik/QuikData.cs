using System;
using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;

namespace Server.Quik
{
    static class QuikData
    {
        public static void SubscribeToOrderBook(string ticker, QuoteHandler quoteDoDelegate)
        {
            var tool = CreateTool(ticker);
            if (!string.IsNullOrEmpty(tool.Name))
            {
                QuikConnector.Quik.OrderBook.Subscribe(tool.ClassCode, tool.SecurityCode).Wait();
                bool isSubscribedToolOrderBook = QuikConnector.Quik.OrderBook.IsSubscribed(tool.ClassCode, tool.SecurityCode).Result;
                if (isSubscribedToolOrderBook)
                {
                    Console.WriteLine("Успешно подписался на " + ticker); //TODO: убрать когда будет проверено
                    QuikConnector.Quik.Events.OnQuote += quoteDoDelegate;
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

        public static void UnsubscribeFromOrderBook(string ticker)
        {
            var tool = CreateTool(ticker);
            if (!string.IsNullOrEmpty(tool.Name))
            {
                QuikConnector.Quik.OrderBook.Unsubscribe(tool.ClassCode, tool.SecurityCode).Wait();
            }
            else
            {
                throw new Exception();
            }
        }

        public static Tool CreateTool(string ticker, string classCode)
        {
            return new Tool(QuikConnector.Quik, ticker, classCode);
        }

        public static Tool CreateTool(string ticker)
        {
            string classCode = "";
            try
            {
                classCode = GetSecurityClass(ticker);
            }
            catch
            {
                Console.WriteLine("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно");
            }
            return new Tool(QuikConnector.Quik, ticker, classCode);
        }

        public static string GetClasses()
        {
            string classesList = "";

            for (int i = 0; i < QuikConnector.Quik.Class.GetClassesList().Result.Length; i++)
            {
                if (i != QuikConnector.Quik.Class.GetClassesList().Result.Length - 1)
                    classesList += QuikConnector.Quik.Class.GetClassesList().Result.GetValue(i) + ",";
                else classesList += QuikConnector.Quik.Class.GetClassesList().Result.GetValue(i);
            }
            return classesList;
        }

        public static string GetSecurityClass(string secCode)
        {
            string classesList = GetClasses();
            return QuikConnector.Quik.Class.GetSecurityClass(classesList, secCode).Result;
        }
        private static decimal BestPrice(string secCode)
        {
            Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            decimal bestPrice = Convert.ToDecimal(QuikConnector.Quik.Trading.GetParamEx(classCode, secCode, "BID").Result.ParamValue.Replace('.', separator));
            if (bestPrice.Equals(0))
                bestPrice = Convert.ToDecimal(QuikConnector.Quik.Trading.GetParamEx(classCode, secCode, "LAST").Result.ParamValue.Replace('.', separator));
            return bestPrice;
        }
        private static decimal BestOffer(string secCode)
        {
            Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            decimal bestOffer = Convert.ToDecimal(QuikConnector.Quik.Trading.GetParamEx(classCode, secCode, "OFFER").Result.ParamValue.Replace('.', separator));
            return bestOffer;

        }
        public static long NewOrder(Common.Models.ClientOrder order)
        {
            long res = 0;
            Tool tool = CreateTool(order.SecurityCode, order.ClassCode);
            if (order.MarketPrice)
            {
                switch (order.Operation)
                {
                    case QuikSharp.DataStructures.Operation.Buy:
                        var bestPrice = BestPrice(order.SecurityCode);//
                        order.Price = Math.Round(bestPrice + (decimal)0.001 * bestPrice, tool.PriceAccuracy); //если 0.001 окажется маловато для моментальной сделки, можно изменить                                                                                  
                        break;
                    case QuikSharp.DataStructures.Operation.Sell:
                        var bestOffer = BestOffer(order.SecurityCode); //Должно сработать
                        order.Price = Math.Round(bestOffer - (decimal)0.001 * bestOffer, tool.PriceAccuracy);
                        break;
                }
            }
            Order newOrder = new Order
            {
                ClassCode = tool.ClassCode,
                SecCode = tool.SecurityCode,
                Operation = order.Operation,
                Price = order.Price,
                Quantity = order.Quantity,
                Account = tool.AccountID
            };

            try
            {
                res = QuikConnector.Quik.Orders.CreateOrder(newOrder).Result;
            }
            catch
            {
                Console.WriteLine("Неудачная попытка отправки заявки");
            }
            return res;
        }
        public static long NewStopOrder(Common.Models.ClientStopOrder order)
        {
            long res = 0;
            Tool tool = CreateTool(order.SecurityCode);
            StopOrder stopOrder = new StopOrder
            {
                ClassCode = tool.ClassCode,
                SecCode = tool.SecurityCode,
                Operation = order.Operation,
                Price = order.Price2,
                ConditionPrice = order.Price,
                Quantity = order.Quantity,
                Account = tool.AccountID,
                StopOrderType = StopOrderType.StopLimit
            };
            try
            {
                res = QuikConnector.Quik.StopOrders.CreateStopOrder(stopOrder).Result;
            }
            catch
            {
                Console.WriteLine("Неудачная попытка отправки Стоп-заявки");
            }
            return res;
        }

        public static void SubscribeToOrdersRefresh(OrderHandler onRefresh)
        {
            QuikConnector.Quik.Events.OnOrder += onRefresh;
        }
        public static void SubscribeToTradesRefresh(TradeHandler tradesRefresh)
        {
            QuikConnector.Quik.Events.OnTrade += tradesRefresh;
        }
    }
}

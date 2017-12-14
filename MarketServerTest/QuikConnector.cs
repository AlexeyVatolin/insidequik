using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static List<Order> list = new List<Order>();
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

        public static List<DepoLimitEx> GetDepoLimits()
        {
            return Quik.Trading.GetDepoLimits().Result;
        }
        //коды валют
        enum currCode { SUR, EUR, USD }
        public static List<MoneyLimitEx> GetMoneyLimit()
        {
            List<MoneyLimitEx> list = new List<MoneyLimitEx>();
            string client = Quik.Class.GetClientCode().Result;
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    list.Add(Quik.Trading.GetMoneyEx("MC0058900000", client, "EQTV", ((currCode)j).ToString(), i).Result);
                }

            }
            return list;

        }
        public static string[] GetClasses()
        {
            return Quik.Class.GetClassesList().Result;
        }

        public static string GetSecurityClass(string secCode)
        {
            return Quik.Class.GetSecurityClass("SPBFUT,EQBR,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB,CROSSRATE,SPBOPT,CETS", secCode).Result;//сделать получение классов с сервера!
        }
        public static SecurityInfo GetSecurityInfo(string secCode)
        {
            string classCode = GetSecurityClass(secCode);
            return Quik.Class.GetSecurityInfo(classCode, secCode).Result;
        }
        public static decimal GetCrossRate(string secCode)
        {
            Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            return Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "CROSSRATE").Result.ParamValue.Replace('.', separator));
        }
        public static decimal LastPrice(string secCode)
        {
            Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            return Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "LAST").Result.ParamValue.Replace('.', separator));
        }
        /// <summary>
        /// Функция предназначена для получения ликв. цены (лучшая цена спроса)
        /// </summary>
        /// <returns></returns>
        public static decimal BestPrice(string secCode)
        {
            Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            decimal bestPrice = Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "BID").Result.ParamValue.Replace('.', separator));
            if (bestPrice.Equals(0))
                bestPrice = Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "LAST").Result.ParamValue.Replace('.', separator));
            return bestPrice;
        }
        public static FuturesClientHolding GetFuturesClientHolding(string firmid, string accid, string seccode)
        {
            return Quik.Trading.GetFuturesHolding(firmid, accid, seccode, 1).Result;
        }

        public static int GetStopOrdersQty(string secCode)
        {
            int count = 0;
            foreach (var item in GetStopOrders())
            {

                if (item.SecCode == secCode && item.State == State.Active)
                    count++;

            }
            return count;
        }

        public static List<Order> GetOrders()
        {
            return Quik.Orders.GetOrders().Result;
        }
        public static List<Trade> GetTrades()
        {
            return Quik.Trading.GetTrades().Result;
        }

        public static void CancelOrder(Order order)
        {
            Quik.Orders.KillOrder(order);
        }
        public static void CancelStopOrder(StopOrder stopOrder)
        {
            Quik.StopOrders.KillStopOrder(stopOrder);
        }
        public static List<StopOrder> GetStopOrders()
        {
            return Quik.StopOrders.GetStopOrders().Result;
        }
        public static void SendBid(string ticker, decimal price, int qty, Operation operationType, bool marketPrice)
        {
            try
            {
                Tool tool = CreateTool(ticker);
                if (marketPrice)
                {
                    price = Math.Round(tool.LastPrice + tool.Step * 5, tool.PriceAccuracy);
                }
                long transactionID = NewOrder(Quik, tool, operationType, price, qty);
            }
            catch { }
        }
        public static void SubscribeToDepoLimit(QuoteHandler depoLimitRefresh)
        {
            Quik.Events.OnQuote += depoLimitRefresh;
        }
        public static void SubscribeToOrdersRefresh(OrderHandler ordersRefresh)
        {
            Quik.Events.OnOrder += ordersRefresh;
        }
        public static void SubscribeToTradesRefresh(TradeHandler tradesRefresh)
        {
            Quik.Events.OnTrade += tradesRefresh;
        }
        public static void SubscribeToStopOrdersRefresh(StopOrderFunctions.StopOrderHandler stopOrdersRefresh)
        {
            Quik.StopOrders.NewStopOrder += stopOrdersRefresh;
        }
        static Tool CreateTool(string ticker)
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
                if (res < 0) Console.WriteLine("Всё очень плохо...");
            }
            catch
            {
                Console.WriteLine("Неудачная попытка отправки заявки");
            }
            return res;
        }

        public static void SubscribeToOrderBook(string ticker, QuoteHandler quoteDoDelegate)
        {
            var tool = CreateTool(ticker);
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

        public static void UnsubsckibeFromOrderBook(string ticker)
        {
            var tool = CreateTool(ticker);
            if (!string.IsNullOrEmpty(tool.Name))
            {
                Quik.OrderBook.Unsubscribe(tool.ClassCode, tool.SecurityCode).Wait();
            }
            else
            {
                throw new Exception();
            }
        }

        public static async Task<ObservableCollection<ClassesAndSecuritiesNode>> GetCurrentClassesAndSecuritites()
        {
            string[] classes = await Quik.Class.GetClassesList();
            var classesAndSecuritiesList = new ObservableCollection<ClassesAndSecuritiesNode>();

            foreach (var @class in classes)
            {
                ClassInfo classInfo = await Quik.Class.GetClassInfo(@class);
                var currentItem = new ClassesAndSecuritiesNode
                {
                    ClassInfo = classInfo,
                    SecurityInfos = new ObservableCollection<SecurityInfoRow>()
                };
                classesAndSecuritiesList.Add(currentItem);
                string[] classSecurities = await Quik.Class.GetClassSecurities(@class);
                foreach (var classSecurity in classSecurities)
                {
                    SecurityInfo securityInfo = await Quik.Class.GetSecurityInfo(@class, classSecurity);
                    currentItem.SecurityInfos.Add(new SecurityInfoRow { Parent = currentItem, SecurityInfo = securityInfo });
                }

            }
            return classesAndSecuritiesList;
        }

        public static void SendStopOrderBid(string ticker, decimal price, decimal price2, int qty, Operation operation)
        {
            try
            {
                Tool tool = CreateTool(ticker);
                long tranctionID = NewStopOrder(Quik, tool, operation, price, price2, qty);
            }
            catch { }
        }

        static long NewStopOrder(Quik quik, Tool tool, Operation operation, decimal price, decimal price2, int qty)
        {
            long res = 0;
            StopOrder stopOrder = new StopOrder();
            stopOrder.ClassCode = tool.ClassCode;
            stopOrder.SecCode = tool.SecurityCode;
            stopOrder.Operation = operation;
            stopOrder.Price = price2;
            stopOrder.ConditionPrice = price;
            stopOrder.Quantity = qty;
            stopOrder.Account = tool.AccountID;
            stopOrder.StopOrderType = StopOrderType.StopLimit;
            try
            {
                res = quik.StopOrders.CreateStopOrder(stopOrder).Result;
                if (res < 0) Console.WriteLine("Всё очень плохо...");
            }
            catch
            {
                Console.WriteLine("Неудачная попытка отправки Стоп-заявки");
            }
            return res;
        }

    }
}
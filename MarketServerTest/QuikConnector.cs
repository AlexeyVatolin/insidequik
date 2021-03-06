﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MarketServerTest.DataRows;
using MarketServerTest.Models;
using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;

namespace MarketServerTest
{
    static class QuikConnector
    {
        private static Quik Quik;
        public static UserAccount userAccount;
        public static bool isConnected { get; private set; }
        public static bool isStopOrderCanceled;


        public delegate void OnQuoteDoDelegate(OrderBook quote);

        public static async Task<bool> Connect()
        {

            Quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            var connectionCheckTask = Quik.Service.IsConnected();
            int timeout = 2000; //2 раз проверяем подключение и ждем ответа 2 с. Если не подключилось, от возвращаем false

            Thread.Sleep(100); //Ожидаем полной инициализации Quik.
            for (int i = 0; i < 2; i++)
            {
                if (await Task.WhenAny(connectionCheckTask, Task.Delay(timeout)) == connectionCheckTask)
                {
                    isConnected = true;
                    userAccount = new UserAccount();
                    userAccount.currentBalance = BalanceWorker.GetCurrentBalance();
                    return true;
                }
                try
                {
                    Quik.StopService();
                }
                catch (Exception)
                {
                    // ignored
                }
                Quik = null;
                Quik = new Quik(Quik.DefaultPort, new InMemoryStorage());
            }

            return false;
        }

        public static void Disconnect()
        {
            isConnected = false;
            Quik.StopService();
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

        public static int GetLots(string securityCode, string classCode)
        {
            Tool tool = new Tool(Quik, securityCode, classCode);
            return tool.Lot;
        }
        public static string GetClasses()
        {
            string classesList = "";

            for (int i = 0; i < Quik.Class.GetClassesList().Result.Length; i++)
            {
                if (i != Quik.Class.GetClassesList().Result.Length - 1)
                    classesList += Quik.Class.GetClassesList().Result.GetValue(i) + ",";
                else classesList += Quik.Class.GetClassesList().Result.GetValue(i);
            }
            return classesList;
        }

        public static string GetSecurityClass(string secCode)
        {
            string classesList = GetClasses();
            return Quik.Class.GetSecurityClass(classesList, secCode).Result;
        }
        public static SecurityInfo GetSecurityInfo(string secCode)
        {
            string classCode = GetSecurityClass(secCode);
            return Quik.Class.GetSecurityInfo(classCode, secCode).Result;
        }
        public static decimal GetCrossRate(string secCode)
        {
            Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            return Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "CROSSRATE").Result.ParamValue.Replace('.', separator));
        }
        public static decimal LastPrice(string secCode)
        {
            Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            return Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "LAST").Result.ParamValue.Replace('.', separator));
        }
        /// <summary>
        /// Функция предназначена для получения ликв. цены (лучшая цена спроса)
        /// </summary>
        /// <returns></returns>
        public static decimal BestPrice(string secCode)
        {
            Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            decimal bestPrice = Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "BID").Result.ParamValue.Replace('.', separator));
            if (bestPrice.Equals(0))
                bestPrice = Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "LAST").Result.ParamValue.Replace('.', separator));
            return bestPrice;
        }
        /// <summary>
        /// Функция предназначена для получения лучшей цены предложения 
        /// </summary>
        /// <param name="secCode"></param>
        /// <returns></returns>
        public static decimal BestOffer(string secCode)
        {
            Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            string classCode = GetSecurityClass(secCode);
            decimal bestOffer = Convert.ToDecimal(Quik.Trading.GetParamEx(classCode, secCode, "OFFER").Result.ParamValue.Replace('.', separator));
            return bestOffer;

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
            isStopOrderCanceled = true;
        }
        public static List<StopOrder> GetStopOrders()
        {
            return Quik.StopOrders.GetStopOrders().Result;
        }
        /// <summary>
        /// Отправление заявки
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="price"></param>
        /// <param name="qty"></param>
        /// <param name="operationType"></param>
        /// <param name="marketPrice"></param>
        public static void SendBid(string ticker, decimal price, int qty, Operation operationType, bool marketPrice)
        {
            try
            {
                Tool tool = CreateTool(ticker);

                if (marketPrice)
                {
                    switch (operationType)
                    {
                        case Operation.Buy:
                            var bestPrice = BestPrice(ticker);//
                            price = Math.Round(bestPrice + (decimal)0.001 * bestPrice, tool.PriceAccuracy); //если 0.001 окажется маловато для моментальной сделки, можно изменить
                            MessageBox.Show(price.ToString());
                            break;
                        case Operation.Sell:
                            var bestOffer = BestOffer(ticker); //Должно сработать
                            price = Math.Round(bestOffer - (decimal)0.001 * bestOffer, tool.PriceAccuracy);
                            break;
                    }

                }

                if (operationType == Operation.Buy)
                {
                    if (price * qty < userAccount.currentBalance)
                    {
                        long transactionID = NewOrder(Quik, tool, operationType, price, qty);
                    }
                    else MessageBox.Show("Недостаточно средств для соверешения сделки: " + (price * qty - userAccount.currentBalance).ToString(), "Недостаточно средств!");
                }
                else if (operationType == Operation.Sell)
                {
                    long transactionID = NewOrder(Quik, tool, operationType, price, qty);
                }
            }
            catch { }
        }
        public static void SendBid(string classCode, string ticker, decimal price, int qty, Operation operationType, bool marketPrice)
        {
            try
            {
                Tool tool = CreateTool(ticker, classCode);

                if (marketPrice)
                {
                    switch (operationType)
                    {
                        case Operation.Buy:
                            var bestPrice = BestPrice(ticker);//
                            price = Math.Round(bestPrice + (decimal)0.001 * bestPrice, tool.PriceAccuracy); //если 0.001 окажется маловато для моментальной сделки, можно изменить
                            //MessageBox.Show(price.ToString());
                            break;
                        case Operation.Sell:
                            var bestOffer = BestOffer(ticker); //Должно сработать
                            price = Math.Round(bestOffer - (decimal)0.001 * bestOffer, tool.PriceAccuracy);
                            break;
                    }

                }

                if (operationType == Operation.Buy)
                {
                    if (price * qty < userAccount.currentBalance)
                    {
                        long transactionID = NewOrder(Quik, tool, operationType, price, qty);
                        //MessageBox.Show(transactionID.ToString());
                    }
                    else MessageBox.Show("Недостаточно средств для соверешения сделки: " + (price * qty - userAccount.currentBalance).ToString(), "Недостаточно средств!");
                }
                else if (operationType == Operation.Sell)
                {
                    long transactionID = NewOrder(Quik, tool, operationType, price, qty);
                }
            }
            catch { }
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
                classCode = GetSecurityClass(ticker);
            }
            catch
            {
                Console.WriteLine("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно");
            }
            return new Tool(Quik, ticker, classCode);

        }
        static Tool CreateTool(string ticker, string classCode)
        {
            return new Tool(Quik, ticker, classCode);
        }
        static long NewOrder(Quik _quik, Tool _tool, Operation operation, decimal price, int qty)
        {
            long res = 0;
            
            Order newOrder = new Order
            {
                ClassCode = _tool.ClassCode,
                SecCode = _tool.SecurityCode,
                Operation = operation,
                Price = price,
                Quantity = qty,
                Account = _tool.AccountID,
                TransID = 777               
                //ClientCode=_tool.ClientCode+"//"+"info"
            };


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
                currentItem.SecurityInfos =
                    new ObservableCollection<SecurityInfoRow>(
                        currentItem.SecurityInfos.OrderBy(item => item.SecurityInfo.Name));

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
            stopOrder.TransId = 777;
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

        public static async void UpdateSecurityInfo(SecuritiesModel security)
        {
            var lastTask = Quik.Trading.GetParamEx(security.ClassCode, security.SecCode, "LAST");
            var changePercentTask = Quik.Trading.GetParamEx(security.ClassCode, security.SecCode, "LASTCHANGE");
            var flowTask = Quik.Trading.GetParamEx(security.ClassCode, security.SecCode, "VALTODAY");
            var results = await Task.WhenAll(lastTask, changePercentTask, flowTask);
            security.LastPrice = double.Parse(results[0].ParamValue, CultureInfo.InvariantCulture);
            security.ChangePercent = double.Parse(results[1].ParamValue, CultureInfo.InvariantCulture);
            //ToString("N") приводит число к виду 123 456 789
            security.Flow = double.Parse(results[2].ParamValue, CultureInfo.InvariantCulture).ToString("N0");
        }

    }
}
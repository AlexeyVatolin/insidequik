using System;
using QuikSharp;

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

        public static void UnsubsckibeFromOrderBook(string ticker)
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
    }
}

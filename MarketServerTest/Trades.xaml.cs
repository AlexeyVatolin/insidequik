using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using QuikSharp.DataStructures.Transaction;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Deals.xaml
    /// </summary>
    public partial class Trades : Window
    {
        List<Trade> listTrades = new List<Trade>();
        public Trades()
        {
            InitializeComponent();
            InitializeTable();
            QuikConnector.SubscribeToTradesRefresh(TradesRefresh);
        }
        
        public void InitializeTable()
        {
            listTrades = QuikConnector.GetTrades();
            List<Order> listOrders = QuikConnector.GetOrders();
            foreach (var item in listTrades)
            {
                var listOrderItem = listOrders.Find(i => i.OrderNum == item.OrderNum);
                var operation = listOrderItem.Operation.ToString();

                var date = listOrderItem.Datetime.hour.ToString("00") + ":" + listOrderItem.Datetime.min.ToString("00") 
                    + ":" + listOrderItem.Datetime.sec.ToString("00") + "." + listOrderItem.Datetime.ms;

                TradesTable.Items.Add(new ColumnsForTrades
                {
                    Company = item.SecCode, //инструмент
                    Value = item.Value.ToString(CultureInfo.InvariantCulture), //объем в денежных единицах
                    Quantity = item.Quantity.ToString(), // предположительно объем бумаг
                    Date = date, // дата сделки
                    Time = item.Period.ToString(), // время в милисекундах
                    Opeartion = operation, // покупка/продажа
                });
            }
        }

        public void TradesRefresh(Trade trade)
        {
            List<Order> listOrders = QuikConnector.GetOrders();
            var listOrderItem = listOrders.Find(i => i.OrderNum == trade.OrderNum);
            var operation = listOrderItem.Operation.ToString();
            var date = listOrderItem.Datetime.hour.ToString("00") + ":" + listOrderItem.Datetime.min.ToString("00")
                + ":" + listOrderItem.Datetime.sec.ToString("00") + "." + listOrderItem.Datetime.ms;
            listTrades.Add(trade);
            TradesTable.Dispatcher.Invoke(() =>
            {
                TradesTable.Items.Add(new ColumnsForTrades
                {
                    Company = trade.SecCode, //инструмент
                    Value = trade.Value.ToString(CultureInfo.InvariantCulture), //объем в денежных единицах
                    Quantity = trade.Quantity.ToString(), // предположительно объем бумаг
                    Date = date, // дата сделки
                    Time = trade.Period.ToString(), // время в милисекундах
                    Opeartion = operation, // покупка/продажа
                });
            });     
        }
    }
}

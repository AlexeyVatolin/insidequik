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
        public Trades()
        {
            InitializeComponent();
            InitializeTable();
        }
        
        public void InitializeTable()
        {
            List<Trade> listTrades = QuikConnector.GetTrades();
            List<Order> listOrders = QuikConnector.GetOrders();
            foreach (var item in listTrades)
            {
                var listOrderItem = listOrders.Find(i => i.OrderNum == item.OrderNum);
                var direction = listOrderItem.Operation.ToString();

                var data = listOrderItem.Datetime.hour.ToString("00") + ":" + listOrderItem.Datetime.min.ToString("00") 
                    + ":" + listOrderItem.Datetime.sec.ToString("00") + "." + listOrderItem.Datetime.ms;

                TradesTable.Items.Add(new ColumnsForTrades
                {
                    Company = item.SecCode, //инструмент
                    Value = item.Value.ToString(CultureInfo.InvariantCulture), //объем в денежных единицах
                    Quantity = item.Quantity.ToString(), // предположительно объем бумаг
                    Date = data, // дата сделки
                    Time = item.Period.ToString(), // время в милисекундах
                    Direction = direction, // покупка/продажа
                });
            }
        }
    }
}

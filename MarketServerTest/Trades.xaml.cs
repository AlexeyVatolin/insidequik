using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            List<Trade> ListTrade = QuikConnector.GetTrades();
            List<Order> ListOrder = QuikConnector.GetOrders();
            string direction = null;
            string data = null;
            foreach (var item in ListTrade)
            {
                foreach (var items in ListOrder)
                {
                    if (items.OrderNum == item.OrderNum)
                    {
                        direction = items.Operation.ToString();
                        data = items.Datetime.hour.ToString() + ":" + items.Datetime.min.ToString() + ":" + items.Datetime.sec.ToString() + "." + items.Datetime.ms.ToString();
                    }
                }
                TradesTable.Items.Add(new ColumnsForTrades
                {
                    Company = item.SecCode.ToString(), //инструмент
                    Value = item.Value.ToString(), //объем в денежных единицах
                    Quantity = item.Quantity.ToString(), // предположительно объем бумаг
                    Date = data, // дата сделки
                    Time = item.Period.ToString(), // время в милисекундах
                    Direction = direction, // покупка/продажа
                });
            }
        }
}
}

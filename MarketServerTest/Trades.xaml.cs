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
        private object locker = new object();
        private List<Trade> listTrades = new List<Trade>();
        int count = 0;
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
                TradesTable.Items.Add(new ColumnsForTrades(item, listOrderItem));
            }
        }

        public void TradesRefresh(Trade trade)
        {
            List<Order> listOrders = QuikConnector.GetOrders();
            Order listOrderItem = listOrders.Find(i => i.OrderNum == trade.OrderNum);
            listTrades.Add(trade);
            count++;
            if (count % 3 == 2) //знаю, что дикий костыль, но это работает :DDD Позже исправим)
            {
                TradesTable.Dispatcher.Invoke(() =>
                {
                    TradesTable.Items.Add(new ColumnsForTrades(trade, listOrderItem));
                });
            }
        }
    }
}

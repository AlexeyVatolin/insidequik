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
            lock (locker)
            {
                List<Order> listOrders = QuikConnector.GetOrders();
                int index = listTrades.FindIndex(i => i.OrderNum == trade.OrderNum);
                if (index > 0)
                {
                    if (listTrades[index] != trade)
                    {
                        Order listOrderItem = listOrders.Find(i => i.OrderNum == trade.OrderNum);
                        listTrades[index] = trade;
                        TradesTable.Dispatcher.Invoke(() =>
                            TradesTable.Items[index] = new ColumnsForTrades(trade, listOrderItem));
                    }
                }
                else
                {
                    Order listOrderItem = listOrders.Find(i => i.OrderNum == trade.OrderNum);
                    listTrades.Add(trade);
                    TradesTable.Dispatcher.Invoke(() =>
                    {
                        TradesTable.Items.Add(new ColumnsForTrades(trade, listOrderItem));
                    });
                }

            }
        }
    }
}

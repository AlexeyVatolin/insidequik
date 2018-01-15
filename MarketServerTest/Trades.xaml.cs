using System.Collections.Generic;
using System.Windows;
using QuikSharp.DataStructures.Transaction;
using System.ComponentModel;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Trades.xaml
    /// </summary>
    public partial class Trades : MetroWindow
    {
        ListSortDirection direction;
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
            direction = ListSortDirection.Descending;
            TradesTable.Items.SortDescriptions.Add(new SortDescription("Date", direction));
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
        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection newDir = ListSortDirection.Ascending;
            TradesTable.Items.SortDescriptions.Clear();
            if (direction == ListSortDirection.Ascending)
            {
                newDir = ListSortDirection.Descending;
            }
            else if (direction == ListSortDirection.Descending)
            {
                newDir = ListSortDirection.Ascending;
            }
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            TradesTable.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            direction = newDir;
        }
    }
}
using System.Collections.Generic;
using System.Windows;
using QuikSharp.DataStructures.Transaction;
using System.ComponentModel;
using System.Windows.Controls;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для BidsAndDeals.xaml
    /// </summary>
    public partial class Orders : Window
    {
        ListSortDirection direction;
        private List<Order> list = new List<Order>();
        public Orders()
        {
            InitializeComponent();
            InitializeOrdersTable();
            QuikConnector.SubscribeToOrdersRefresh(OrdersRefresh);
        }

        public void OrdersRefresh(Order order)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].OrderNum == order.OrderNum)
                {
                    list[i] = order;
                    OrdersTable.Dispatcher.Invoke(() =>
                    {
                        OrdersTable.Items[i] = new ColumnsForOrders(order);
                    });
                    break;
                }
                else if (i == list.Count - 1)
                {
                    list.Add(order);
                    OrdersTable.Dispatcher.Invoke(() =>
                    {
                        OrdersTable.Items.Add(new ColumnsForOrders(order));
                    });
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.CancelOrder(list[OrdersTable.SelectedIndex]);
        }
        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection newDir = ListSortDirection.Ascending;
            OrdersTable.Items.SortDescriptions.Clear();
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
            OrdersTable.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            direction = newDir;
        }

        public void InitializeOrdersTable()
        {
            list = QuikConnector.GetOrders();
            foreach (var item in list)
            {
                OrdersTable.Items.Add(new ColumnsForOrders(item));
            }
            direction = ListSortDirection.Descending;
            OrdersTable.Items.SortDescriptions.Add(new SortDescription("Time", direction));
        }
    }
}

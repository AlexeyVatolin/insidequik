using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using QuikSharp.DataStructures.Transaction;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для BidsAndDeals.xaml
    /// </summary>
    public partial class Orders : Window
    {
        private object locker = new object();
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

        public void InitializeOrdersTable()
        {
            list = QuikConnector.GetOrders();
            foreach (var item in list)
            {
                OrdersTable.Items.Add(new ColumnsForOrders(item));
            }
        }
    }
}

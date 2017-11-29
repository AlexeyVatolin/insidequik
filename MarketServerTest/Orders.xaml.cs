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
using QuikSharp.DataStructures.Transaction;
namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для BidsAndDeals.xaml
    /// </summary>
    public partial class Orders : Window
    {
        List<Order> list = new List<Order>();
        public Orders()
        {
            InitializeComponent();
            InitializeOrdersTable();
            QuikConnector.SubscribeToOrders(OrdersRefresh);
        }

        public void OrdersRefresh(Order order)
        {
            list.Add(order);
            OrdersTable.Items.Add(new ColumnsForOrders(order));
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

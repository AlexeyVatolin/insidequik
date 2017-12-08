using QuikSharp.DataStructures;
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

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для StopOrders.xaml
    /// </summary>
    public partial class StopOrders : Window
    {
        private List<StopOrder> stopOrdersList = new List<StopOrder>();
        public StopOrders()
        {
            InitializeComponent();
            InitializeStopOrdersTable();
            QuikConnector.SubscribeToStopOrdersRefresh(StopOrdersRefresh);
        }
        public void InitializeStopOrdersTable()
        {
            stopOrdersList = QuikConnector.GetStopOrders();
            foreach(var item in stopOrdersList)
            {
                StopOrdersTable.Items.Add(new ColumnsForStopOrders(item));
            }
        }
        public void StopOrdersRefresh(StopOrder stopOrder)
        {
            for (int i = 0; i < stopOrdersList.Count; i++)
            {
                if (stopOrdersList[i].OrderNum == stopOrder.OrderNum)
                {
                    stopOrdersList[i] = stopOrder;
                    StopOrdersTable.Dispatcher.Invoke(() =>
                    {
                        StopOrdersTable.Items[i] = new ColumnsForStopOrders(stopOrder);
                    });
                    break;
                }
                else if (i == stopOrdersList.Count - 1)
                {
                    stopOrdersList.Add(stopOrder);
                    StopOrdersTable.Dispatcher.Invoke(() =>
                    {
                        StopOrdersTable.Items.Add(new ColumnsForStopOrders(stopOrder));
                    });
                }
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.CancelStopOrder(stopOrdersList[StopOrdersTable.SelectedIndex]);
        }
    }
}

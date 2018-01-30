using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для StopOrders.xaml
    /// </summary>
    public partial class StopOrders : MetroWindow
    {
        private List<StopOrder> stopOrdersList = new List<StopOrder>();
        ListSortDirection direction;
        int index;
        object locker = new object();
        public StopOrders()
        {
            InitializeComponent();
            InitializeStopOrdersTable();
            QuikConnector.SubscribeToStopOrdersRefresh(StopOrdersRefresh);
        }
        public void InitializeStopOrdersTable()
        {
            stopOrdersList = QuikConnector.GetStopOrders();
            foreach (var item in stopOrdersList)
            {
                StopOrdersTable.Items.Add(new ColumnsForStopOrders(item));
            }
        }
        public void StopOrdersRefresh(StopOrder stopOrder)
        {
            lock (locker)
            {
                if (QuikConnector.isStopOrderCanceled)
                {
                    for (int i = 0; i < stopOrdersList.Count; i++)
                    {
                        if (stopOrdersList[i].OrderNum == stopOrder.OrderNum)
                        {
                            stopOrdersList[i] = stopOrder;
                            StopOrdersTable.Dispatcher.Invoke(() =>
                            {
                                StopOrdersTable.Items[index] = new ColumnsForStopOrders(stopOrder);
                            });
                            QuikConnector.isStopOrderCanceled = false;
                            break;
                        }
                    }
                }
                else
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
            var selectedItem = StopOrdersTable.SelectedItem as ColumnsForStopOrders;
            foreach (var item in stopOrdersList)
            {
                if (selectedItem.OrderNum == item.OrderNum)
                {
                    index = StopOrdersTable.SelectedIndex;
                    QuikConnector.CancelStopOrder(item);
                    break;
                }
            }
        }
        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection newDir = ListSortDirection.Ascending;
            StopOrdersTable.Items.SortDescriptions.Clear();
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
            StopOrdersTable.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            direction = newDir;
        }
    }
}
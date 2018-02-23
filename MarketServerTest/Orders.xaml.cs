using System.Collections.Generic;
using System.Windows;
using QuikSharp.DataStructures.Transaction;
using System.ComponentModel;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System;
using System.Linq;
using Microsoft.AspNet.SignalR.Client;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для BidsAndDeals.xaml
    /// </summary>
    public partial class Orders : MetroWindow
    {
        ListSortDirection direction;
        private List<Order> list = new List<Order>();
        int index;
        int count = 0;
        object locker = new object();
        public Orders()
        {
            InitializeComponent();
            //на будущее для отладки
            /*OrdersTable.Items.Add(new ColumnsForOrders() { Operation = "Buy", Balance ="fjksbfshbd", State = "Canceled", ClassCode ="fdjsnfkjsnfjk", Company="fdjnsjkf", Price="fdsfjkndsjk", Quantity="fsfdsfs", Time="fbdshfbsd", Value="gnfkgjfdg" });
            OrdersTable.Items.Add(new ColumnsForOrders() { Operation = "Buy", Balance ="fjksbfshbd", State = "Completed", ClassCode ="fdjsnfkjsnfjk", Company="fdjnsjkf", Price="fdsfjkndsjk", Quantity="fsfdsfs", Time="fbdshfbsd", Value="gnfkgjfdg" });
            OrdersTable.Items.Add(new ColumnsForOrders() { Operation = "Buy", Balance = "fjksbfshbd", State = "Active", ClassCode = "fdjsnfkjsnfjk", Company = "fdjnsjkf", Price = "fdsfjkndsjk", Quantity = "fsfdsfs", Time = "fbdshfbsd", Value = "gnfkgjfdg" });
            OrdersTable.Items.Add(new ColumnsForOrders() { Operation = "Sell", Balance = "fjksbfshbd", State = "Canceled", ClassCode = "fdjsnfkjsnfjk", Company = "fdjnsjkf", Price = "fdsfjkndsjk", Quantity = "fsfdsfs", Time = "fbdshfbsd", Value = "gnfkgjfdg" });
            OrdersTable.Items.Add(new ColumnsForOrders() { Operation = "Sell", Balance = "fjksbfshbd", State = "Completed", ClassCode = "fdjsnfkjsnfjk", Company = "fdjnsjkf", Price = "fdsfjkndsjk", Quantity = "fsfdsfs", Time = "fbdshfbsd", Value = "gnfkgjfdg" });
            OrdersTable.Items.Add(new ColumnsForOrders() { Operation = "Sell", Balance = "fjksbfshbd", State = "Active", ClassCode = "fdjsnfkjsnfjk", Company = "fdjnsjkf", Price = "fdsfjkndsjk", Quantity = "fsfdsfs", Time = "fbdshfbsd", Value = "gnfkgjfdg" });
            */
            //InitializeOrdersTable();
            //QuikConnector.SubscribeToOrdersRefresh(OrdersRefresh);

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
                        OrdersTable.Items[index] = new ColumnsForOrders(order);
                    });
                    break;
                }

            }
            if (count % 2 == 0)
            {
                list.Add(order);
                OrdersTable.Dispatcher.Invoke(() =>
                {
                    list.Add(order);
                    OrdersTable.Dispatcher.Invoke(() =>
                    {
                        OrdersTable.Items.Add(new ColumnsForOrders(order));
                    });
                });
            }
            else
            {
                list.Add(order);
                OrdersTable.Dispatcher.Invoke(() =>
                {
                    OrdersTable.Items.Add(new ColumnsForOrders(order));
                });
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
{
    var selectedItem = OrdersTable.SelectedItem as ColumnsForOrders;
    foreach (var item in list)
    {
        if (selectedItem.OrderNum == item.OrderNum)
        {
            index = OrdersTable.SelectedIndex;
            QuikConnector.CancelOrder(item);
            break;
        }
    }
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

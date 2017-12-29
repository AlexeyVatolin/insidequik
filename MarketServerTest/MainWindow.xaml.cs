using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetOrdersBook.IsEnabled = false;
            Ticker.IsEnabled = false;
            SetOrder.IsEnabled = false;
            GetTrades.IsEnabled = false;
            GetOrders.IsEnabled = false;
            GetStopOrders.IsEnabled = false;
            ShowCurrentTrades.IsEnabled = false;
            GetBalance.IsEnabled = false;
            SetStopOrder.IsEnabled = false;
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            Loading.IsOpen = true;
            var isConnected = await Task.Run(QuikConnector.Connect);
            Loading.IsOpen = false;
            Message.Text = isConnected ? "Connected to QUIK" : "Some error while connecting to QUIK";
            IsConnected.IsOpen = true;
            if (isConnected)
            {
                Connect.IsEnabled = false;
                GetOrdersBook.IsEnabled = true;
                Ticker.IsEnabled = true;
                SetOrder.IsEnabled = true;
                GetTrades.IsEnabled = true;
                GetOrders.IsEnabled = true;
                GetStopOrders.IsEnabled = true;
                ShowCurrentTrades.IsEnabled = true;
                GetBalance.IsEnabled = true;
                SetStopOrder.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new SendBid().Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Orders().Show();
        }

        private void ShowCurrentTrades_Click(object sender, RoutedEventArgs e)
        {
            var createNewSecurititesWindow = new CreateNewSecurititesWindow();
            createNewSecurititesWindow.Show();
            createNewSecurititesWindow.Initialize();
        }

        private void GetOrdersBook_OnClickr_Click(object sender, RoutedEventArgs e)
        {
            new OrdersBook(Ticker.Text).Show();
        }

        private void GetStopOrders_Click(object sender, RoutedEventArgs e)
        {
            new StopOrders().Show();
        }

        private void GetTrades_OnClick(object sender, RoutedEventArgs e)
        {
            new Trades().Show();
        }

        private void GetBalance_OnClick(object sender, RoutedEventArgs e)
        {
            new Balance().Show();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new StopOrderBid().Show();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (QuikConnector.isConnected)
            {
                QuikConnector.Disconnect();
            }
            Hide();
            //TODO: посмотреть возможности закрывать программу быстрее
            Environment.Exit(0);
        }
    }
}

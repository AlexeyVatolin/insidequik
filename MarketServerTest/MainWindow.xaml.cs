using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuikSharp.DataStructures;
using System.Threading;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;
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
        private void Callback(Object state)
        {
            // Long running operation
            if (BalanceWorker.GetDayProfit() <= QuikConnector.userAccount.userLimit)
            {
                QuikConnector.userAccount.limitLock = true;//блокировка пользователя
                MessageBox.Show("LIMIT LOCK! Превышен лимит суточных потерь. Аккаунт заблокирован");//переделать
                //Здесь должна быть логика, которая отменит все активные заявки, продаст активы и отключит возможность совершать операции
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }
            timer.Change(1000 * 3, Timeout.Infinite);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.Connect();
            Message.Text = QuikConnector.isConnected ? "Connected to QUIK" : "Some error while connecting to QUIK";
            IsConnected.IsOpen = true;
            if (QuikConnector.isConnected)
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

                timer = new Timer(Callback, null, 1000 * 3, Timeout.Infinite);
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
            QuikConnector.Disconnect();
            Environment.Exit(0);
        }
    }
}

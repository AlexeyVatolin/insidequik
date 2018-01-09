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
using System.Threading;
using Timer = System.Threading.Timer;
using QuikSharp.DataStructures.Transaction;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Trades trades;
        private Orders orders;
        private StopOrders stopOrders;
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
                List<Order> orders = QuikConnector.GetOrders();
                List<StopOrder> stopOrders = QuikConnector.GetStopOrders();
                List<DepoLimitEx> depoLimit = QuikConnector.GetDepoLimits();
                // List<
                foreach (var order in orders)
                {
                    if (order.State.ToString() == "Active")
                    {
                        QuikConnector.CancelOrder(order);
                    }
                }
                foreach (var stopOrder in stopOrders)
                {
                    if (stopOrder.State.ToString() == "Active")
                    {
                        QuikConnector.CancelStopOrder(stopOrder);
                    }
                }
                MessageBox.Show("Все заявки и стоп-заявки отменены");
                foreach (var lim in depoLimit)
                {
                    if (lim.LimitKindInt == 2)
                    {
                        int qty = QuikConnector.GetLots(lim.SecCode, QuikConnector.GetSecurityClass(lim.SecCode));
                        QuikConnector.SendBid(lim.SecCode, 0, (int)lim.CurrentBalance/qty, Operation.Sell, true);
                    }
                }
                MessageBox.Show("Все активы выставлены на продажу");
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }
            timer.Change(1000 * 3, Timeout.Infinite);
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

                timer = new Timer(Callback, null, 1000 * 3, Timeout.Infinite);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new SendBid().Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (orders == null || orders.IsLoaded == false)
            {
                orders = new Orders();
                orders.Show();
            }
            else
            {
                orders.Activate();
            }
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
            if (stopOrders == null || stopOrders.IsLoaded == false)
            {
                stopOrders = new StopOrders();
                stopOrders.Show();
            }
            else
            {
                stopOrders.Activate();
            }
        }

        private void GetTrades_OnClick(object sender, RoutedEventArgs e)
        {
            if (trades == null || trades.IsLoaded == false)
            {
                trades = new Trades();
                trades.Show();
            }
            else
            {
                trades.Activate();
            }
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using QuikSharp.DataStructures;
using System.Threading;
using Timer = System.Threading.Timer;
using QuikSharp.DataStructures.Transaction;
using MahApps.Metro.Controls;
using Microsoft.ApplicationInsights;
using finam.ru_economic_calendar;
using MarketServerTest.SecurityTables;
using MarketServerTest.ViewModels;
using Microsoft.AspNet.SignalR.Client;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly HubConnection connection;
        private Trades trades;
        private Orders orders;
        private StopOrders stopOrders;
        private Timer timer;

        private TelemetryClient telemetryClient = new TelemetryClient();
        private ObservableCollection<TradeTableViewModel> tradeTables;

        public MainWindow()
        {
            InitializeComponent();
            //Временно отключил таймер потому что теперь есть только подключение к серверу, но не к квику
            //timer = new Timer(Callback, null, 1000 * 3, Timeout.Infinite);
        }

        public MainWindow(HubConnection connection)
        {
            this.connection = connection;
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
                        QuikConnector.SendBid(lim.SecCode, 0, (int)lim.CurrentBalance / qty, Operation.Sell, true);
                    }
                }
                MessageBox.Show("Все активы выставлены на продажу");
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }
            timer.Change(1000 * 3, Timeout.Infinite);
        }

        private void SetNewOrder_OnClick(object sender, RoutedEventArgs e)
        {
            telemetryClient.TrackPageView("Новая заявка");
            new SendBid().Show();
        }

        private void Orders_OnClick(object sender, RoutedEventArgs e)
        {
            telemetryClient.TrackPageView("Таблица сделок");
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

        private void ShowCurrentTrades_OnClick(object sender, RoutedEventArgs e)
        {
            telemetryClient.TrackPageView("Текущие торги");
            var createNewSecurititesWindow = new CreateNewSecurititesWindow();
            createNewSecurititesWindow.Show();
            createNewSecurititesWindow.Initialize();
        }

        private void GetOrdersBook_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new Views.OrdersBook(Ticker.Text).Show();
                telemetryClient.TrackPageView("Стакан заявок");
            }
            catch (Exception) { }
        }

        private void GetStopOrders_OnClick(object sender, RoutedEventArgs e)
        {
            telemetryClient.TrackPageView("Таблица СТОП-заявок");
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
            telemetryClient.TrackPageView("Таблица сделок");
        }

        private void GetBalance_OnClick(object sender, RoutedEventArgs e)
        {
            new Balance().Show();
            telemetryClient.TrackPageView("Состояние счета");
        }
        private void SetNewStopOrder_OnClick(object sender, RoutedEventArgs e)
        {
            new SendStopOrder().Show();
            telemetryClient.TrackPageView("Новая стоп-заявка");
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

        private void Calendar_OnClick(object sender, RoutedEventArgs e)
        {
            new CalendarMainWindow().Show();
            telemetryClient.TrackPageView("Календарь");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (QuikConnector.isConnected)
            {
                QuikConnector.Disconnect();
            }
            Hide();
            Environment.Exit(0);
        }

        private void TradeTableClick(string id)
        {
            new Securities(SecurityTablesRepository.GetSecuritiesById(id)).Show();
        }
        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.Disconnect();
            Hide();
            new Login().ShowDialog();
            Close();
        }

        private void ConnectToServer_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OrderBook_OnClick(object sender, RoutedEventArgs e)
        {
            new Views.OrdersBook(Ticker.Text).Show();
        }
    }
}

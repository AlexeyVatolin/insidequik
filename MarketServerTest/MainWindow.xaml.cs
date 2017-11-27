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
        private string currentTicker;
        public MainWindow()
        {
            InitializeComponent();
            GetOrdersBook.IsEnabled = false;
            Ticker.IsEnabled = false;
            SetOrder.IsEnabled = false;
            GetTrades.IsEnabled = false;
            GetOrders.IsEnabled = false;
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.Connect();
            if (QuikConnector.isConnected)
            {
                MessageBox.Show("Connected to QUIK");
            }
            else
            {
                MessageBox.Show("Some error while connectiong to QUIK");
            }
            if (QuikConnector.isConnected)
            {
                Connect.IsEnabled = false;
                GetOrdersBook.IsEnabled = true;
                Ticker.IsEnabled = true;
                SetOrder.IsEnabled = true;
                GetTrades.IsEnabled = true;
                GetOrders.IsEnabled = true;
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SendBid sendbid = new SendBid();
            sendbid.Show();
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Orders().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new Trades().Show();
        }

        private void GetOrdersBook_OnClickr_Click(object sender, RoutedEventArgs e)
        {
            new OrdersBook(Ticker.Text).Show();
        }
    }
}

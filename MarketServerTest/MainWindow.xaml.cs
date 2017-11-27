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
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.Connect();
            MessageBox.Show(QuikConnector.isConnected.ToString());
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SendBid sendbid = new SendBid();
            sendbid.Show();
        }
        private void Select_Ticker_Click(object sender, RoutedEventArgs e)
        {
            currentTicker = Ticker.Text;
            QuikConnector.SubscribeToOrderBook(Ticker.Text, OnQuoteDo);
        }

        public void OnQuoteDo(OrderBook quote)
        {
            if (quote.sec_code.ToUpperInvariant() == currentTicker) //Функция срабатывает на все подписанные стаканы
            {                                               //на сервере нужно придумать способ распихивать стаканы только
                Console.WriteLine("Стакан обновлен");       //тем, кому они реально нужны
                OrderBooksListView.Dispatcher.Invoke(() => OrderBooksListView.Items.Clear());
                foreach (var quoteBid in quote.bid)
                {
                    OrderBooksListView.Dispatcher.Invoke(() => OrderBooksListView.Items.Add(quoteBid));
                }

                foreach (var quoteOffer in quote.offer)
                {
                    OrderBooksListView.Dispatcher.Invoke(() => OrderBooksListView.Items.Add(quoteOffer));
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Orders().Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new Trades().Show();
        }
    }
}

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
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для OrdersBook.xaml
    /// </summary>
    public partial class OrdersBook : Window
    {
        private string ticker;
        public OrdersBook(string ticker)
        {
            InitializeComponent();
            this.ticker = ticker;
            Title = ticker.ToUpper();
            QuikConnector.SubscribeToOrderBook(ticker, OnQuoteDo);
        }

        public void OnQuoteDo(OrderBook quote)
        {
            if (quote.sec_code.ToUpperInvariant() == ticker) //Функция срабатывает на все подписанные стаканы
            {
                //на сервере нужно придумать способ распихивать стаканы только
                //тем, кому они реально нужны
                if (quote.bid == null || quote.offer == null)
                {
                    return;
                }
                OrderBookListView.Dispatcher.Invoke(() =>
                {
                    if (quote.bid.Length + quote.offer.Length == OrderBookListView.Items.Count)
                    {
                        for (int i = 0; i < quote.offer.Length; i++)
                        {
                            var item = new ListViewItem
                            {
                                Foreground = Brushes.Red,
                                Content = quote.offer[i]
                            };
                            OrderBookListView.Items[quote.offer.Length - 1 - i] = item;
                        }
                        for (int i = 0; i < quote.bid.Length; i++)
                        {
                            var item = new ListViewItem
                            {
                                Foreground = Brushes.Green,
                                Content = quote.bid[i]
                            };
                            OrderBookListView.Items[quote.offer.Length + quote.bid.Length - 1 - i] = item;
                        }
                    }
                    else
                    {
                        OrderBookListView.Items.Clear();
                        foreach (var quoteOffer in quote.offer.Reverse())
                        {
                            var item = new ListViewItem
                            {
                                Foreground = Brushes.Red,
                                Content = quoteOffer
                            };
                            OrderBookListView.Items.Add(item);
                        }
                        foreach (var quoteBid in quote.bid.Reverse())
                        {
                            var item = new ListViewItem
                            {
                                Foreground = Brushes.Green,
                                Content = quoteBid
                            };
                            OrderBookListView.Items.Add(item);
                        }
                    }
                });
            }
        }

        private void OrdersBook_OnClosed(object sender, EventArgs e)
        {
            QuikConnector.UnsubsckibeFromOrderBook(ticker);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OrderBook.PriceQuantity selectedOrderBook = (OrderBook.PriceQuantity) OrderBookListView.SelectedItems[0];
            SendBid sendBid = new SendBid(ticker, selectedOrderBook.price);
            sendBid.Show();
        }
    }
}

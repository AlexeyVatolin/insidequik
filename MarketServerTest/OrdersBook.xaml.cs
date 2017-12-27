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
                OrderBooksBidListView.Dispatcher.Invoke(() =>
                {
                    if (quote.bid.Length == OrderBooksBidListView.Items.Count)
                    {
                        for (int i = 0; i < quote.bid.Length; i++)
                        {
                            OrderBooksBidListView.Items[i] = quote.bid[i];
                        }
                    }
                    else
                    {
                        OrderBooksBidListView.Items.Clear();
                        foreach (var quoteBid in quote.bid)
                        {
                            OrderBooksBidListView.Items.Add(quoteBid);
                        }
                    }
                });

                OrderBookOfferListView.Dispatcher.Invoke(() =>
                {
                    if (quote.bid.Length == OrderBookOfferListView.Items.Count)
                    {
                        for (int i = 0; i < quote.offer.Length; i++)
                        {
                            OrderBookOfferListView.Items[i] = quote.offer[i];
                        }
                    }
                    else
                    {
                        OrderBookOfferListView.Items.Clear();
                        foreach (var quoteBid in quote.offer)
                        {
                            OrderBookOfferListView.Items.Add(quoteBid);
                        }
                    }
                });
            }
        }

        private void OrdersBook_OnClosed(object sender, EventArgs e)
        {
            QuikConnector.UnsubsckibeFromOrderBook(ticker);
        }
    }
}

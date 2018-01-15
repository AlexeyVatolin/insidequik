using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MarketServerTest.Data;
using MahApps.Metro.Controls;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для OrdersBook.xaml
    /// </summary>
    public partial class OrdersBook : MetroWindow
    {
        private string ticker;
        private ObservableCollection<OrderBookRow> OrderBookRows { get; set; }
        public OrdersBook(string ticker)
        {
            InitializeComponent();
            this.ticker = ticker;
            Title = ticker.ToUpper();
            QuikConnector.SubscribeToOrderBook(ticker, OnQuoteDo);
            OrderBookRows = new ObservableCollection<OrderBookRow>();
            OrderBookListView.ItemsSource = OrderBookRows;
        }

        public void OnQuoteDo(OrderBook quote)
        {
            if (quote.sec_code.ToUpperInvariant() == ticker) //Функция срабатывает на все подписанные стаканы
            {
                //на сервере нужно придумать способ распихивать стаканы только
                //тем, кому они реально нужны

                //в конце торгов приходит null
                if (quote.bid == null || quote.offer == null)
                {
                    return;
                }
                OrderBookListView.Dispatcher.Invoke(() =>
                {
                    //Сохраняем индекс выделенной строки
                    int selectedItemIndex = -1;
                    if (OrderBookListView.SelectedItem != null)
                    {
                        selectedItemIndex = OrderBookRows.IndexOf((OrderBookRow)OrderBookListView.SelectedItem);
                    }
                    if (quote.bid.Length + quote.offer.Length == OrderBookRows.Count)
                    {
                        var reversedOffer = quote.offer.Reverse().ToList();
                        for (int i = 0; i < quote.offer.Length; i++)
                        {
                            OrderBookRows[i] = new OrderBookRow(reversedOffer[i], "offer");
                        }

                        var reversedBid = quote.bid.Reverse().ToList();
                        for (int i = 0; i < quote.bid.Length; i++)
                        {
                            OrderBookRows[quote.offer.Length + i] = new OrderBookRow(reversedBid[i], "bid");
                        }

                        //Восстанавливаем выделенную строку. 
                        //По дефолту после обновления данных выделение строки спадает
                        if (selectedItemIndex != -1)
                        {
                            OrderBookListView.SelectedItem = OrderBookRows[selectedItemIndex];
                        }
                    }
                    else
                    {
                        OrderBookRows.Clear();
                        foreach (var quoteOffer in quote.offer.Reverse())
                        {
                            OrderBookRows.Add(new OrderBookRow(quoteOffer, "offer"));
                        }
                        foreach (var quoteBid in quote.bid.Reverse())
                        {
                            OrderBookRows.Add(new OrderBookRow(quoteBid, "bid"));
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
            if (OrderBookListView.SelectedItems.Count > 0)
            {
                OrderBookRow selectedOrderBook = (OrderBookRow) OrderBookListView.SelectedItems[0];
                SendBid sendBid = new SendBid(ticker, selectedOrderBook.Price);
                sendBid.Show();
            }
        }
    }
}

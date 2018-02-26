using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Interfaces;
using Common.Models;
using MarketServerTest.Helpers;
using MarketServerTest.SignalR;

namespace MarketServerTest.ViewModels
{
    public class OrderBookViewModel : TickClientBase, IOrderBook
    {
        public ObservableCollection<OrderBookMST> OrderBooks { get; set; } = new ObservableCollection<OrderBookMST>();
        public OrderBook SelectedOrderBook { get; set; }
        public event OpenWindow ShowSendOrderWindow;
        public event OpenWindow ShowSendStopOrderWindow;
        public delegate void OpenWindow(string ticker, double price);

        public OrderBookViewModel(string ticker)
        {
            Ticker = ticker;
            OnQuoteAction = OnQuote;
            Connect();
        }

        public void OnQuote(List<OrderBook> orderBooks)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (orderBooks.Count != OrderBooks.Count)
                {
                    foreach (var orderBook in orderBooks) 
                    {
                        OrderBooks.Add(new OrderBookMST() {Price = orderBook.Price,  Quantity = orderBook.Quantity, Type = orderBook.Type});
                    }
                }
                else
                {
                    for (int i = 0; i<orderBooks.Count;i++)
                    {
                        OrderBooks[i] = new OrderBookMST() { Price = orderBooks[i].Price, Quantity = orderBooks[i].Quantity, Type = orderBooks[i].Type };
                    }
                }
            });
        }

        public ICommand DisconnectCommand
        {
            get
            {
                //Вызывается при открытии окна. Почему?
                //return new RelayCommand(obj => Disconnect());
                return new RelayCommand(o => Console.WriteLine(1));
            }
        }

        public ICommand SendOrderCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (SelectedOrderBook != null)
                    {
                        ShowSendOrderWindow?.Invoke(Ticker, SelectedOrderBook.Price);
                    }
                });
            }
        }

        public ICommand SendStopOrderCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    if (SelectedOrderBook != null)
                    {
                        ShowSendStopOrderWindow?.Invoke(Ticker, SelectedOrderBook.Price);
                    }
                });
            }
        }
    }
}

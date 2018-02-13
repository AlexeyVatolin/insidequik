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
        public ObservableCollection<OrderBook> OrderBooks { get; set; } = new ObservableCollection<OrderBook>();
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
            OrderBooks = new ObservableCollection<OrderBook>(orderBooks);
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

﻿using MahApps.Metro.Controls;
using MarketServerTest.ViewModels;

namespace MarketServerTest.Views
{
    /// <summary>
    /// Логика взаимодействия для OrdersBook.xaml
    /// </summary>
    public partial class OrdersBook : MetroWindow
    {
        public OrdersBook(string ticker)
        {
            InitializeComponent();
            Title = ticker;
            OrderBookViewModel viewModel = new OrderBookViewModel(ticker);
            viewModel.ShowSendOrderWindow += ShowSendOrderWindow;
            viewModel.ShowSendStopOrderWindow += ShowSendStopOrderWindow;
            DataContext = viewModel;
        }

        private void ShowSendOrderWindow(string ticker, double price)
        {
            SendOrder sendOrder = new SendOrder(ticker, price);
            sendOrder.Show();
        }

        private void ShowSendStopOrderWindow(string ticker, double price)
        {
            SendStopOrder sendStopOrder = new SendStopOrder(ticker);
            sendStopOrder.Show();
        }
    }
}

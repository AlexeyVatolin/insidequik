using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Microsoft.AspNet.SignalR.Client;
using Common.Models;
using System.Globalization;
using QuikSharp.DataStructures;
using MarketServerTest.ViewModels;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для SendStopOrder.xaml
    /// </summary>
    public partial class SendStopOrder : MetroWindow
    {
        public SendStopOrder()
        {
            InitializeComponent();
            var viewModel = new SendOrderViewModel();
            DataContext = viewModel;
        }

        public SendStopOrder(string ticker)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
            var viewModel = new SendOrderViewModel();
            DataContext = viewModel;
        }

       
        private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((!Char.IsDigit(e.Text, 0)) && (e.Text != ",") && (e.Text != "."))
            {
                e.Handled = true;
            }
        }

        private void QuantityBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void QuantityBoxAndPriceBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}

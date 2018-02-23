using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.AspNet.SignalR.Client;
using Common.Models;
using QuikSharp.DataStructures;
using MarketServerTest.ViewModels;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для SendBid.xaml
    /// </summary>
    public partial class SendOrder : MetroWindow
    {
        public SendOrder()
        {
            InitializeComponent();
            var viewModel = new SendOrderViewModel();
            DataContext = viewModel;
        }

        public SendOrder(string ticker)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
            var viewModel = new SendOrderViewModel();
            DataContext = viewModel;
        }

        public SendOrder(string ticker, double price)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
            PriceBox.Text = price.ToString(CultureInfo.InvariantCulture);
            var viewModel = new SendOrderViewModel();
            DataContext = viewModel;

        }
        

        private void PriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != "," && e.Text != ".")
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
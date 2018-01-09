using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для SendBid.xaml
    /// </summary>
    public partial class SendBid : Window
    {
        public SendBid()
        {
            InitializeComponent();
        }

        public SendBid(string ticker, double price)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
            PriceBox.Text = price.ToString(CultureInfo.InvariantCulture);
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.SendBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), Int32.Parse(QuantityBox.Text),
                QuikSharp.DataStructures.Operation.Buy, MarketPrice.IsChecked.Value);
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.SendBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), Int32.Parse(QuantityBox.Text),
                QuikSharp.DataStructures.Operation.Sell, MarketPrice.IsChecked.Value);
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
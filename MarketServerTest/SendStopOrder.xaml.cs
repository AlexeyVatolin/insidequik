using System;
using System.Windows;
using System.Windows.Input;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для SendStopOrder.xaml
    /// </summary>
    public partial class SendStopOrder : Window
    {
        public SendStopOrder()
        {
            InitializeComponent();
        }

        public SendStopOrder(string ticker)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Sell.IsChecked == true)
                QuikConnector.SendStopOrderBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), 
                    Decimal.Parse(Price2Box.Text), Int32.Parse(QuantityBox.Text), 
                    QuikSharp.DataStructures.Operation.Sell);
            if (Buy.IsChecked == true)
                QuikConnector.SendStopOrderBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), 
                    Decimal.Parse(Price2Box.Text), Int32.Parse(QuantityBox.Text), 
                    QuikSharp.DataStructures.Operation.Buy);
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

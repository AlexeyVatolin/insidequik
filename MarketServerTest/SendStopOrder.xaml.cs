using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Microsoft.AspNet.SignalR.Client;
using Common.Models;
using System.Globalization;
using QuikSharp.DataStructures;

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
        }

        public SendStopOrder(string ticker)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var hubConnection = new HubConnection("http://localhost:8080/signalr", false)
            {
                TraceLevel = TraceLevels.All
            };
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("SendOrderHub");
            await hubConnection.Start();

            Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            decimal price = Decimal.Parse(PriceBox.Text.Replace('.', separator));
            decimal price2 = Decimal.Parse(Price2Box.Text.Replace('.', separator));


            //if (Sell.IsChecked == true)
            //    QuikConnector.SendStopOrderBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), 
            //        Decimal.Parse(Price2Box.Text), Int32.Parse(QuantityBox.Text), 
            //        QuikSharp.DataStructures.Operation.Sell);
            //if (Buy.IsChecked == true)
            //    QuikConnector.SendStopOrderBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), 
            //        Decimal.Parse(Price2Box.Text), Int32.Parse(QuantityBox.Text), 
            //        QuikSharp.DataStructures.Operation.Buy);

            ClientStopOrder newOrder = new ClientStopOrder
            {
                SecurityCode = TickerBox.Text,
                Operation = (bool)Buy.IsChecked ? Operation.Buy : Operation.Sell,
                Price = price,
                Price2= price2,
                Quantity = Int32.Parse(QuantityBox.Text),
            };
            var result = await stockTickerHubProxy.Invoke<object>("SendStopOrder", newOrder);
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

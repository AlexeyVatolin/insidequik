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

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для SendBid.xaml
    /// </summary>
    public partial class SendBid : MetroWindow
    {
        public SendBid()
        {
            InitializeComponent();
        }

        public SendBid(string ticker)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
        }

        public SendBid(string ticker, double price)
        {
            InitializeComponent();
            TickerBox.Text = ticker;
            PriceBox.Text = price.ToString(CultureInfo.InvariantCulture);
        }
        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            var hubConnection = new HubConnection("http://localhost:8080/signalr", false)
            {
                TraceLevel = TraceLevels.All
            };
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("SendOrderHub");
            await hubConnection.Start();

            Char separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            decimal price = Decimal.Parse(PriceBox.Text.Replace('.',separator));

            //if (Buy.IsChecked == true)
            //    QuikConnector.SendBid(ClassCodeBox.Text,TickerBox.Text, price, 
            //        Int32.Parse(QuantityBox.Text), QuikSharp.DataStructures.Operation.Buy, 
            //        MarketPrice.IsChecked.Value);
            //if (Sell.IsChecked == true)
            //    QuikConnector.SendBid(ClassCodeBox.Text,TickerBox.Text,price, Int32.Parse(QuantityBox.Text), QuikSharp.DataStructures.Operation.Sell,
            //        MarketPrice.IsChecked.Value);
            ClientOrder newOrder = new ClientOrder
            {
                SecurityCode = TickerBox.Text,
                ClassCode = ClassCodeBox.Text,
                Operation = (bool)Buy.IsChecked? Operation.Buy : Operation.Sell,
                Price = price,
                Quantity = Int32.Parse(QuantityBox.Text),
                MarketPrice = MarketPrice.IsChecked.Value
            };
            var result = await stockTickerHubProxy.Invoke<object>("SendOrder", newOrder);

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
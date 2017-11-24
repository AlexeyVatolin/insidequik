using System;
using System.Collections.Generic;
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

        private void Buy_Click(object sender, RoutedEventArgs e)
        {       
            QuikConnector.SendBid(TickerBox.Text,Decimal.Parse(PriceBox.Text),Int32.Parse(QuantityBox.Text),QuikSharp.DataStructures.Operation.Buy,MarketPrice.IsChecked.Value);
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            QuikConnector.SendBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), Int32.Parse(QuantityBox.Text), QuikSharp.DataStructures.Operation.Sell, MarketPrice.IsChecked.Value);
        }
    }
}

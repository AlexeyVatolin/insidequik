﻿using System;
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
    /// Логика взаимодействия для StopOrderBid.xaml
    /// </summary>
    public partial class StopOrderBid : Window
    {
        public StopOrderBid()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Sell.IsChecked == true)
                QuikConnector.SendStopOrderBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), Decimal.Parse(Price2Box.Text), Int32.Parse(QuantityBox.Text), QuikSharp.DataStructures.Operation.Sell);
            if (Buy.IsChecked == true)
                QuikConnector.SendStopOrderBid(TickerBox.Text, Decimal.Parse(PriceBox.Text), Decimal.Parse(Price2Box.Text), Int32.Parse(QuantityBox.Text), QuikSharp.DataStructures.Operation.Buy);
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
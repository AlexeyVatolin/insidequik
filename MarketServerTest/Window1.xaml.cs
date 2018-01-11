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
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            ListView.Items.Add(new OrderBook.PriceQuantity() {price = 10.0, quantity = 12.0});
            ListView.Items.Add(new OrderBook.PriceQuantity() {price = 11.0, quantity = 13.0});
            ListView.Items.Add(new OrderBook.PriceQuantity() {price = 12.0, quantity = 14.0});
            ListView.Items.Add(new OrderBook.PriceQuantity() {price = 13.0, quantity = 15.0});
        }
    }
}

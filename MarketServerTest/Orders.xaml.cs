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
using QuikSharp.DataStructures.Transaction;
namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для BidsAndDeals.xaml
    /// </summary>
    public partial class Orders : Window
    {
        public Orders()
        {
            InitializeComponent();
            InitializeBidsTable();
        }

        public void InitializeBidsTable()
        {
            List<Order> list = new List<Order>();
            list = QuikConnector.GetOrders();
            foreach (var item in list)
            {
                OrdersTable.Items.Add(new ColumnsForOrders
                {
                    Company = item.SecCode.ToString(),
                    ClassCode = item.ClassCode.ToString(),
                    Operation = item.Operation.ToString(),
                    Quantity = item.Quantity.ToString(),
                    Price = item.Price.ToString(),
                    Time = item.Datetime.hour.ToString() + ":" + item.Datetime.min.ToString() + ":" + item.Datetime.sec.ToString(),
                    Balance = item.Balance.ToString(),
                    Value = item.Value.ToString(),
                    State = item.State.ToString()
                });
            }
        }
    }
}

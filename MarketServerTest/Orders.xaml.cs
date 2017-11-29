using System.Globalization;
using System.Windows;

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
            var list = QuikConnector.GetOrders();
            foreach (var item in list)
            {
                OrdersTable.Items.Add(new ColumnsForOrders
                {
                    Company = item.SecCode,
                    ClassCode = item.ClassCode,
                    Operation = item.Operation.ToString(),
                    Quantity = item.Quantity.ToString(),
                    Price = item.Price.ToString(CultureInfo.InvariantCulture),
                    Time = item.Datetime.hour.ToString("00") + ":" + item.Datetime.min.ToString("00")
                    + ":" + item.Datetime.sec.ToString("00") + "." + item.Datetime.ms,
                    Balance = item.Balance.ToString(),
                    Value = item.Value.ToString(CultureInfo.InvariantCulture),
                    State = item.State.ToString()
                });
            }
        }
    }
}

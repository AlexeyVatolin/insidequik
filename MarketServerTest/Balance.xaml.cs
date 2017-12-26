using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;
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
    /// Логика взаимодействия для Balance.xaml
    /// </summary>
    public partial class Balance : Window
    {
        private List<DepoLimitEx> list = new List<DepoLimitEx>();
        private List<MoneyLimitEx> moneyLimit = new List<MoneyLimitEx>();

        public Balance()
        {
            InitializeComponent();
            InitializeOrdersTable();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //QuikConnector.CancelOrder(list[OrdersTable.SelectedIndex]);
        }


        public void InitializeOrdersTable()
        {
           

            list = QuikConnector.GetDepoLimits();
            moneyLimit = QuikConnector.GetMoneyLimit();
            foreach (var item in moneyLimit)
            {
                if (item != null && item.LimitKind == 2)
                    BalanceTableMoney.Items.Add(new ColumnsForBalance(item));
            }
            foreach (var item in list)
            {
                if (item != null && item.LimitKindInt == 2) //Заполняем только по T2
                    BalanceTableSecurities.Items.Add(new ColumnsForBalance(item));
            }
           
        }
    }
}

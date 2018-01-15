using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using MahApps.Metro.Controls;
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
using System.Threading;
using System.ComponentModel;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Balance.xaml
    /// </summary>
    public partial class Balance : MetroWindow
    {
        private List<DepoLimitEx> depoLimit = new List<DepoLimitEx>();
        private List<MoneyLimitEx> moneyLimit = new List<MoneyLimitEx>();  
        private static Timer timer;

        public Balance()
        {
            InitializeComponent();

            InitializeOrdersTable();
            timer = new Timer(Callback, null, 1000 * 3, Timeout.Infinite);
        }
        private void Balance_OnClosing(object sender, CancelEventArgs e)
        {
            timer.Dispose();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //QuikConnector.CancelOrder(list[OrdersTable.SelectedIndex]);
        }

        private void Callback(Object state)
        {
            // Long running operation
            InitializeOrdersTable();
            timer.Change(1000 * 3, Timeout.Infinite);
        }
        public void InitializeOrdersTable()
        {

            depoLimit = BalanceWorker.GetDepoLimit();
            moneyLimit = BalanceWorker.GetMoneyLimit();

            BalanceTableMoney.Dispatcher.Invoke(() =>
            {
                BalanceTableMoney.Items.Clear();
                foreach (var item in moneyLimit)
                {
                    if (item != null && item.LimitKind == 2)
                    {
                        BalanceTableMoney.Items.Add(new MoneyLimit(item));
                    }
                }
            });

            BalanceTableSecurities.Dispatcher.Invoke(() =>
            {
                BalanceTableSecurities.Items.Clear();
                foreach (var item in depoLimit)
                {
                    if (item != null && item.LimitKindInt == 2) //Заполняем только по T2
                    {
                        BalanceTableSecurities.Items.Add(new DepoLimit(item));
                    }
                }
                dProfit.Content = BalanceWorker.GetDayProfit().ToString(); //Label dProfit
                userLimit.Content = QuikConnector.userAccount.userLimit;
            });
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MarketServerTest.DataRows;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Securities.xaml
    /// </summary>
    public partial class Securities : Window
    {
        public ObservableCollection<SecuritiesRow> SecurityInfos { get; set; }
        private Timer timer;
        public Securities()
        {
            InitializeComponent();
            
        }

        public void Initialize(List<SecurityInfo> securityInfos)
        {
            foreach (var securityInfo in securityInfos)
            {
                SecurityInfos.Add(new SecuritiesRow
                {
                    ClassCode = securityInfo.ClassCode,
                    SecCode = securityInfo.SecCode,
                    Name = securityInfo.Name
                });
            }
            timer = new Timer
            {
                Enabled = true,
                Interval = 1000
            };
            timer.Elapsed += UpdateTable;
        }

        private void UpdateTable(object sender, ElapsedEventArgs e)
        {
            foreach (var securityInfo in SecurityInfos)
            {
                QuikConnector.UpdateSecurityInfo(securityInfo);
            }
        }
    }
}

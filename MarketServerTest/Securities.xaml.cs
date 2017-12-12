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
    /// Логика взаимодействия для Securities.xaml
    /// </summary>
    public partial class Securities : Window
    {
        public List<SecurityInfo> SecurityInfos;
        public Securities(List<SecurityInfo> securityInfos)
        {
            this.SecurityInfos = securityInfos;
            InitializeComponent();
            SecuritiesListView.ItemsSource = SecurityInfos;
        }
    }
}

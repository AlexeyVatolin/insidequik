using MarketServerTest.Interfaces;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для MyPasswordBox.xaml
    /// </summary>
    public partial class MyPasswordBox : UserControl, IPasswordSupplier
    {
        public MyPasswordBox()
        {
            InitializeComponent();
        }

        public string GetPassword()
        {
            return pwdBox.Password;
        }
    }
}

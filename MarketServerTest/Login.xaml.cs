using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {

            Loading.Visibility = Visibility.Visible;
            bool isConnected = await Task.Run(QuikConnector.Connect);
            Loading.Visibility = Visibility.Hidden;
            if (isConnected)
            {
                Hide();
                new MainWindow().ShowDialog();
                Close();
            }
            else
            {
                await this.ShowMessageAsync("Error", "Some error while connecting to QUIK");
            }
        }
    }
}

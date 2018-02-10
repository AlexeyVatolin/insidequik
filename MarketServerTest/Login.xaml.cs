using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        ProgressDialogController controller;
        public Login()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            controller = await this.ShowProgressAsync("Connecting...", "");
            controller.SetIndeterminate();
            bool isConnected = await Task.Run(QuikConnector.Connect);
            if (isConnected)
            {
                await controller.CloseAsync();
                Hide();
                new MainWindow().ShowDialog();
                Close();
            }
            else
            {
                controller.Canceled += Controller_Canceled;
                controller.SetCancelable(true);
                controller.SetProgress(0);
                controller.SetTitle("Error");
                controller.SetMessage("Some error while connecting to QUIK");
            }
        }

        private async void Controller_Canceled(object sender, EventArgs e)
        {
            await controller.CloseAsync();
        }
    }
}

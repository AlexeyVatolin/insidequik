using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MarketServerTest.ViewModels;

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
            var viewModel = new LoginViewModel(DialogCoordinator.Instance);
            viewModel.ShowMainWindow += ShowMainWindow;
            DataContext = viewModel;
        }

        private void ShowMainWindow(object sender, EventArgs args)
        {
            Hide();
            new MainWindow().Show();
            Close();
        }
        //private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    controller = await this.ShowProgressAsync("Connecting...", "");
        //    controller.SetIndeterminate();
        //    bool isConnected = await Task.Run(QuikConnector.Connect);
        //    if (isConnected)
        //    {
        //        await controller.CloseAsync();
        //        Hide();
        //        new MainWindow().ConnectToQuik();
        //        Close();
        //    }
        //    else
        //    {
        //        controller.Canceled += Controller_Canceled;
        //        controller.SetCancelable(true);
        //        controller.SetProgress(0);
        //        controller.SetTitle("Error");
        //        controller.SetMessage("Some error while connecting to QUIK");
        //    }
        //}

        //private async void Controller_Canceled(object sender, EventArgs e)
        //{
        //    await controller.CloseAsync();
        //}
    }
}

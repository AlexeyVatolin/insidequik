using System;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MarketServerTest.ViewModels;
using Unity;
using MarketServerTest.Interfaces;
using Microsoft.AspNet.SignalR.Client;

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
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IPasswordSupplier>(pwdBox);
            var viewModel = new LoginViewModel(DialogCoordinator.Instance, container);
            viewModel.ShowMainWindow += ShowMainWindow;
            DataContext = viewModel;
        }
        private void ShowMainWindow(object sender, EventArgs args)
        {
            Hide();
            new MainWindow().Show();
            Close();
        }
    }
}

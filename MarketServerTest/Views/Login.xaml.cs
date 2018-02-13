using System;
using MahApps.Metro.Controls;
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
    }
}

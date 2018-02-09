using System;
using System.Windows;
using Common.Extensions;
using Common.Models;
using Microsoft.AspNet.SignalR.Client;

namespace ClientTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            var hubConnection = new HubConnection("http://localhost:8080/signalr", false)
                {
                    TraceLevel = TraceLevels.All
                };
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("LoginTestHub");
            //stockTickerHubProxy.On<object>("Login", OnLogin);
            await hubConnection.Start();
            try
            {
                var result = await stockTickerHubProxy.Invoke<object>("Login",
                    new LoginOptionsRequest() { Login = Login.Text, Password = Password.Text });
            }
            catch (Exception exception)
            {
                if (exception.Message == ErrorCodes.WrongUserNameOrPassword.GetDescription())
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }

        private void OnLogin(object data)
        {
            Console.WriteLine("Stock update for new price");
        }
    }
}

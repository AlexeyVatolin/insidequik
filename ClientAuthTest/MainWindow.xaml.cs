using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNet.SignalR.Client;

namespace ClientAuthTest
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var connection = new HubConnection("http://localhost:8000/signalr");

            IHubProxy proxy = connection.CreateHubProxy("RemoteLogin");
            await connection.Start();

            string sessionId = await proxy.Invoke<string>("Login", "admin", "admin123");

            connection.Stop();

            connection.Headers.Add("sessionId", sessionId);
            IHubProxy testProxy = connection.CreateHubProxy("TestDataHub");
            await connection.Start();

            string hello = await testProxy.Invoke<string>("SayHello");

        }

        private bool AuthenticateUser(string user, string password, out Cookie authCookie)
        {
            var request = WebRequest.Create("https://www.contoso.com/RemoteLogin") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();

            var authCredentials = "UserName=" + user + "&Password=" + password;
            byte[] bytes = Encoding.UTF8.GetBytes(authCredentials);
            request.ContentLength = bytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                authCookie = response.Cookies[FormsAuthentication.FormsCookieName];
            }

            if (authCookie != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

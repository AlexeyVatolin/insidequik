using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.HockeyApp;//for logs


namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private App()
        {   
        }
       protected override async void OnStartup(StartupEventArgs e)
        {
            //hockeayapp logs
            HockeyClient.Current.Configure("ec1a26cdd29c48ada16e2ae641920989");
            await HockeyClient.Current.SendCrashesAsync(true);
        }
    }
}

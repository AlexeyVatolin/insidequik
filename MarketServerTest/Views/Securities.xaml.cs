using System.Collections.Generic;
using MahApps.Metro.Controls;
using MarketServerTest.ViewModels;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для Securities.xaml
    /// </summary>
    public partial class Securities : MetroWindow
    {
        public Securities(List<SecurityInfo> securityInfos)
        {
            InitializeComponent();
            //TODO: не тру передавать параметры в ViewModel, лучше регистрировать DattaConatext в View. Придумать как избавиться от этого
            DataContext = new SecuritiesViewModel(securityInfos);
        }
    }
}

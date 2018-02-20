using System.Collections.Generic;
using System.Timers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MarketServerTest.Helpers;
using MarketServerTest.Models;
using QuikSharp.DataStructures;

namespace MarketServerTest.ViewModels
{
    public class SecuritiesViewModel
    {
        public ObservableCollection<SecuritiesModel> Securities { get; set; } = new ObservableCollection<SecuritiesModel>();
        public SecuritiesModel SelectedItem { get; set; }

        public ICommand DisposeOnClosingCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    _timer.Dispose();
                });
            }
        }

        public ICommand NewBidCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    new SendOrder(SelectedItem.SecCode).Show();
                },
                (obj) => SelectedItem != null);
            }
        }

        public ICommand NewStopOrder
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                  new SendStopOrder(SelectedItem.SecCode).Show();
                },
                (obj) => SelectedItem != null);
            }

        }

        private Timer _timer;

        public SecuritiesViewModel(List<SecurityInfo> securityInfos)
        {
            foreach (var securityInfo in securityInfos)
            {
                Securities.Add(new SecuritiesModel
                {
                    ClassCode = securityInfo.ClassCode,
                    SecCode = securityInfo.SecCode,
                    Name = securityInfo.Name
                });
            }
            _timer = new Timer
            {
                Enabled = true,
                Interval = 1000
            };
            _timer.Elapsed += UpdateTable;
        }

        private void UpdateTable(object sender, ElapsedEventArgs e)
        {
            foreach (var securityInfo in Securities)
            {
                QuikConnector.UpdateSecurityInfo(securityInfo);
            }

        }
    }
}

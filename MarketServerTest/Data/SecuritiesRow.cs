using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketServerTest.DataRows
{
    public class SecuritiesRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _name;
        private double _lastPrice;
        private double _changePercent;
        private string _flow;
        public string ClassCode { get; set; }
        public string SecCode { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }

        public double LastPrice
        {
            get => _lastPrice;
            set
            {
                _lastPrice = value;
                OnPropertyChanged(nameof(LastPrice));
            }
        }

        public double ChangePercent
        {
            get => _changePercent;
            set
            {
                _changePercent = value;
                OnPropertyChanged(nameof(ChangePercent));
            }
        }

        public string Flow
        {
            get => _flow;
            set
            {
                _flow = value;
                OnPropertyChanged(nameof(Flow));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

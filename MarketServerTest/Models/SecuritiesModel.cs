using MarketServerTest.Helpers;

namespace MarketServerTest.Models
{
    public class SecuritiesModel : NotifierMST
    {
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
                NotifyPropertyChanged();
            }
        }

        public double LastPrice
        {
            get => _lastPrice;
            set
            {
                _lastPrice = value;
                NotifyPropertyChanged();
            }
        }

        public double ChangePercent
        {
            get => _changePercent;
            set
            {
                _changePercent = value;
                NotifyPropertyChanged();

            }
        }

        public string Flow
        {
            get => _flow;
            set
            {
                _flow = value;
                NotifyPropertyChanged();
            }
        }
    }
}

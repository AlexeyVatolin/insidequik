using MarketServerTest.Helpers;

namespace Common.Models
{
    public class OrderBook : Notifier
    {
        private double _price;
        private double _qantity;
        private string _type;

        public double Price
        {
            get => _price;
            set
            {
                _price = value; 
                NotifyPropertyChanged();
            }
        }

        public double Quantity
        {
            get => _qantity;
            set
            {
                _qantity = value;
                NotifyPropertyChanged();
            } 
        }
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                NotifyPropertyChanged();
            } 
        }
    }
}

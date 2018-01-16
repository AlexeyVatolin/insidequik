using System.ComponentModel;
using QuikSharp.DataStructures;

namespace Server.Data
{
    class OrderBookRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double _price;
        private double _quantity;
        private string _type;

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public OrderBookRow(OrderBook.PriceQuantity priceQuantity, string type)
        {
            Price = priceQuantity.price;
            Quantity = priceQuantity.quantity;
            Type = type;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace finam.ru_economic_calendar
{
    public class Post : INotifyPropertyChanged
    {
        private string _date;
        private string _time;
        private string _name;
        private string _country;
        private string _dateSecret;
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }
        }
        public string DateSecret
        {
            get { return _dateSecret; }
            set
            {
                _dateSecret = value;
                OnPropertyChanged("DateSecret");
            }
        }
        public Post()
        {
            _date = string.Empty;
            _time = string.Empty;
            _name = string.Empty;
            _country = string.Empty;
            _dateSecret = string.Empty;

        }
        public Post(string date, string time, string name, string country)
        {
            Date = date;
            Time = time;
            Name = name;
            Country = country;
        }
        public Post(string date, string time, string name, string country, string dateSec)
        {
            Date = date;
            Time = time;
            Name = name;
            Country = country;
            DateSecret = dateSec;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

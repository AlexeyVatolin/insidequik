using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace finam.ru_economic_calendar.Class
{
    public class Item : INotifyPropertyChanged
    {
        private string _date;
        private string _time;
        private string _last;
        private string _vol;
        private string _id;
        private string _oper;

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
        public string Last
        {
            get { return _last; }
            set
            {
                _last = value;
                OnPropertyChanged("Last");
            }
        }
        public string Vol
        {
            get { return _vol; }
            set
            {
                _vol = value;
                OnPropertyChanged("Vol");
            }
        }
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Oper
        {
            get { return _oper; }
            set
            {
                _oper = value;
                OnPropertyChanged("Oper");
            }
        }
        public Item(string date, string time, string last, string vol, string id, string oper)
        {
            Date = date.Insert(4, ".").Insert(7, ".");
            Time = time.Insert(2, ":").Insert(5, ":");
            Last = last;
            Vol = vol;
            Id = id;
            Oper = oper;
        }

        public Item(params string[] parameters) : this(date: parameters[0], time: parameters[1], last: parameters[2], vol: parameters[3], id: parameters[4], oper: parameters[5]) { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

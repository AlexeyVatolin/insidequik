using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace finam.ru_economic_calendar.Class
{
    class TableModelCotirovki : INotifyPropertyChanged
    {
        private TaskScheduler _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private bool _isReading = false;
        private string _title;
        private string _min;
        private string _price;
        private string _max;
        private bool _isUpdating = false;
        private List<Item> _buffer;
        public static ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public string TITLE
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("TITLE");
            }
        }
        public string MIN
        {
            get { return _min; }
            set
            {
                _min = value;
                OnPropertyChanged("MIN");
            }
        }
        public string MAX
        {
            get { return _max; }
            set
            {
                _max = value;
                OnPropertyChanged("MAX");
            }
        }
        public string PRICE
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("PRICE");
            }
        }
        public InfoModel Info { set; get; }
        public bool IsReading
        {
            set
            {
                _isReading = value;
                OnPropertyChanged();
            }
            get
            {
                return _isReading;
            }
        }
        public bool IsUpdating
        {
            set
            {
                _isUpdating = value;
                OnPropertyChanged();
            }
            get
            {
                return _isUpdating;
            }
        }
        private void ReadTask()
        {
            IsReading = true;
            _buffer = new List<Item>();
            string pathTest = "Files/" + Info.NameFile;
            using (StreamReader fs = new StreamReader(pathTest))
            {
                const char delimiter = ';';
                while (true)
                {
                    string value = fs.ReadLine();
                    if (value == "<DATE>;<TIME>;<LAST>;<VOL>;<ID>;<OPER>")
                    { }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        _buffer.Add(new Item(value.Split(delimiter)));
                    }
                    else if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                }
            }
            IsReading = false;
        }
        private void LoadItemsToPanel()
        {
            Items.Clear();
            DateTime time1 = DateTime.Parse(Info.TimeSearch);
            double tik = Convert.ToDouble(Info.Tiker);
            DateTime time2 = time1.AddMinutes(Convert.ToDouble(tik));


            //double min = Price;
            //double max = Price;
            IsUpdating = true;
            //            DateTime timeNew = Convert.ToDateTime(Info.TimeSearch).AddMinutes(0.01667);
            for (var i = 0; i < _buffer.Count; ++i)
            {
                if (Convert.ToDateTime(_buffer[i].Time) >= Convert.ToDateTime(Info.TimeSearch) && Convert.ToDateTime(_buffer[i].Time) <= time2)
                {
                    PRICE = _buffer[i].Last;
                }

            }
            double min = 0;
            double max = 0;
            if (PRICE != null)
            {
                min = Convert.ToDouble(PRICE.Replace(".", ","));
                max = Convert.ToDouble(PRICE.Replace(".", ","));
            }
            for (var i = 0; i < _buffer.Count; ++i)
            {
                if (DateTime.Parse(_buffer[i].Time) >= time1 && DateTime.Parse(_buffer[i].Time) <= time2)
                {
                    if (double.Parse(_buffer[i].Last.Replace(".", ",").Trim()) < min)
                    {
                        min = double.Parse(_buffer[i].Last.Replace(".", ",").Trim());
                    }
                    else if (double.Parse(_buffer[i].Last.Replace(".", ",").Trim()) > max)
                    {
                        max = double.Parse(_buffer[i].Last.Replace(".", ",").Trim());
                    }
                    Items.Add(_buffer[i]);
                }

            }
            if (Items.Count == 0)
            {
                TITLE = "Статистики за данный период нет!!!";
            }
            else
            {
                MIN = "MIN: " + min.ToString();
                MAX = "MAX: " + max.ToString();
            }
            IsUpdating = false;
        }

        public TableModelCotirovki(InfoModel _info)
        {
            Info = _info;
            Task.Factory.StartNew(ReadTask).ContinueWith(t => LoadItemsToPanel(), _uiScheduler);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

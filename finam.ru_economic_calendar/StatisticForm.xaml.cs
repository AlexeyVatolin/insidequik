using finam.ru_economic_calendar.Class;
using MahApps.Metro.Controls;

namespace finam.ru_economic_calendar
{
    /// <summary>
    /// Логика взаимодействия для StatisticForm.xaml
    /// </summary>
    public partial class StatisticForm : MetroWindow
    {
        public InfoModel INFO { set; get; }
        public StatisticForm(InfoModel _info)
        {
            INFO = _info;
            InitializeComponent();
            DataContext = new TableModelCotirovki(INFO);
        }

        private void MetroWindow_Closed(object sender, System.EventArgs e)
        {

        }
    }
}

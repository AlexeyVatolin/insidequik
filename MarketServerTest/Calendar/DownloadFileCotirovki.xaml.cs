using finam.ru_economic_calendar.Class;
using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace finam.ru_economic_calendar
{
    /// <summary>
    /// Логика взаимодействия для DownloadFileCotirovki.xaml
    /// </summary>
    public partial class DownloadFileCotirovki : MetroWindow
    {
        public Post post = new Post();
        public DownloadFileCotirovki()
        {
            InitializeComponent();
        }

        private void Donwload_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem temp = (ComboBoxItem)cb_Time.SelectedItem;
            if (tb_NameCotirovka.Text == "")
            {
                MessageBox.Show("Введите название котировки!");
                //await this.ShowMessageAsync("Введите название котировки", "Ошибка...");
            }
            else
            {
                string[] test = post.DateSecret.Split(' ');
                string month = DateConvert(test[1]);
                string date1Day;
                if (Convert.ToInt32(test[0]) < 10)
                {
                    date1Day = "0" + test[0];
                }
                else
                {
                    date1Day = test[0];
                }

                string date1Month = DateConvert(test[1]);
                string date1Year = DateTime.Now.Year.ToString();
                string d1 = date1Year + date1Month + date1Day;
                string d1str = date1Day + "." + date1Month + "." + date1Year; ;

                string nameContr = tb_NameCotirovka.Text;
                string fileName = nameContr.ToUpper() + "_" + d1 + "_" + d1;

                string str = string.Format("http://export.finam.ru/{0}.csv?market=1&em=16842&code={1}&apply=0&df={2}&mf={3}&yf={4}&from={5}&dt={6}&mt={7}&yt={8}&to={9}&p=1&f={10}&e=.csv&cn={11}&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=3&sep2=2&datf=12&at=1", fileName, nameContr, date1Day, Convert.ToInt32(date1Month) - 1, date1Year, d1str, date1Day, Convert.ToInt32(date1Month) - 1, date1Year, d1str, fileName, nameContr);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(str);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
                {
                    //пишем то что нам пришло в файл
                    File.WriteAllText(string.Format("Files/{0}.txt", fileName), stream.ReadToEnd());
                }

                string value = temp.Tag.ToString();

                InfoModel infoModel = new InfoModel(fileName + ".txt", value, post.Time);

                StatisticForm SF = new StatisticForm(infoModel);
                SF.Show();
            }


        }
        public string DateConvert(string date)
        {
            string month = string.Empty;
            switch (date)
            {
                case "Янв": { month = "01"; break; }
                case "Фев": { month = "02"; break; }
                case "Мрт": { month = "03"; break; }
                case "Апр": { month = "04"; break; }
                case "Май": { month = "05"; break; }
                case "Июн": { month = "06"; break; }
                case "Июл": { month = "07"; break; }
                case "Авг": { month = "08"; break; }
                case "Сен": { month = "09"; break; }
                case "Окт": { month = "10"; break; }
                case "Ноя": { month = "11"; break; }
                case "Дек": { month = "12"; break; }
            }
            return month;
        }
    }
}

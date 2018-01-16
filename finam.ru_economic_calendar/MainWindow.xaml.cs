using finam.ru_economic_calendar.Class;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace finam.ru_economic_calendar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        public List<UniqTimer> listTimers = new List<UniqTimer>();
        static double width = SystemParameters.FullPrimaryScreenWidth;
        static double height = SystemParameters.FullPrimaryScreenHeight;
        public int timerMin = 15;
        WindowMessage WM;

        public static string UserIdTelegram = string.Empty;
        MyTelegram myTel = new MyTelegram();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
            new Window1().Show();
        }
        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //сформированый http запрос
        //    string str = string.Format("http://export.finam.ru/{0}.csv?market=1&em=16842&code={1}&apply=0&df={2}&mf={3}&yf={4}&from={5}&dt={6}&mt={7}&yt={8}&to={9}&p=1&f={10}&e=.csv&cn={11}&dtf=1&tmf=1&MSOR=1&mstime=on&mstimever=1&sep=3&sep2=2&datf=12&at=1",
        //        "GAZP", "GAZP", DateTime.Now.Day, DateTime.Now.Month - 1, DateTime.Now.Year, DateTime.Now.Date, DateTime.Now.Day, DateTime.Now.Month - 1, DateTime.Now.Year, DateTime.Now.Date, "GAZP", "GAZP");

        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(str);
        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

        //    using (StreamReader stream = new StreamReader(
        //                    resp.GetResponseStream(), Encoding.UTF8))
        //    {
        //        //пишем то что нам пришло в файл
        //        File.WriteAllText(string.Format(path), stream.ReadToEnd());
        //    }
        //    using (StreamReader fs = new StreamReader(path))
        //    {
        //        //const char delimiter = ';';
        //        while (true)
        //        {
        //            string value = fs.ReadLine();
        //            if (value == "<DATE>;<TIME>;<LAST>;<VOL>;<ID>;<OPER>")
        //            { }
        //            else if (!string.IsNullOrEmpty(value))
        //            {
        //                string[] buffer = value.Split(';');
        //                lists.Add(new Item(buffer[0], buffer[1], buffer[2], buffer[3], buffer[4], buffer[5]));
        //            }
        //            else if (string.IsNullOrEmpty(value))
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}
        async void OnChecked(object sender, RoutedEventArgs e)
        {           
            DataGridCell test = (DataGridCell)sender;
            Post post = (Post)test.DataContext;

            TimeSpan ts = TimeSpan.FromMilliseconds(0);

            string[] strs = post.DateSecret.Split(' ');
            string date = string.Format("{0}/{1}/{2} {3}", strs[0], DateConvert(strs[1]), DateTime.Now.Year, post.Time);
            DateTime dt = Convert.ToDateTime(date);

            if (dt.Ticks < DateTime.Now.Ticks)
            {
                await this.ShowMessageAsync("Внимание!!!", "Новость которую выбрали уже прошла...");
                test.IsEnabled = false;
            }
            else
            {
                ts = dt - DateTime.Now.AddMinutes(timerMin);
                UniqTimer uniqTimer = new UniqTimer()
                {
                    post = (Post)test.DataContext,
                    Id = post.Name + post.Time,
                    Timer = new System.Timers.Timer((ts.TotalMilliseconds < 0) ? 1 : ts.TotalMilliseconds)
                };
                //uniqTimer.Timer.Elapsed += (sender, e) => Timer_Elapsed(sender, e, "TestNews");
                //uniqTimer.Timer.Elapsed += (sender, e) => { Timer_Elapsed(sender, e, "Test"); };

                //string theString = ...;
                //timer.Elapsed += (sender, e) => MyElapsedMethod(sender, e, theString);

                uniqTimer.Timer.Elapsed += Timer_Elapsed;
                uniqTimer.Timer.AutoReset = false;
                uniqTimer.Timer.Start();

                listTimers.Add(uniqTimer);
            }

            //MessageBox.Show(post.Name + " добавлен!");
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = sender as System.Timers.Timer;
            Post post = listTimers.Where(a => a.Timer == timer).FirstOrDefault().post;

            Dispatcher.Invoke(() => { new WindowMessage(height, width, post, UserIdTelegram).Show(); });
        }
        void Unchecked(object sender, RoutedEventArgs e)
        {
            DataGridCell test = (DataGridCell)sender;
            Post post = (Post)test.DataContext;
            string tmp = post.Name + post.Time;
            var item = listTimers.Where(p => p.Id == tmp).FirstOrDefault();
            if (item != null)
            {
                item.Timer.Stop();
                listTimers.Remove(item);
            }
            //MessageBox.Show(post.Name + " удалён!");
        }
        private void ContextMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Post post = postsGrid.SelectedItem as Post;
            string[] test = post.DateSecret.Split(' ');
            //"01/08/2008 14:50:50.42";
            string date = string.Format("{0}/{1}/{2} {3}", test[0], DateConvert(test[1]), DateTime.Now.Year, post.Time);
            DateTime dt = Convert.ToDateTime(date);
            if(DateTime.Compare(dt, DateTime.Now) < 0 || DateTime.Compare(dt, DateTime.Now) == 0)
            {
                DownloadFileCotirovki DFC = new DownloadFileCotirovki();
                DFC.post = post;
                DFC.Show();
            }
            else 
            {
                MessageBox.Show("Этой новости ещё не было");
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
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbItem = (ComboBoxItem)cb_Timer.SelectedItem;
            int timerMin = Convert.ToInt32(cbItem.Tag);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var result = this.ShowModalInputExternal("Telegram Notification!", "Введите id Telegram ?");

            //if (result == null) //user pressed cancel
            //    return;

            //this.ShowModalMessageExternal("TelegramBot", "Приветсвуем, " + result + "!");
            //lb_Telegram.Content = result;
            TelegramSettings ts = new TelegramSettings();
            ts.Show();
            ts.Closed += Ts_Closed;
              
        }

        private void Ts_Closed(object sender, EventArgs e)
        {
            UserIdTelegram = ((TelegramSettings)sender).result;
            if(UserIdTelegram != null)
            {
                lb_Telegram.Content = "Hello, " + UserIdTelegram;
                myTel.userID = UserIdTelegram;
                myTel.SendMessage("Вы успешно подписались на нашего бота!!!");
            }    
        }

        private void DataGridRowPreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var row = GetVisualParentByType((FrameworkElement)e.OriginalSource, typeof(DataGridRow)) as DataGridRow;
                if (row != null)
                {
                    row.IsSelected = !row.IsSelected;
                    e.Handled = true;
                }
            }
        }

        public static DependencyObject GetVisualParentByType(DependencyObject startObject, Type type)
        {
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }
    }
    
    
}
using finam.ru_economic_calendar.Class;
using MahApps.Metro.Controls;
using System;
using System.Timers;

namespace finam.ru_economic_calendar
{
    /// <summary>
    /// Логика взаимодействия для WindowMessage.xaml
    /// </summary>
    /// 

    public partial class WindowMessage : MetroWindow
    {
        public static string UserIdTelegram = string.Empty;
        MyTelegram myTel = new MyTelegram();

        public WindowMessage(double height, double width, Post post, string _id)
        {
            InitializeComponent();
            Topmost = true;
            Left = width - 320;
            Top = height - 100;
            tb_text.Text = post.Name;
            UserIdTelegram = _id;

            string[] strs = post.DateSecret.Split(' ');
            string date = string.Format("{0}/{1}/{2} {3}", strs[0], DateConvert(strs[1]), DateTime.Now.Year, post.Time);
            DateTime dt = Convert.ToDateTime(date);
            TimeSpan ts = dt - DateTime.Now;
            if (ts.TotalMinutes < 15)
            {
                string temp = string.Format("через {0} минут", ts.TotalMinutes.ToString("F0"));
                lb_text.Content = temp;
                if(UserIdTelegram != string.Empty)
                {
                    myTel.userID = UserIdTelegram;
                    myTel.SendMessage(post.Name + '\n' + temp);
                }
               
            }
            //var player = new MediaPlayer();
            //player.Open(new Uri("beep9.mp3", UriKind.RelativeOrAbsolute));
            //player.Play();

            Timer timer = new Timer(300000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Close();
            });
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

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using System.Windows;

namespace finam.ru_economic_calendar
{
    /// <summary>
    /// Логика взаимодействия для TelegramSettings.xaml
    /// </summary>
    public partial class TelegramSettings : MetroWindow
    {
        public string result { set; get; } = null;
        public TelegramSettings()
        {
            InitializeComponent();
        }

        private async void AddIdClick(object sender, RoutedEventArgs e)
        {
            if (tb_idTelegram.Text != string.Empty)
            {
                string idUser = tb_idTelegram.Text;
                bool IsDigit = idUser.Length == idUser.Where(c => char.IsDigit(c)).Count();
                if (IsDigit == true)
                {
                    result = idUser;
                    Close();
                }
                else
                {
                    await this.ShowMessageAsync("Ошибка", "idUser имеет недопустимый формат...");
                }
            }
            else
            {
                await this.ShowMessageAsync("Ошибка", "Введите idUser...");
            }
        }
    }
}

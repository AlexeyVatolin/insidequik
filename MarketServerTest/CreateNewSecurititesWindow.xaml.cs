using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using QuikSharp.DataStructures;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для CreateNewSecurititesWindow.xaml
    /// </summary>
    public partial class CreateNewSecurititesWindow : Window
    {
        private ObservableCollection<ClassesAndSecuritiesNode> сlassesAndSecuritites { get; set; }
        bool isUserInteraction;
        public CreateNewSecurititesWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Необходимо вызвать после создания окна. Сделано так как конструктор не может быть асинхронным
        /// </summary>
        public async void Initialize()
        {
            Loading.IsOpen = true;
            сlassesAndSecuritites = await QuikConnector.GetCurrentClassesAndSecuritites();
            SecurititesTree.ItemsSource = сlassesAndSecuritites;
            Loading.IsOpen = false;
        }

        private void ParentCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            ParentCheckbox_CheckedChanged(sender, true);
        }

        private void ParentCheckbox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ParentCheckbox_CheckedChanged(sender, false);
        }

        private void ParentCheckbox_CheckedChanged(object sender, bool value)
        {
            if (isUserInteraction)
            {
                isUserInteraction = false;
                CheckBox checkBox = (CheckBox) sender;
                string checkboxContent = checkBox.Content as string;
                ClassesAndSecuritiesNode currentNode = сlassesAndSecuritites
                    .SingleOrDefault(item => item.ClassInfo.Name == checkboxContent);

                /*var parent = checkBox.GetBindingExpression(CheckBox.IsCheckedProperty);
                var t = parent.ResolvedSource;    Как-то так можно получить объект биндинга, 
                но подробности этой магии неизвестны*/

                if (currentNode != null)
                {
                    currentNode.IsChecked = value;
                    currentNode.ChangeChildernIsChecked(value);
                }
            }
        }

        private void ChildCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            ChildCheckbox_CheckedChanged(sender, true);
        }

        private void ChildCheckbox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ChildCheckbox_CheckedChanged(sender, false);
        }

        private void ChildCheckbox_CheckedChanged(object sender, bool value)
        {
            if (isUserInteraction)
            {
                isUserInteraction = false;
                CheckBox checkBox = (CheckBox) sender;
                string checkboxContent = checkBox.Content as string;

                SecurityInfoRow currentNode = сlassesAndSecuritites.SelectMany(item => item.SecurityInfos)
                    .SingleOrDefault(i => i.SecurityInfo.Name == checkboxContent);
                //currentNode.IsChecked = value;
                currentNode?.ChangeParentIsChecked(value);
            }
        }

        /*Функция нужна чтобы реагировать только на пользовательские нажатия. По дефолту OnChecked/OnUnchecked 
         * реагирует на все */
        private void ChildCheckbox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isUserInteraction = true;
        }

        private void ParentCheckbox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isUserInteraction = true;
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            List<SecurityInfo> securities = сlassesAndSecuritites.SelectMany(item => item.SecurityInfos)
                .Where(i => i.IsChecked).Select(info => info.SecurityInfo).ToList();
            var securitiesWindow = new Securities(securities);
            this.Close();
            securitiesWindow.ShowDialog();
        }
    }
}

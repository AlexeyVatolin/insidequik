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
    }
}

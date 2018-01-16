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

namespace finam.ru_economic_calendar
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ObservableCollection<Test> Collection { get; set; }
        public Window1()
        {
            InitializeComponent();
            Collection = new ObservableCollection<Test>();
            Collection.Add(new Test() {Field = "123"});
            Collection.Add(new Test() {Field = "123", IsSelected = true});
            Collection.Add(new Test() {Field = "123"});
            Collection.Add(new Test() {Field = "123"});
            Collection.Add(new Test() {Field = "123"});
            Collection.Add(new Test() {Field = "123"});
            Grid.ItemsSource = Collection;
        }
    }

    public class Test
    {
        public string Field { get; set; }
        public bool IsSelected { get; set; }
    }
}

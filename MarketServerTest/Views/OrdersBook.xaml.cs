using MahApps.Metro.Controls;
using MarketServerTest.ViewModels;

namespace MarketServerTest
{
    /// <summary>
    /// Логика взаимодействия для OrdersBook.xaml
    /// </summary>
    public partial class OrdersBook : MetroWindow
    {
        public OrdersBook(string ticker)
        {
            InitializeComponent();
            OrderBookViewModel viewModel = new OrderBookViewModel(ticker);
            viewModel.ShowSendOrderWindow += ShowSendOrderWindow;
            viewModel.ShowSendStopOrderWindow += ShowSendStopOrderWindow;
            DataContext = viewModel;
        }

        private void ShowSendOrderWindow(string ticker, double price)
        {
            SendBid sendOrder = new SendBid(ticker, price);
            sendOrder.Show();
        }

        private void ShowSendStopOrderWindow(string ticker, double price)
        {
            SendStopOrder sendStopOrder = new SendStopOrder(ticker);
            sendStopOrder.Show();
        }
    }
}

using System.Collections.ObjectModel;
using MarketServerTest.Helpers;
using MarketServerTest.Models;
using MarketServerTest.SecurityTables;
using MarketServerTest.Views;

namespace MarketServerTest.ViewModels
{
    public class TradeTableViewModel
    {
        public ObservableCollection<TradeTableModel> TradeTables { get; set; }

        public TradeTableViewModel()
        {
            TradeTables = new ObservableCollection<TradeTableModel>(
                SecurityTablesRepository.GetSecuritiesTablesInfos())
            {
                null, //Значние null добавляет разделитель меню
                new TradeTableModel()
                {
                    Name = "Управление таблицами",
                    Command = new RelayCommand((obj) =>
                    {
                        new SecuritiesTablesEdit().Show();
                    })
                }
            };
        }
    }
}

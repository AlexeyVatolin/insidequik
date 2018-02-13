using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MarketServerTest.Helpers;
using MarketServerTest.Models;
using MarketServerTest.SecurityTables;

namespace MarketServerTest.ViewModels
{
    public class SecuritiesTablesEditViewModel 
    {
        public ObservableCollection<TradeTableModel> TradeTables { get; set; }

        public ICommand Delete
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    string id = ((TradeTableModel) obj).Id;
                    TradeTables.Remove(TradeTables.FirstOrDefault(item => item.Id == id));
                    SecurityTablesRepository.DeleteById(id);
                });
            }
        }

        public SecuritiesTablesEditViewModel()
        {
            TradeTables = new ObservableCollection<TradeTableModel>(
                SecurityTablesRepository.GetSecuritiesTablesInfos());
        }
    }
}

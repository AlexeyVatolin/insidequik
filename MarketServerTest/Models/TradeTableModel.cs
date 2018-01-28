using System.Windows.Input;
using MarketServerTest.Helpers;
using MarketServerTest.SecurityTables;

namespace MarketServerTest.Models
{
    public class TradeTableModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICommand Command
        {
            get
            {
                if (_command != null)
                {
                    return _command;
                }
                return new RelayCommand((obj) =>
                {
                    new Securities(SecurityTablesRepository.GetSecuritiesById(Id)).Show();
                });
            }
            set => _command = value;
        }

        private ICommand _command;
    }
}

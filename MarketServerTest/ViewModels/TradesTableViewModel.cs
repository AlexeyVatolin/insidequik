using System.Windows.Input;
using MarketServerTest.SecurityTables;

namespace MarketServerTest.ViewModels
{
    class TradesTableViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /*public ICommand OpenSecuryTable
        {
            get
            {
               // return new Command();
            }
        }*/
        public OnClick Command { get; set; }

        public delegate void OnClick(string id);

        public TradesTableViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

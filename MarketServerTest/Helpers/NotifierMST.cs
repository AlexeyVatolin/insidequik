using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MarketServerTest.Helpers
{
    public class NotifierMST : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

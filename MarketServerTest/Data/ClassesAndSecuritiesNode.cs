using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MarketServerTest.Annotations;
using QuikSharp.DataStructures;

namespace MarketServerTest.Data
{
    public class ClassesAndSecuritiesNode : INotifyPropertyChanged
    {
        public ClassInfo ClassInfo { get; set; }
        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public void ChangeChildernIsChecked(bool value)
        {
            foreach (var row in SecurityInfos)
            {
                row.IsChecked = value;
            }
        }

        public ObservableCollection<SecurityInfoRow> SecurityInfos { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SecurityInfoRow : INotifyPropertyChanged
    {
        public SecurityInfo SecurityInfo { get; set; }
        private bool _isChecked;
        public ClassesAndSecuritiesNode Parent;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public void ChangeParentIsChecked(bool value)
        {
            if (!value && Parent.IsChecked)
            {
                Parent.IsChecked = false;
            }
            else if (value)
            {
                bool isAllChildsSelected = true;
                foreach (var child in Parent.SecurityInfos)
                {
                    if (!child.IsChecked)
                        isAllChildsSelected = false;
                    break;
                }
                if (isAllChildsSelected)
                {
                    Parent.IsChecked = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

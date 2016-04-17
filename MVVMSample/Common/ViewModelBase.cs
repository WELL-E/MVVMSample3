using System.ComponentModel;

namespace MVVMSample.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            var hander = PropertyChanged;
            hander.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
  
    }
}

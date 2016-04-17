using MVVMSample.Common;

namespace MVVMSample.ViewModels
{
    public class RightViewModel : ViewModelBase
    {
        public RightViewModel()
        {
            var vm = ViewModelManager.GetByKey("left") as LeftViewModel;
            if (vm != null) vm.NumberChanged += VmOnNumberChanged;
        }


        private int _number;

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VmOnNumberChanged(object sender, NumberChangedEventArgs e)
        {
            Number = e.Number + 1;
        }
    }
}

using System;
using System.Collections.Generic;
using MVVMSample.Common;

namespace MVVMSample.ViewModels
{
    public class LeftViewModel : ViewModelBase
    {

        public LeftViewModel()
        {

        }

        public List<int> SourceCount
        {
            get
            {
                return new List<int>
                {
                    1,
                    2,
                    3,
                    4
                };
            }
        }

        /// <summary>
        /// 选择命令
        /// </summary>
        private DelegateCommand<ExCommandParameter> _selectionChangedCmd;

        public DelegateCommand<ExCommandParameter> SelectionChangedCmd
        {
            get
            {
                if (_selectionChangedCmd == null)
                {
                    _selectionChangedCmd = new DelegateCommand<ExCommandParameter>(InvokeMouseDown);
                }

                return _selectionChangedCmd;
            }
        }

        private void InvokeMouseDown(ExCommandParameter param)
        {
            var number = param.Parameter is int ? (int) param.Parameter : 0;
            OnNumberChanged(new NumberChangedEventArgs(number));
        }

        public event EventHandler<NumberChangedEventArgs> NumberChanged;

        protected virtual void OnNumberChanged(NumberChangedEventArgs e)
        {
            var handler = NumberChanged;
            if (handler != null) handler(this, e);
        }

      
    }

}

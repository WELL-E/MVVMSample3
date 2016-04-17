using System.Windows.Controls;
using MVVMSample.Common;
using MVVMSample.ViewModels;

namespace MVVMSample.Views
{
    /// <summary>
    /// RightView.xaml 的交互逻辑
    /// </summary>
    public partial class RightView : UserControl
    {
        public RightView()
        {
            InitializeComponent();

            var vm = new RightViewModel();
            ViewModelManager.Add("right", vm);
            this.DataContext = vm;
        }
    }
}

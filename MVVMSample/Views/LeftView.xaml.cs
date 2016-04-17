using System.Windows.Controls;
using MVVMSample.Common;
using MVVMSample.ViewModels;

namespace MVVMSample.Views
{
    /// <summary>
    /// LeftView.xaml 的交互逻辑
    /// </summary>
    public partial class LeftView : UserControl
    {
        public LeftView()
        {
            InitializeComponent();

            var vm = new LeftViewModel();
            ViewModelManager.Add("left", vm);
            this.DataContext = vm;
        }
    }
}

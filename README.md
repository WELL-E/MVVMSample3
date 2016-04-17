有种想写一个MVVM框架的冲动！！！
---

**1、Model中的属性应不应该支持OnPropertyChanged事件？**

不应该。应该有ViewModel对该属性进行封装，由ViewModel提供OnPropertyChanged事件。
WPF之MVVM（1）中有实例

**2、如何将控件事件转换为命令？**
	
- 在“扩展”中添加“System.Windows.Interractivity”引用
- xaml中添加`xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity`命名空间
- 使用

		<ListBox Name="LbBox" ItemsSource="{Binding Path=SourceCount}">
		    <i:Interaction.Triggers>
		        <i:EventTrigger EventName="SelectionChanged">
		            <common:ExInvokeCommandAction Command="{Binding Path=SelectionChangedCmd}" CommandParameter="{Binding ElementName=LbBox, Path=SelectedItem}"/>
		        </i:EventTrigger>
		    </i:Interaction.Triggers>
		</ListBox>
	

**3、View中如何访问ViewModel**

WPF之MVVM（2）中介绍了

**4、ViewModel中如何访问View**

WPF之MVVM（2）中介绍了

**5、ViewModel之间通信**

- 聚合关系

		public class VM01
		{
			public string Name{get;set;}
		}
		
		public class VM02
		{
			public list<VM01> Property
			{
				get;
				set;
			}
		}
		
- 组合关系

		public class VM01
		{
			public string Name{get;set;}
		}
		
		public class VM02
		{
			public string Name{get;set;}
		}
		
		public class VM03
		{
			private VM01 _vm01;
			private VM02 _vm02;
			...
			public VM03(VM01 vm01, VM02 vm02){}
		}

- 依赖关系

这里主要介绍下依赖关系的ViewModel如何通信

![](http://i.imgur.com/gwkq3Wn.png)

通过一个非常简单的程序来演示这种实现：点击左边的数字，右边的数字加1。

左边为`LeftViewModel`右边为`RightViewModel`,两个VM是相互独立的，通过事件进行通信。


1、定义类型来容纳所有需要发送给事件通知接收者的附件信息

	public class NumberChangedEventArgs : EventArgs
	{
	    public int Number { get; set; }
	
	    public NumberChangedEventArgs(int num)
	    {
	        Number = num;
	    }
	}

2、在`LeftViewModel`中定义事件成员
	
	public event EventHandler<NumberChangedEventArgs> NumberChanged;

    protected virtual void OnNumberChanged(NumberChangedEventArgs e)
    {
        var handler = NumberChanged;
        if (handler != null) handler(this, e);
    }
    
3、定义负责引发事件的方法来通知事件的登记对象
	
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
	    //触发事件
	    OnNumberChanged(new NumberChangedEventArgs(number));
	}

4、定义方法将输入转化为期望事件

	public class RightViewModel : ViewModelBase
	{
		public RightViewModel()
		{
			//订阅事件
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
	
**问题**：`RightViewModel`中如何获取`LeftViewModel`呢？

定义一个容器

	public static class ViewModelManager
	{
	    private static readonly Dictionary<string, ViewModelBase> Dic = new Dictionary<string, ViewModelBase>();
	
	    public static void Add(string key, ViewModelBase value)
	    {
	        if (Dic.ContainsKey(key)) return;
	
	        Dic.Add(key, value);
	    }
	
	    public static ViewModelBase GetByKey(string key)
	    {
	        if (!Dic.ContainsKey(key)) return null;
	
	        ViewModelBase value;
	        Dic.TryGetValue(key, out value);
	
	        return value;
	    }
	}
	
在设置View的`DataContext`时将ViewModel添加到`ViewModelManager`中

	public LeftView()
	{
	    InitializeComponent();
	
	    var vm = new LeftViewModel();
	    ViewModelManager.Add("left", vm);
	    this.DataContext = vm;
	}
	
## 总结
回顾上面3篇博文中解决的问题，我们再来看下MvvmLight ToolKit是如何实现MVVM的，这里引用下园友的总结[MvvmLight ToolKit 教程](http://www.cnblogs.com/HelloMyWorld/p/4750070.html)。

我们可以猜测MvvmLight作者使用这些组件是为了解决什么问题？

- ViewModelBase && ObservableObject（INotifyPropertyChanged接口的实现，解决属性改变通知的问题）
- ViewModelLocator && SimpleIoc（IOC容器，我们的`ViewModelManager`高级版）
- RelayCommand（ICommand接口的实现，解决View和ViewModel通信问题）
- EventToCommand && IEventArgsConverter（Interaction的封装，解决将事件转换为命令的问题）
- Messenger（解决View和ViewModel以及ViewModel和ViewModel之间通信的问题）
- DispatcherHelper（博客中未提到）

这样分析后，我们就知道MvvmLight是如何产生的，以及它能为我们做什么。

吹牛吹到现在，我都有种自己想写一个MVVM框架的冲动了。你们有没有？
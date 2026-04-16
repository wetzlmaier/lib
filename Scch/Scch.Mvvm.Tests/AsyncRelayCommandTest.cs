using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.Mvvm.Command;
using Scch.Mvvm.ViewModel;

namespace Scch.Mvvm.Tests
{
    [TestClass]
    public class AsyncRelayCommandTest
    {
        private MainViewModel _viewModel;
        private SynchronizationContext _context;
        private string _property;
        private ManualResetEvent _sync;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new MainViewModel();

            _context = new SynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(_context);

            _property = null;
            _sync = new ManualResetEvent(false);

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (_property == e.PropertyName)
                    _sync.Set();
            };
        }

        [TestMethod]
        public void TestLazyProperty()
        {
            _property = "Items2";
            var items2 = _viewModel.Items2;
            _sync.WaitOne();

            Assert.AreEqual(30, _viewModel.Items2.Count);
            _sync.Reset();

            _property = "SelectedItem2";
            var selectedItem2 = _viewModel.SelectedItem2;
            if (selectedItem2 == null)
                _sync.WaitOne();

            Assert.AreEqual("5", _viewModel.SelectedItem2);
        }

        [TestMethod]
        public void TestExecute()
        {
            Assert.AreEqual(true, _viewModel.Update.CanExecute());
            _viewModel.Update.Execute();
            Assert.AreEqual(false, _viewModel.Update.CanExecute());

            _property = "Items";
            _sync.WaitOne();
            Assert.AreEqual(true, _viewModel.Update.CanExecute());
        }

        [TestMethod]
        public void TestCancel()
        {
            _viewModel.Update.Execute();
            Thread.Sleep(100);
            _viewModel.Update.Cancel();
            Thread.Sleep(500);
            Assert.IsTrue(_viewModel.Update.IsCancellationRequested);

            _property = "Items";
            _sync.WaitOne();
            Assert.IsTrue(_viewModel.Completed);
            Assert.IsTrue(_viewModel.Cancelled);
            Assert.AreEqual(0, _viewModel.Items.Count);
        }

        private class MainViewModel : ViewModelBase
        {
            public MainViewModel() : base("Test")
            {
                Update = new AsyncRelayCommand<ObservableCollection<string>>(Execute, CanExecute);
                Update.ExecuteCompleted += ExecuteCompleted;
            }

            private ObservableCollection<string> Execute(CancellationToken token)
            {
                var items = CreateItems();

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                return items;
            }

            private bool CanExecute()
            {
                return true;
            }

            public AsyncRelayCommand<ObservableCollection<string>> Update { get; private set; }

            private ObservableCollection<string> CreateItems()
            {
                var items = new ObservableCollection<string>();

                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(100);
                    items.Add(i.ToString());
                }

                Count++;
                return items;
            }

            public bool Cancelled { get; private set; }

            public bool Completed { get; private set; }

            private void ExecuteCompleted(object sender, AsyncCompletedEventArgs e)
            {
                Completed = true;
                Cancelled = e.Cancelled;

                Items = (e.Cancelled ? new ObservableCollection<string>() : Update.Result);
            }

            private ObservableCollection<string> _items;

            public ObservableCollection<string> Items
            {
                get
                {
                    return _items;
                }
                private set
                {
                    _items = value;
                    RaisePropertyChanged(() => Items);
                }
            }

            private ObservableCollection<string> _items1;

            public ObservableCollection<string> Items1
            {
                get
                {
                    if (_items1 == null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Items1 = CreateItems();
                            SelectedItem1 = (from i in Items1 where i == "10" select i).Single();
                        });
                    }
                    return _items1;
                }
                private set
                {
                    _items1 = value;
                    RaisePropertyChanged(() => Items1);
                }
            }

            private ObservableCollection<string> _items2;

            public ObservableCollection<string> Items2
            {
                get
                {
                    if (_items2 == null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Items2 = CreateItems();
                            SelectedItem2 = (from i in Items2 where i == "5" select i).Single();
                        });
                    }
                    return _items2;
                }
                private set
                {
                    _items2 = value;
                    RaisePropertyChanged(() => Items2);
                }
            }

            private int _count;
            public int Count
            {
                get
                {
                    return _count;
                }
                private set
                {
                    _count = value;
                    RaisePropertyChanged(() => Count);
                }
            }

            private string _selectedItem1;

            public string SelectedItem1
            {
                get
                {
                    return _selectedItem1;
                }
                set
                {
                    _selectedItem1 = value;
                    RaisePropertyChanged(() => SelectedItem1);
                }
            }

            private string _selectedItem2;

            public string SelectedItem2
            {
                get
                {
                    return _selectedItem2;
                }
                set
                {
                    _selectedItem2 = value;
                    RaisePropertyChanged(() => SelectedItem2);
                }
            }
        }

    }
}

using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using Scch.Mvvm.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Controls.Tests
{
    /// <summary>
    /// Summary description for CollectionEditorTest
    /// </summary>
    [TestClass]
    public class CollectionEditorTest
    {
        public CollectionEditorTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
            _viewModel = new CollectionEditorViewModel();
            _viewModel.AvailablePersons.Add(new Person { Name = "Person1", Age = 1 });
            _viewModel.AvailablePersons.Add(new Person { Name = "Person2", Age = 2 });
            _viewModel.AvailablePersons.Add(new Person { Name = "Person3", Age = 3 });

            _editor = new CollectionEditor();
            _lvAvailableItems = (SortableListView)_editor.FindName("lvAvailableItems");
            _lvSelectedItems = (SortableListView)_editor.FindName("lvSelectedItems");
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private CollectionEditorViewModel _viewModel;
        private CollectionEditor _editor;
        private SortableListView _lvAvailableItems;
        private SortableListView _lvSelectedItems;

        [TestMethod]
        public void TestBindingOrder1()
        {
            _viewModel.SelectedPersons.Add(_viewModel.AvailablePersons[0]);

            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });

            Assert.AreSame(_viewModel.AvailablePersons[1], _lvAvailableItems.Items[0]);
            Assert.AreSame(_viewModel.AvailablePersons[2], _lvAvailableItems.Items[1]);

            Assert.AreSame(_viewModel.AvailablePersons[0], _lvSelectedItems.Items[0]);
        }

        [TestMethod]
        public void TestBindingOrder2()
        {
            _viewModel.SelectedPersons.Add(_viewModel.AvailablePersons[0]);

            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            Assert.AreSame(_viewModel.AvailablePersons[1], _lvAvailableItems.Items[0]);
            Assert.AreSame(_viewModel.AvailablePersons[2], _lvAvailableItems.Items[1]);

            Assert.AreSame(_viewModel.AvailablePersons[0], _lvSelectedItems.Items[0]);
        }

        [TestMethod]
        public void TestBindingAddAvailableItem()
        {
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            var person = new Person { Name = "New Person", Age = 5 };
            _viewModel.AvailablePersons.Add(person);

            Assert.AreSame(person, _lvAvailableItems.Items[_lvAvailableItems.Items.Count - 1]);
            Assert.AreEqual(4, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestBindingRemoveAvailableItem()
        {
            var person = new Person { Name = "New Person", Age = 5 };
            _viewModel.AvailablePersons.Add(person);

            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            _viewModel.AvailablePersons.Remove(person);

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestBindingAddSelectedItem()
        {
            var person = new Person { Name = "New Person", Age = 5 };
            _viewModel.AvailablePersons.Add(person);

            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            _viewModel.SelectedPersons.Add(person);

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(1, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestBindingRemoveSelectedItem()
        {
            var person = new Person { Name = "New Person", Age = 5 };
            _viewModel.AvailablePersons.Add(person);
            _viewModel.SelectedPersons.Add(person);

            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            _viewModel.SelectedPersons.Remove(person);

            Assert.IsTrue(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(4, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestBindingAdd()
        {
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            var person = _viewModel.AvailablePersons[0];
            _lvAvailableItems.SelectedItem = person;
            _editor.Add();

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(2, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_viewModel.AvailablePersons.Contains(person));
            Assert.AreEqual(3, _viewModel.AvailablePersons.Count);
            Assert.IsTrue(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(1, _lvSelectedItems.Items.Count);
            Assert.IsTrue(_viewModel.SelectedPersons.Contains(person));
            Assert.AreEqual(1, _viewModel.SelectedPersons.Count);
        }

        [TestMethod]
        public void TestBindingAddAll()
        {
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            var person = _viewModel.AvailablePersons[0];
            _lvAvailableItems.SelectedItem = person;
            _editor.AddAll();

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(0, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_viewModel.AvailablePersons.Contains(person));
            Assert.AreEqual(3, _viewModel.AvailablePersons.Count);
            Assert.IsTrue(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(3, _lvSelectedItems.Items.Count);
            Assert.IsTrue(_viewModel.SelectedPersons.Contains(person));
            Assert.AreEqual(3, _viewModel.SelectedPersons.Count);
        }

        [TestMethod]
        public void TestBindingRemoveAll()
        {
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            var person = _viewModel.AvailablePersons[0];
            _viewModel.SelectedPersons.Add(person);
            _viewModel.SelectedPersons.Add(_viewModel.AvailablePersons[1]);
            _lvSelectedItems.SelectedItem = person;
            _editor.RemoveAll();

            Assert.IsTrue(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_viewModel.AvailablePersons.Contains(person));
            Assert.AreEqual(3, _viewModel.AvailablePersons.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
            Assert.IsFalse(_viewModel.SelectedPersons.Contains(person));
            Assert.AreEqual(0, _viewModel.SelectedPersons.Count);
        }

        [TestMethod]
        public void TestBindingRemove()
        {
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            var person = _viewModel.AvailablePersons[0];
            _viewModel.SelectedPersons.Add(person);
            _lvSelectedItems.SelectedItem = person;
            _editor.Remove();

            Assert.IsTrue(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_viewModel.AvailablePersons.Contains(person));
            Assert.AreEqual(3, _viewModel.AvailablePersons.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
            Assert.IsFalse(_viewModel.SelectedPersons.Contains(person));
            Assert.AreEqual(0, _viewModel.SelectedPersons.Count);
        }

        [TestMethod]
        public void TestBindingColumns()
        {
            _editor.SetBinding(CollectionEditor.ColumnsSourceProperty, new Binding("Columns") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            var gvAvailableItems = (GridView)_lvAvailableItems.View;
            Assert.AreEqual(_viewModel.Columns.Count, gvAvailableItems.Columns.Count);
            Assert.AreEqual(_viewModel.Columns[0].HeaderText, gvAvailableItems.Columns[0].Header);
            Assert.AreEqual(_viewModel.Columns[0].DisplayMember, ((Binding)gvAvailableItems.Columns[0].DisplayMemberBinding).Path.Path);
            Assert.AreEqual(_viewModel.Columns[1].HeaderText, gvAvailableItems.Columns[1].Header);
            Assert.AreEqual(_viewModel.Columns[1].DisplayMember, ((Binding)gvAvailableItems.Columns[1].DisplayMemberBinding).Path.Path);

            var gvSelectedItems = (GridView)_lvSelectedItems.View;
            Assert.AreEqual(_viewModel.Columns.Count, gvSelectedItems.Columns.Count);
            Assert.AreEqual(_viewModel.Columns[0].HeaderText, gvSelectedItems.Columns[0].Header);
            Assert.AreEqual(_viewModel.Columns[0].DisplayMember, ((Binding)gvSelectedItems.Columns[0].DisplayMemberBinding).Path.Path);
            Assert.AreEqual(_viewModel.Columns[1].HeaderText, gvSelectedItems.Columns[1].Header);
            Assert.AreEqual(_viewModel.Columns[1].DisplayMember, ((Binding)gvSelectedItems.Columns[1].DisplayMemberBinding).Path.Path);
        }

        [TestMethod]
        public void TestBindingSelectedItem()
        {
            _editor.SetBinding(CollectionEditor.AvailableItemsSourceProperty, new Binding("AvailablePersons") { Source = _viewModel });
            _editor.SetBinding(CollectionEditor.SelectedItemsSourceProperty, new Binding("SelectedPersons") { Source = _viewModel });

            _viewModel.SelectedPersons.Add(_viewModel.AvailablePersons[0]);
            _editor.SetBinding(CollectionEditor.AvailableItemsSelectedItemProperty, new Binding("SelectedAvailablePerson") { Source = _viewModel, Mode = BindingMode.TwoWay});
            _editor.SetBinding(CollectionEditor.SelectedItemsSelectedItemProperty, new Binding("SelectedSelectedPerson") { Source = _viewModel, Mode = BindingMode.TwoWay });

            _lvAvailableItems.SelectedItem = _viewModel.AvailablePersons[1];
            Assert.AreSame(_lvAvailableItems.SelectedItem, _viewModel.SelectedAvailablePerson);

            _lvSelectedItems.SelectedItem=_viewModel.SelectedPersons[0];
            Assert.AreSame(_lvSelectedItems.SelectedItem, _viewModel.SelectedSelectedPerson);
        }

        [TestMethod]
        public void TestAddAvailableItem()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            var person = new Person { Name = "New Person", Age = 5 };
            _editor.AvailableItems.Add(person);

            Assert.AreSame(person, _lvAvailableItems.Items[_lvAvailableItems.Items.Count - 1]);
            Assert.AreEqual(4, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestRemoveAvailableItem()
        {
            var person = new Person { Name = "New Person", Age = 5 };
            _editor.AvailableItems.Add(person);

            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            _editor.AvailableItems.Remove(person);

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestAddSelectedItem()
        {
            var person = new Person { Name = "New Person", Age = 5 };
            _editor.AvailableItems.Add(person);

            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            _editor.SelectedItems.Add(person);

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(1, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestRemoveSelectedItem()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            var person = new Person { Name = "New Person", Age = 5 };
            _editor.AvailableItems.Add(person);
            _editor.SelectedItems.Add(person);

            _editor.SelectedItems.Remove(person);

            Assert.IsTrue(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(4, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestAdd()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            var person = _viewModel.AvailablePersons[0];
            _lvAvailableItems.SelectedItem = person;
            _editor.Add();

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(2, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(1, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestAddAll()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            var person = _editor.AvailableItems.GetItemAt(0);
            _lvAvailableItems.SelectedItem = person;
            _editor.AddAll();

            Assert.IsFalse(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(0, _lvAvailableItems.Items.Count);
            Assert.IsTrue(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(3, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestRemoveAll()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            var person = _viewModel.AvailablePersons[0];
            _editor.SelectedItems.Add(person);
            _editor.SelectedItems.Add(_editor.AvailableItems.GetItemAt(1));
            _lvSelectedItems.SelectedItem = person;
            _editor.RemoveAll();

            Assert.IsTrue(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestRemove()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            var person = _viewModel.AvailablePersons[0];
            _editor.SelectedItems.Add(person);
            _lvSelectedItems.SelectedItem = person;
            _editor.Remove();

            Assert.IsTrue(_lvAvailableItems.Items.Contains(person));
            Assert.AreEqual(3, _lvAvailableItems.Items.Count);
            Assert.IsFalse(_lvSelectedItems.Items.Contains(person));
            Assert.AreEqual(0, _lvSelectedItems.Items.Count);
        }

        [TestMethod]
        public void TestColumns()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            _editor.ColumnsSource=_viewModel.Columns;

            var gvAvailableItems = (GridView)_lvAvailableItems.View;
            Assert.AreEqual(_viewModel.Columns.Count, gvAvailableItems.Columns.Count);
            Assert.AreEqual(_viewModel.Columns[0].HeaderText, gvAvailableItems.Columns[0].Header);
            Assert.AreEqual(_viewModel.Columns[0].DisplayMember, ((Binding)gvAvailableItems.Columns[0].DisplayMemberBinding).Path.Path);
            Assert.AreEqual(_viewModel.Columns[1].HeaderText, gvAvailableItems.Columns[1].Header);
            Assert.AreEqual(_viewModel.Columns[1].DisplayMember, ((Binding)gvAvailableItems.Columns[1].DisplayMemberBinding).Path.Path);

            var gvSelectedItems = (GridView)_lvSelectedItems.View;
            Assert.AreEqual(_viewModel.Columns.Count, gvSelectedItems.Columns.Count);
            Assert.AreEqual(_viewModel.Columns[0].HeaderText, gvSelectedItems.Columns[0].Header);
            Assert.AreEqual(_viewModel.Columns[0].DisplayMember, ((Binding)gvSelectedItems.Columns[0].DisplayMemberBinding).Path.Path);
            Assert.AreEqual(_viewModel.Columns[1].HeaderText, gvSelectedItems.Columns[1].Header);
            Assert.AreEqual(_viewModel.Columns[1].DisplayMember, ((Binding)gvSelectedItems.Columns[1].DisplayMemberBinding).Path.Path);
        }

        [TestMethod]
        public void TestSelectedItem()
        {
            foreach (var item in _viewModel.AvailablePersons)
                _editor.AvailableItems.Add(item);

            _viewModel.SelectedPersons.Add(_viewModel.AvailablePersons[0]);
            _editor.SelectedItems.Add(_viewModel.AvailablePersons[0]);

            _lvAvailableItems.SelectedItem = _viewModel.AvailablePersons[1];
            Assert.AreSame(_lvAvailableItems.SelectedItem, _editor.AvailableItemsSelectedItem);

            _lvSelectedItems.SelectedItem = _viewModel.SelectedPersons[0];
            Assert.AreSame(_lvSelectedItems.SelectedItem, _editor.SelectedItemsSelectedItem);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class CollectionEditorViewModel : ViewModelBase
    {
        public CollectionEditorViewModel()
            : base("")
        {
            AvailablePersons = new ObservableCollection<Person>();
            SelectedPersons = new ObservableCollection<Person>();
            Columns = new ObservableCollection<LocalizedColumnDescriptor>
                          {
                              new LocalizedColumnDescriptor<Person>(x => x.Name),
                              new LocalizedColumnDescriptor<Person>(x => x.Age)
                          };
        }

        public ObservableCollection<Person> AvailablePersons { get; set; }

        public Person SelectedAvailablePerson { get; set; }

        public ObservableCollection<Person> SelectedPersons { get; set; }

        public Person SelectedSelectedPerson { get; set; }

        public ObservableCollection<LocalizedColumnDescriptor> Columns { get; set; }
    }
}

using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using Scch.Mvvm.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Controls.Tests
{
    /// <summary>
    /// Summary description for ListBoxSorterTest
    /// </summary>
    [TestClass]
    public class ListBoxSorterTest
    {
        public ListBoxSorterTest()
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
            _viewModel = new ListBoxSorterViewModel();
            _viewModel.Persons.Add(new Person { Name = "Person1", Age = 1 });
            _viewModel.Persons.Add(new Person { Name = "Person2", Age = 2 });
            _viewModel.Persons.Add(new Person { Name = "Person3", Age = 3 });

            _listBox=new ListBox();
            _sorter = new ListBoxSorter {Target = _listBox};
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private ListBoxSorterViewModel _viewModel;
        private ListBoxSorter _sorter;
        private ListBox _listBox;

        [TestMethod]
        public void TestBindingMoveDown()
        {
            _listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Persons") { Source = _viewModel });

            _listBox.SelectedIndex = 1;
            var person0 = _viewModel.Persons[0];
            var person1 = _viewModel.Persons[1];
            var person2 = _viewModel.Persons[2];
            _sorter.MoveDown();
            Assert.AreEqual(2, _listBox.SelectedIndex);

            Assert.AreEqual(3, _listBox.Items.Count);
            Assert.AreSame(person0, _listBox.Items[0]);
            Assert.AreSame(person1, _listBox.Items[2]);
            Assert.AreSame(person2, _listBox.Items[1]);

            Assert.AreSame(person0, _viewModel.Persons[0]);
            Assert.AreSame(person1, _viewModel.Persons[2]);
            Assert.AreSame(person2, _viewModel.Persons[1]);
        }

        [TestMethod]
        public void TestBindingMoveUp()
        {
            _listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Persons") { Source = _viewModel });

            _listBox.SelectedIndex = 1;
            var person0 = _viewModel.Persons[0];
            var person1 = _viewModel.Persons[1];
            var person2 = _viewModel.Persons[2];
            _sorter.MoveUp();
            Assert.AreEqual(0, _listBox.SelectedIndex);

            Assert.AreEqual(3, _listBox.Items.Count);
            Assert.AreSame(person0, _listBox.Items[1]);
            Assert.AreSame(person1, _listBox.Items[0]);
            Assert.AreSame(person2, _listBox.Items[2]);

            Assert.AreSame(person0, _viewModel.Persons[1]);
            Assert.AreSame(person1, _viewModel.Persons[0]);
            Assert.AreSame(person2, _viewModel.Persons[2]);
        }

        [TestMethod]
        public void TestMoveUp()
        {
            foreach (var item in _viewModel.Persons)
                _listBox.Items.Add(item);

            _listBox.SelectedIndex = 1;
            var person0 = _listBox.Items[0];
            var person1 = _listBox.Items[1];
            var person2 = _listBox.Items[2];
            _sorter.MoveUp();
            Assert.AreEqual(0, _listBox.SelectedIndex);

            Assert.AreEqual(3, _listBox.Items.Count);
            Assert.AreSame(person0, _listBox.Items[1]);
            Assert.AreSame(person1, _listBox.Items[0]);
            Assert.AreSame(person2, _listBox.Items[2]);
        }

        [TestMethod]
        public void TestMoveDown()
        {
            foreach (var item in _viewModel.Persons)
                _listBox.Items.Add(item);

            _listBox.SelectedIndex = 1;
            var person0 = _listBox.Items[0];
            var person1 = _listBox.Items[1];
            var person2 = _listBox.Items[2];
            _sorter.MoveDown();
            Assert.AreEqual(2, _listBox.SelectedIndex);

            Assert.AreEqual(3, _listBox.Items.Count);
            Assert.AreSame(person0, _listBox.Items[0]);
            Assert.AreSame(person1, _listBox.Items[2]);
            Assert.AreSame(person2, _listBox.Items[1]);
        }

        public class ListBoxSorterViewModel : ViewModelBase
        {
            public ListBoxSorterViewModel()
                : base("")
            {
                Persons = new ObservableCollection<Person>();
            }

            public ObservableCollection<Person> Persons { get; set; }

            public Person SelectedPerson { get; set; }
        }
    }
}

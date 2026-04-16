using System;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.DomainModel.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for TrackableCollection
    /// </summary>
    [TestClass]
    public class TrackableCollectionTest
    {
        public TrackableCollectionTest()
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
            _removeCount = 0;
            _addCount = 0;
            _collection = new TrackableCollection<long, Person>();
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private TrackableCollection<long, Person> _collection;

        [TestMethod]
        public void TestSortEmpty()
        {
            _collection.Sort();
            Assert.AreEqual(0, _collection.Count);
        }

        [TestMethod]
        public void TestSortOne()
        {
            var p = new Person { Name = "Person1" };
            _collection.Add(p);
            _collection.Sort();
            Assert.AreEqual(1, _collection.Count);
            Assert.AreSame(p, _collection[0]);
        }

        [TestMethod]
        public void TestSortMany()
        {
            var p1 = new Person { Name = "Person1" };
            var p2 = new Person { Name = "Person2" };
            var p3 = new Person { Name = "Person3" };

            _collection.Add(p1);
            _collection.Add(p3);
            _collection.Add(p2);
            _collection.Sort(new PersonNameComparer());

            Assert.AreEqual(3, _collection.Count);
            Assert.AreSame(p1, _collection[0]);
            Assert.AreSame(p2, _collection[1]);
            Assert.AreSame(p3, _collection[2]);
        }

        [TestMethod]
        public void TestClear()
        {
            var p1 = new Person { Name = "Person1" };
            var p2 = new Person { Name = "Person2" };
            var p3 = new Person { Name = "Person3" };

            _collection.Add(p1);
            _collection.Add(p3);
            _collection.Add(p2);

            _collection.CollectionChanged += CollectionChanged;
            _collection.Clear();
            // each item is removed separately
            Assert.AreEqual(3, _removeCount);
        }

        [TestMethod]
        public void TestAddTransientDuplicates()
        {
            _collection.CollectionChanged += CollectionChanged;
            _collection.Add(new Person { Name = "Person1" });
            _collection.Add(new Person { Name = "Person1" });

            _collection.Clear();
            // duplicate transient entities are allowed
            Assert.AreEqual(2, _addCount);
        }

        [TestMethod]
        public void TestAddDuplicates()
        {
            _collection.CollectionChanged += CollectionChanged;
            var p1 = new Person { Name = "Person1", Id = 1 };
            _collection.Add(p1);
            var p2 = new Person { Name = "Person1", Id = 1 };
            _collection.Add(p2);

            _collection.Clear();
            // duplicate non-transient entities are not allowed
            Assert.AreEqual(1, _addCount);
        }

        private int _addCount;
        private int _removeCount;

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _addCount++;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    _removeCount++;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.DomainModel.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ManyToManyFixupTest
    {
        public ManyToManyFixupTest()
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestManyToMany()
        {
            var many1 = new Class1 { Id = 1 };
            var many2 = new Class2 { Id = 2 };

            many1.Many1.Add(many2);
            Assert.IsTrue(many1.Many1.Contains(many2));
            Assert.IsTrue(many2.Many2.Contains(many1));

            many2.Many2.Remove(many1);
            Assert.IsFalse(many1.Many1.Contains(many2));
            Assert.IsFalse(many2.Many2.Contains(many1));
        }

        private class Class1 : EntityFrameworkEntity<long>
        {
            private TrackableCollection<long, Class2> _many1;

            public Class1()
            {
                Many1 = new TrackableCollection<long, Class2>();
            }

            public TrackableCollection<long, Class2> Many1
            {
                get
                {
                    return _many1;
                }
                private set
                {
                    if (Many1 == value)
                        return;

                    ThrowExceptionIfChangeTrackingEnabled();

                    if (Many1 != null)
                    {
                        Many1.CollectionChanged -= FixupMany;
                    }

                    _many1 = value;

                    if (Many1 != null)
                    {
                        Many1.CollectionChanged += FixupMany;
                    }

                    RaiseNavigationPropertyChanged(() => Many1);
                }
            }

            private void FixupMany(object sender, NotifyCollectionChangedEventArgs e)
            {
                FixupManyToManyProperty<Class1, Class2>(e, x => x.Many1, y => y.Many2);
            }
        }

        private class Class2 : EntityFrameworkEntity<long>
        {
            private TrackableCollection<long, Class1> _many2;

            public Class2()
            {
                Many2 = new TrackableCollection<long, Class1>();
            }

            public TrackableCollection<long, Class1> Many2
            {
                get { return _many2; }
                private set
                {
                    if (Many2 == value)
                        return;

                    ThrowExceptionIfChangeTrackingEnabled();

                    if (Many2 != null)
                    {
                        Many2.CollectionChanged -= FixupMany;
                    }

                    _many2 = value;

                    if (Many2 != null)
                    {
                        Many2.CollectionChanged += FixupMany;
                    }

                    RaiseNavigationPropertyChanged(() => Many2);
                }
            }

            private void FixupMany(object sender, NotifyCollectionChangedEventArgs e)
            {
                FixupManyToManyProperty<Class2, Class1>(e, x => x.Many2, y => y.Many1);
            }
        }
    }
}

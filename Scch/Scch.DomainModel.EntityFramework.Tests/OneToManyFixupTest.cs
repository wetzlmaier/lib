using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.DomainModel.EntityFramework.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class OneToManyFixupTest
    {
        public OneToManyFixupTest()
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
        public void TestOneToMany()
        {
            var one = new One {Id=1};
            var many = new Many {Id = 2, One = one};

            Assert.AreEqual(one, many.One);
            Assert.AreEqual(1, many.OneId);
            Assert.IsTrue(one.Many.Contains(many));

            var second = new One {Id = 3};
            many.One = second;
            Assert.AreEqual(second, many.One);
            Assert.AreEqual(3, many.OneId);
            Assert.IsFalse(one.Many.Contains(many));
            Assert.IsTrue(second.Many.Contains(many));

            second.Many.Remove(many);
            Assert.IsFalse(second.Many.Contains(many));
            Assert.IsFalse(one.Many.Contains(many));
            Assert.IsNull(many.One);
        }

        private class One : EntityFrameworkEntity<long>
        {
            private TrackableCollection<long, Many> _many;

            public One()
            {
                Many = new TrackableCollection<long, Many>();
            }

            public TrackableCollection<long, Many> Many
            {
                get
                {
                    return _many;
                }
                private set
                {
                    if (Many == value)
                        return;

                    ThrowExceptionIfChangeTrackingEnabled();

                    if (Many != null)
                    {
                        Many.CollectionChanged -= FixupMany;
                    }

                    _many = value;

                    if (Many != null)
                    {
                        Many.CollectionChanged += FixupMany;
                    }

                    RaiseNavigationPropertyChanged(() => Many);
                }
            }

            private void FixupMany(object sender, NotifyCollectionChangedEventArgs e)
            {
                FixupOneToManyProperty<One, Many>(e, x => x.Many, x => x.One);
            }
        }

        private class Many : EntityFrameworkEntity<long>
        {
            private One _one;
            private long _oneId;

            public One One
            {
                get { return _one; }
                set
                {
                    if (_one == value)
                        return;

                    var previousValue = _one;
                    _one = value;
                    FixupManyToOneProperty(previousValue, x => x.One, x => x.Many);

                    RaiseNavigationPropertyChanged(() => One);
                }
            }

            [Browsable(false)]
            public long OneId
            {
                get { return _oneId; }
                set
                {
                    if (_oneId == value)
                        return;

                    ChangeTracker.RecordOriginalValue(() => OneId, _oneId);

                    _oneId = value;
                    RaisePropertyChanged(() => OneId);
                }
            }
        }
    }
}

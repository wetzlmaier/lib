using System.ComponentModel;
using Scch.Common.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Common.Tests.ComponentModel
{
    /// <summary>
    /// Summary description for SortDescriptionComparerTest
    /// </summary>
    [TestClass]
    public class SortDescriptionComparerTest
    {
        public SortDescriptionComparerTest()
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
        // [TestInitialize]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAscending()
        {
            var comparer = new SortDescriptionComparer<Person>(new[] { new SortDescription("Name", ListSortDirection.Ascending) });

            var x = new Person { Name = "b", Age = 1 };
            var y = new Person { Name = "a", Age = 2 };
            var z = new Person { Name = "b", Age = 1 };

            Assert.AreEqual(1, comparer.Compare(x, y));
            Assert.AreEqual(-1, comparer.Compare(y, x));
            Assert.AreEqual(0, comparer.Compare(x, z));
        }

        [TestMethod]
        public void TestDescending()
        {
            var comparer = new SortDescriptionComparer<Person>(new[] { new SortDescription("Name", ListSortDirection.Descending) });

            var x = new Person { Name = "b", Age = 1 };
            var y = new Person { Name = "a", Age = 2 };
            var z = new Person { Name = "b", Age = 1 };

            Assert.AreEqual(-1, comparer.Compare(x, y));
            Assert.AreEqual(1, comparer.Compare(y, x));
            Assert.AreEqual(0, comparer.Compare(x, z));
        }

        [TestMethod]
        public void TestMultiSortDescription()
        {
            var comparer = new SortDescriptionComparer<Person>(new[] { new SortDescription("Name", ListSortDirection.Ascending), 
                new SortDescription("Age", ListSortDirection.Descending) });

            var x = new Person { Name = "b", Age = 2 };
            var y = new Person { Name = "a", Age = 1 };
            var z = new Person { Name = "b", Age = 2 };

            Assert.AreEqual(1, comparer.Compare(x, y));
            Assert.AreEqual(-1, comparer.Compare(y, x));
            Assert.AreEqual(0, comparer.Compare(x, z));
        }

        class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}

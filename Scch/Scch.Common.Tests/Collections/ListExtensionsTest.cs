using System.Collections.Generic;
using Scch.Common.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Common.Tests.Collections
{
    /// <summary>
    /// Summary description for ListExtensionsTest
    /// </summary>
    [TestClass]
    public class ListExtensionsTest
    {
        public ListExtensionsTest()
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
        public void TestMoveUp()
        {
            var list = new List<int> {0, 1, 2, 3, 4, 5};

            list.Move(2, 4);

            Assert.AreEqual(3, list[2]);
            Assert.AreEqual(4, list[3]);
            Assert.AreEqual(2, list[4]);
        }

        [TestMethod]
        public void TestMoveDown()
        {
            var list = new List<int> {0, 1, 2, 3, 4, 5};

            list.Move(4, 2);

            Assert.AreEqual(4, list[2]);
            Assert.AreEqual(2, list[3]);
            Assert.AreEqual(3, list[4]);
        }
    }
}

using Scch.Common.ComponentModel;
using Scch.Mvvm.Tests.Properties;
using Scch.Mvvm.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Mvvm.Tests
{
    /// <summary>
    /// Summary description for EnumViewModelTest
    /// </summary>
    [TestClass]
    public class EnumViewModelTest
    {
        public EnumViewModelTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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

        enum TestEnum
        {
            EnumValue1,
            [LocalizedDisplayName(typeof(TestEnum), "EnumValue2", typeof(Resources))]
            EnumValue2 = 2
        }

        [TestMethod]
        public void Test()
        {
            var e = new EnumViewModel<int> { Enum = TestEnum.EnumValue1 };
            Assert.AreEqual("EnumValue1", e.Name);
            Assert.AreEqual(0, e.Value);

            e = new EnumViewModel<int> { Enum = TestEnum.EnumValue2 };
            Assert.AreEqual("Enum Value 2", e.Name);
            Assert.AreEqual(2, e.Value);
        }
    }
}

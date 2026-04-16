using System;
using Scch.Common.ComponentModel;
using Scch.Common.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Common.Tests
{
    /// <summary>
    /// Summary description for EnumConverterTest
    /// </summary>
    [TestClass]
    public class EnumValueToStringConverterTest
    {
        public EnumValueToStringConverterTest()
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

        enum TestEnum
        {
            EnumValue1,
            [LocalizedDisplayName(typeof(TestEnum), "EnumValue2", typeof(Resources))]
            EnumValue2 = 2
        }

        [Flags]
        enum TestFlags
        {
            FlagsValue1 = 1,
            [LocalizedDisplayName(typeof(TestEnum), "FlagsValue2", typeof(Resources))]
            FlagsValue2 = 2
        }

        [TestMethod]
        public void TestConvertEnum()
        {
            var converter = new EnumValueToStringConverter<TestEnum>();
            Assert.AreEqual("Enum Value 2", converter.Convert(TestEnum.EnumValue2, typeof(string), null, null));
        }

        [TestMethod]
        public void TestConvertBackEnum()
        {
            var converter = new EnumValueToStringConverter<TestEnum>();
            Assert.AreEqual(TestEnum.EnumValue2, converter.ConvertBack("Enum Value 2", typeof(TestEnum), null, null));
        }

        [TestMethod]
        public void TestConvertFlags()
        {
            var converter = new EnumValueToStringConverter<TestFlags>();
            Assert.AreEqual("FlagsValue1 | Flags Value 2", converter.Convert(TestFlags.FlagsValue1| TestFlags.FlagsValue2, typeof(string), null, null));
        }

        [TestMethod]
        public void TestConvertBackFlags()
        {
            var converter = new EnumValueToStringConverter<TestFlags>();
            Assert.AreEqual(TestFlags.FlagsValue1 | TestFlags.FlagsValue2, converter.ConvertBack("FlagsValue1 | Flags Value 2", typeof(TestFlags), null, null));
        }
    }
}

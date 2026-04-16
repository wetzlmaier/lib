using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections.Generic;
using Scch.Common.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Common.Tests.ComponentModel
{
    /// <summary>
    /// Summary description for DataErrorInfoTest
    /// </summary>
    [TestClass]
    public class DataErrorInfoTest
    {
        public DataErrorInfoTest()
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
        public void TestValidation()
        {
            var p = new Person();

            Assert.IsTrue(p["Name"].Length > 0);
            p.Name = "Name";
            Assert.IsTrue(p["Name"].Length == 0);
        }

        class Person : DataErrorInfo
        {
            private string _name;

            [Required]
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    if (_name == value)
                        return;
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }
    }
}

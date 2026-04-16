using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.DomainModel.Tests
{
    /// <summary>
    /// Summary description for EntityTest
    /// </summary>
    [TestClass]
    public class EntityTest
    {
        public EntityTest()
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
        [TestInitialize]
        public void MyTestInitialize()
        {
            _changedPropertyName = null;
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        class Dummy:LongEntity
        {
            private string _wrongProperty;
            private string _property;

            public string WrongProperty
            {
                get { return _wrongProperty; }
                set
                {
                    if (_wrongProperty == value)
                        return;

                    _wrongProperty = value;
                    RaisePropertyChanged(() => Property);
                }
            }

            public string Property
            {
                get { return _property; }
                set
                {
                    if (_property == value)
                        return;
                    
                    _property = value;
                    RaisePropertyChanged(() => Property);
                }
            }
        }

        private string _changedPropertyName;

        [TestMethod]
        public void NotifyPropertyChanged()
        {
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var dummy = new Dummy();
                dummy.PropertyChanged += dummy_PropertyChanged;
                dummy.WrongProperty = "test";
            });
        }

        void dummy_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName!="IsValid")
                _changedPropertyName = e.PropertyName;
        }

        [TestMethod]
        public void NotifyPropertyChangedWrongProperty()
        {
            var dummy = new Dummy();
            dummy.PropertyChanged += dummy_PropertyChanged;
            dummy.Property = "test";
            Assert.AreEqual("Property", _changedPropertyName);
        }
    }
}

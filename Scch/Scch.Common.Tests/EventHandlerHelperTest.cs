using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Common.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class EventHandlerHelperTest
    {
        public EventHandlerHelperTest()
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
        public void TestNonStaticEvent()
        {
            var dummy = new EventHandlerDummy();

            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(dummy, "NonStaticEvent", new EventHandler<EventArgs>(NonStaticEvent)));
            dummy.NonStaticEvent += NonStaticEvent;
            Assert.IsTrue(EventHandlerHelper.IsEventHandlerRegistered(dummy, "NonStaticEvent", new EventHandler<EventArgs>(NonStaticEvent)));
            dummy.NonStaticEvent -= NonStaticEvent;
            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(dummy, "NonStaticEvent", new EventHandler<EventArgs>(NonStaticEvent)));

            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(dummy, "NonStaticEvent", new EventHandler<EventArgs>(NonStaticEvent_Static)));
            dummy.NonStaticEvent += NonStaticEvent_Static;
            Assert.IsTrue(EventHandlerHelper.IsEventHandlerRegistered(dummy, "NonStaticEvent", new EventHandler<EventArgs>(NonStaticEvent_Static)));
            dummy.NonStaticEvent -= NonStaticEvent_Static;
            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(dummy, "NonStaticEvent", new EventHandler<EventArgs>(NonStaticEvent_Static)));
        }

        [TestMethod]
        public void TestStaticEvent()
        {
            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(typeof(EventHandlerDummy), "StaticEvent", new EventHandler<EventArgs>(NonStaticEvent)));
            EventHandlerDummy.StaticEvent += NonStaticEvent;
            Assert.IsTrue(EventHandlerHelper.IsEventHandlerRegistered(typeof(EventHandlerDummy), "StaticEvent", new EventHandler<EventArgs>(NonStaticEvent)));
            EventHandlerDummy.StaticEvent -= NonStaticEvent;
            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(typeof(EventHandlerDummy), "StaticEvent", new EventHandler<EventArgs>(NonStaticEvent)));

            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(typeof(EventHandlerDummy), "StaticEvent", new EventHandler<EventArgs>(NonStaticEvent_Static)));
            EventHandlerDummy.StaticEvent += NonStaticEvent_Static;
            Assert.IsTrue(EventHandlerHelper.IsEventHandlerRegistered(typeof(EventHandlerDummy), "StaticEvent", new EventHandler<EventArgs>(NonStaticEvent_Static)));
            EventHandlerDummy.StaticEvent -= NonStaticEvent_Static;
            Assert.IsFalse(EventHandlerHelper.IsEventHandlerRegistered(typeof(EventHandlerDummy), "StaticEvent", new EventHandler<EventArgs>(NonStaticEvent_Static)));
        }

        void NonStaticEvent(object sender, EventArgs e)
        {
        }

        static void NonStaticEvent_Static(object sender, EventArgs e)
        {
        }

        private class EventHandlerDummy
        {

#pragma warning disable 67
            public event EventHandler NonStaticEvent;
#pragma warning restore 67

#pragma warning disable 67
            public static event EventHandler StaticEvent;
#pragma warning restore 67

        }
    }
}

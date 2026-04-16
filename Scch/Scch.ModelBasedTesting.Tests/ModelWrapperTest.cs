using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Scch.ModelBasedTesting.Tests
{
    [TestClass]
    public class ModelWrapperTest
    {
        private ModelWrapper _wrapper;

        [TestInitialize]
        public void Setup()
        {
            Model.Reset();
            _wrapper = new ModelWrapper(typeof(Model), true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestActionNotFound()
        {
            _wrapper.PerformAction("NotFound");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestActionNotEnabled()
        {
            _wrapper.PerformAction("Action1");
            _wrapper.PerformAction("Action2");

            _wrapper.PerformAction("Action2");
        }

        [TestMethod]
        public void Test()
        {
            AreEqual(typeof(ActionAttribute), _wrapper.ActionType);
            AreEqual(typeof(Model), _wrapper.ModelType);

            AreEqual(3, _wrapper.Actions.Count);
            IsTrue(_wrapper.Actions.ContainsKey("Action1"));
            IsTrue(_wrapper.Actions.ContainsKey("Action2"));
            IsTrue(_wrapper.Actions.ContainsKey("DebugAction"));

            AreEqual(3, _wrapper.EnabledActions.Count);
            IsTrue(_wrapper.EnabledActions.ContainsKey("Action1"));
            IsTrue(_wrapper.EnabledActions.ContainsKey("Action2"));
            AreEqual("", _wrapper.State.ToString());

            _wrapper.PerformAction("Action1");
            AreEqual(2, _wrapper.EnabledActions.Count);
            IsFalse(_wrapper.EnabledActions.ContainsKey("Action1"));
            IsTrue(_wrapper.EnabledActions.ContainsKey("Action2"));
            AreEqual("Action1", _wrapper.State.ToString());

            _wrapper.PerformAction("Action2");
            AreEqual(1, _wrapper.EnabledActions.Count);
            IsFalse(_wrapper.EnabledActions.ContainsKey("Action1"));
            IsFalse(_wrapper.EnabledActions.ContainsKey("Action2"));
            AreEqual("Action1,Action2", _wrapper.State.ToString());
        }
    }
}

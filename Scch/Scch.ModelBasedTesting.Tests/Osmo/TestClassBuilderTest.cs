using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scch.ModelBasedTesting.Osmo;

namespace Scch.ModelBasedTesting.Tests.Osmo
{
    [TestClass]
    public class TestClassBuilderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBeforeTestException()
        {
            TestClassBuilder.Build(typeof(Model), true, "package", "name", "beforeTest", "Action2");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAfterTestException()
        {
            TestClassBuilder.Build(typeof(Model), true, "package", "name", "Action1", "afterTest");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBeforeAfterEqualException()
        {
            TestClassBuilder.Build(typeof(Model), true, "package", "name", "Action1", "Action1");
        }

        [TestMethod]
        public void Test()
        {
            var testClass = TestClassBuilder.Build(typeof(Model), true, "package", "name");

            Assert.AreEqual("name", testClass.Name);
            Assert.AreEqual("Model", testClass.ModelType);
            Assert.AreEqual("scch.modelbasedtesting.tests", testClass.ModelNamespace);
            Assert.AreEqual("Scch.ModelBasedTesting.Tests", testClass.ModelAssembly);
            Assert.AreEqual("package", testClass.Package);
            Assert.AreEqual(3, testClass.TestSteps.Count);

            var action1 = testClass.TestSteps.First(ts => ts.Name == "Action1");
            Assert.AreEqual("action1Enabled", action1.GuardMethodName);
            Assert.IsTrue(action1.GuardAvailable);
            Assert.AreEqual("Action1", action1.Name);
            Assert.AreEqual("action1", action1.MethodName);
            Assert.IsFalse(action1.IsBeforeTest);
            Assert.IsFalse(action1.IsAfterTest);
            Assert.AreEqual("Action1Enabled", action1.Guard);

            var action2 = testClass.TestSteps.First(ts => ts.Name == "Action2");
            Assert.AreEqual("action2Enabled", action2.GuardMethodName);
            Assert.IsTrue(action2.GuardAvailable);
            Assert.AreEqual("Action2", action2.Name);
            Assert.AreEqual("action2", action2.MethodName);
            Assert.IsFalse(action2.IsBeforeTest);
            Assert.IsFalse(action2.IsAfterTest);
            Assert.AreEqual("Action2Enabled", action2.Guard);
        }

        [TestMethod]
        public void TestBeforeAfter()
        {
            var testClass = TestClassBuilder.Build(typeof(Model), true, "package", "name", "Action1", "Action2");

            Assert.AreEqual("name", testClass.Name);
            Assert.AreEqual("Model", testClass.ModelType);
            Assert.AreEqual("scch.modelbasedtesting.tests", testClass.ModelNamespace);
            Assert.AreEqual("Scch.ModelBasedTesting.Tests", testClass.ModelAssembly);
            Assert.AreEqual("package", testClass.Package);
            Assert.AreEqual(3, testClass.TestSteps.Count);

            var action1 = testClass.TestSteps.First(ts => ts.Name == "Action1");
            Assert.AreEqual("action1Enabled", action1.GuardMethodName);
            Assert.IsFalse(action1.GuardAvailable);
            Assert.AreEqual("Action1", action1.Name);
            Assert.AreEqual("action1", action1.MethodName);
            Assert.IsTrue(action1.IsBeforeTest);
            Assert.IsFalse(action1.IsAfterTest);
            Assert.AreEqual("Action1Enabled", action1.Guard);

            var action2 = testClass.TestSteps.First(ts => ts.Name == "Action2");
            Assert.AreEqual("action2Enabled", action2.GuardMethodName);
            Assert.IsFalse(action2.GuardAvailable);
            Assert.AreEqual("Action2", action2.Name);
            Assert.AreEqual("action2", action2.MethodName);
            Assert.IsFalse(action2.IsBeforeTest);
            Assert.IsTrue(action2.IsAfterTest);
            Assert.AreEqual("Action2Enabled", action2.Guard);
        }

        [TestMethod]
        public void TestDomain()
        {
            var testClass = TestClassBuilder.Build(typeof(ModelDomain), true, "package", "name");
            Assert.AreEqual(12, testClass.TestSteps.Count);

            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[0], ModelDomain.ColorDomain[0]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[1], ModelDomain.ColorDomain[0]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[2], ModelDomain.ColorDomain[0]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[3], ModelDomain.ColorDomain[0]"));

            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[0], ModelDomain.ColorDomain[1]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[1], ModelDomain.ColorDomain[1]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[2], ModelDomain.ColorDomain[1]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[3], ModelDomain.ColorDomain[1]"));

            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[0], ModelDomain.ColorDomain[2]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[1], ModelDomain.ColorDomain[2]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[2], ModelDomain.ColorDomain[2]"));
            Assert.IsNotNull(testClass.TestSteps.FirstOrDefault(ts => ts.Parameter == "ModelDomain.NumberDomain()[3], ModelDomain.ColorDomain[2]"));

            Assert.AreEqual(12, testClass.TestSteps.Select(ts => ts.MethodName).Distinct().Count());
            Assert.AreEqual(12, testClass.TestSteps.Select(ts => ts.GuardMethodName).Distinct().Count());
        }
    }
}
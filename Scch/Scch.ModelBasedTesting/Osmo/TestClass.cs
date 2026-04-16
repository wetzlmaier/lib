using System.Collections.Generic;

namespace Scch.ModelBasedTesting.Osmo
{
    public class TestClass
    {
        public TestClass()
        {
            TestSteps = new List<TestStep>();
        }

        public IList<TestStep> TestSteps { get; }

        public string Name { get; set; }

        public string ModelType { get; set; }

        public string ModelNamespace { get; set; }

        public string ModelAssembly { get; set; }

        public string Package { get; set; }
    }
}

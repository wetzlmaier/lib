namespace Scch.ModelBasedTesting.Osmo
{
    public class TestStep
    {
        public string GuardMethodName { get; set; }

        public bool GuardAvailable { get; set; }

        public string Name { get; set; }

        public string MethodName { get; set; }

        public bool IsBeforeTest { get; set; }

        public bool IsAfterTest { get; set; }

        public string Guard { get; set; }

        public string Parameter { get; set; }

        public string Method { get; set; }
    }
}

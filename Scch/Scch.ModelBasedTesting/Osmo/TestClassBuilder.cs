using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scch.ModelBasedTesting.Osmo
{
    public static class TestClassBuilder
    {
        private delegate void BuildParameters(int position);

        public static TestClass Build(string modelType, bool debug, string package, string name, string beforeTest = null, string afterTest = null)
        {
            return Build(Type.GetType(modelType, true), debug, package, name, beforeTest, afterTest);
        }

        public static TestClass Build(Type modelType, bool debug, string package, string name, string beforeTest = null, string afterTest = null)
        {
            if (beforeTest != null && beforeTest == afterTest)
                throw new ArgumentException("BeforeTest and AfterTest methods cannot be equal.");

            var wrapper = new ModelWrapper(modelType, debug);

            if (afterTest != null && !wrapper.Actions.ContainsKey(afterTest))
                throw new ArgumentException("AfterTest method not defined as action.");
            if (beforeTest != null && !wrapper.Actions.ContainsKey(beforeTest))
                throw new ArgumentException("BeforeTest method not defined as action.");

            var assemblyName = Path.GetFileNameWithoutExtension(wrapper.ModelType.Assembly.Location);
            var testClass = new TestClass
            {
                Name = name,
                Package = package,
                ModelType = wrapper.ModelType.Name,
                ModelNamespace = wrapper.ModelType.Namespace.ToLower(),
                ModelAssembly = assemblyName,
            };

            foreach (var kvp in wrapper.Actions)
            {
                var action = kvp.Key;
                var combinations = kvp.Value;
                var guard = wrapper.GuardMethodName(action);
                StringBuilder guardMethodName = new StringBuilder(guard) { [0] = guard.ToLower()[0] };
                StringBuilder methodName = new StringBuilder(action) { [0] = action.ToLower()[0] };
                var isBeforeTest = action == beforeTest;
                var isAfterTest = action == afterTest;
                var guardAvailable = wrapper.GuardMethod(action) != null && !isBeforeTest && !isAfterTest;

                /*var parameterList = new List<List<string>>();

                for (int position = 0; position < wrapper.DomainNames(action).Count; position++)
                {
                    var values = new List<string>();
                    var code = wrapper.DomainCode(action, position);

                    for (int index = 0; index < wrapper.DomainValues(action, position).Length; index++)
                    {
                        values.Add($"{code}[{index}]");
                    }

                    parameterList.Add(values);
                }*/

                int number = 0;
                foreach (var combination in combinations)
                {
                    if (combination.Length == 0)
                    {
                        testClass.TestSteps.Add(new TestStep
                        {
                            Parameter = string.Empty,
                            Guard = guard,
                            GuardMethodName = guardMethodName.ToString(),
                            GuardAvailable = guardAvailable,
                            Name = action,
                            Method = action,
                            MethodName = methodName.ToString(),
                            IsBeforeTest = isBeforeTest,
                            IsAfterTest = isAfterTest
                        });
                    }
                    else
                    {
                        var parameter = new StringBuilder();
                        for (int position = 0; position < combination.Length; position++)
                        {
                            var code = wrapper.DomainCode(action, position);
                            parameter.Append($", {testClass.ModelType}.{code}[{combination[position]}]");
                        }
                        parameter.Remove(0, 2);

                        testClass.TestSteps.Add(new TestStep
                        {
                            Parameter = parameter.ToString(),
                            Guard = guard,
                            GuardMethodName = guardMethodName.ToString() + number,
                            GuardAvailable = guardAvailable,
                            Name = action + number,
                            Method = action,
                            MethodName = methodName.ToString() + number,
                            IsBeforeTest = isBeforeTest,
                            IsAfterTest = isAfterTest
                        });
                    }
                    number++;
                }
            }

            return testClass;
        }
    }
}

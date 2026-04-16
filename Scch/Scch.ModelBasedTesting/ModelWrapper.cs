using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Scch.Common.Configuration;

namespace Scch.ModelBasedTesting
{
    public class ModelWrapper
    {
        private readonly PropertyInfo _stateProperty;
        private readonly BindingFlags _bindingFlags;
        private readonly string _guardPostfix;
        private readonly string _guardPrefix;
        private readonly IDictionary<string, IList<string>> _domainIndex;
        private readonly IDictionary<string, Tuple<string, object[]>> _domainValues;

        public ModelWrapper(string modelType, bool debug = false) : this(Type.GetType(modelType, true), debug)
        {
        }

        public ModelWrapper(Type modelType, bool debug = false)
        {
            _bindingFlags = BindingFlags.Static | BindingFlags.Public;

            ModelType = modelType;
            ActionType = Type.GetType(ConfigurationHelper.Current.ReadString("ActionType", typeof(ActionAttribute).AssemblyQualifiedName), true);
            DebugActionType = Type.GetType(ConfigurationHelper.Current.ReadString("DebugActionType", typeof(DebugActionAttribute).AssemblyQualifiedName), true);
            _stateProperty = ModelType.GetProperty(ConfigurationHelper.Current.ReadString("StateProperty", "State"), _bindingFlags);

            _guardPostfix = ConfigurationHelper.Current.ReadString("GuardPostfix", "Enabled");
            _guardPrefix = ConfigurationHelper.Current.ReadString("GuardPrefix", string.Empty);

            _domainIndex = new Dictionary<string, IList<string>>();
            _domainValues = new Dictionary<string, Tuple<string, object[]>>();

            ReadDomains();

            Actions = new Dictionary<string, int[][]>();
            ReadActions(ActionType);

            if (debug)
            {
                ReadActions(DebugActionType);
            }
        }

        private void AddDomain(string domainName, MethodInfo method)
        {
            if (_domainValues.ContainsKey(domainName))
                return;

            var code = method.Name;

            if (method.IsSpecialName)
                code = code.Remove(0, 4);
            else
                code += "()";

            var values = method.Invoke(null, null) as IEnumerable;

            if (values == null)
                throw new InvalidOperationException($"Return type of domain '{domainName}' is not an IEnumerable");

            _domainValues.Add(domainName, new Tuple<string, object[]>(code, values.Cast<object>().ToArray()));
        }

        private void ReadDomains()
        {
            foreach (var property in ModelType.GetProperties(_bindingFlags))
            {
                var domainAttribute = (DomainAttribute)property.GetCustomAttributes(typeof(DomainAttribute)).SingleOrDefault();

                if (domainAttribute == null)
                {
                    domainAttribute = (DomainAttribute)property.GetGetMethod().GetCustomAttributes(typeof(DomainAttribute)).SingleOrDefault();

                    if (domainAttribute == null)
                        continue;
                }

                AddDomain(domainAttribute.Name, property.GetGetMethod());
            }

            foreach (var method in ModelType.GetMethods(_bindingFlags))
            {
                var methodDomain = (DomainAttribute)method.GetCustomAttributes(typeof(DomainAttribute)).SingleOrDefault();

                if (methodDomain == null)
                    continue;

                AddDomain(methodDomain.Name, method);
            }
        }

        private void ReadActions(Type type)
        {
            foreach (var method in ModelType.GetMethods(_bindingFlags))
            {
                if (method.GetCustomAttributes().All(attribute => attribute.GetType() != type))
                    continue;

                _domainIndex.Add(method.Name, new List<string>());

                foreach (var parameter in method.GetParameters())
                {
                    var paramDomain = (DomainAttribute)parameter.GetCustomAttributes(typeof(DomainAttribute)).SingleOrDefault();

                    if (paramDomain == null)
                        throw new InvalidOperationException($"Domain attribute missing on parameter '{parameter.Name}' for action '{method.Name}'.");

                    _domainIndex[method.Name].Add(paramDomain.Name);
                }

                Actions.Add(method.Name, GetCombinations(CreateParameters(method.Name)).ToArray());
            }
        }

        public Type ModelType { get; }

        public Type ActionType { get; }

        public Type DebugActionType { get; }

        public IDictionary<string, int[][]> Actions { get; }

        public IDictionary<string, int[][]> EnabledActions
        {
            get
            {
                var enabledActions = new Dictionary<string, int[][]>();

                foreach (var kvp in Actions)
                {
                    var enabledCombinations = new List<int[]>();
                    var action = kvp.Key;
                    var combinations = kvp.Value;

                    foreach (var combination in combinations)
                    {
                        if (!IsActionEnabled(action, combination))
                            continue;

                        enabledCombinations.Add(combination);
                    }

                    if (enabledCombinations.Count > 0)
                    {
                        enabledActions.Add(action, enabledCombinations.ToArray());
                    }
                }

                return enabledActions;
            }
        }

        private List<List<int>> CreateParameters(string action)
        {
            var parameters = new List<List<int>>();

            for (int position = 0; position < DomainNames(action).Count; position++)
            {
                var values = new List<int>();

                for (int index = 0; index < DomainValues(action, position).Length; index++)
                {
                    values.Add(index);
                }

                parameters.Add(values);
            }

            return parameters;
        }

        private void CreateCombinations(List<List<int>> parameters, List<int> built, int depth, List<int[]> results)
        {
            if (depth >= parameters.Count)
            {
                results.Add(built.ToArray());
                return;
            }

            List<int> values = parameters[depth];

            foreach (var option in values)
            {
                var list = new List<int>(built) { option };
                CreateCombinations(parameters, list, depth + 1, results);
            }
        }

        private List<int[]> GetCombinations(List<List<int>> parameters)
        {
            List<int[]> results = new List<int[]>();
            CreateCombinations(parameters, new List<int>(), 0, results);
            return results;
        }

        private object[] MapParameters(string action, int[] indexes)
        {
            if (DomainNames(action).Count != indexes.Length)
                throw new InvalidOperationException($"Parameter count of action '{action}' does not match.");

            object[] values = new object[indexes.Length];

            for (int position = 0; position < indexes.Length; position++)
                values[position] = DomainValue(action, position, indexes[position]);

            return values;
        }

        private bool IsActionEnabled(string action, params int[] indexes)
        {
            var guard = GuardMethod(action);

            var enabled = true;

            if (guard != null)
                enabled = (bool)guard.Invoke(null, MapParameters(action, indexes));

            return enabled;
        }

        public string GuardMethodName(string action)
        {
            return _guardPrefix + action + _guardPostfix;
        }

        public MethodInfo GuardMethod(string action)
        {
            return ModelType.GetMethod(GuardMethodName(action));
        }

        public object State => _stateProperty?.GetValue(null);

        public void PerformAction(string action, params int[] indexes)
        {
            if (indexes == null)
                indexes = new int[0];

            if (DomainNames(action).Count != indexes.Length)
                throw new InvalidOperationException($"Parameter count of action '{action}' does not match.");

            if (!IsActionEnabled(action, indexes))
                throw new InvalidOperationException($"Action '{action}' is not enabled.");

            MethodInfo selectedMethod = null;
            foreach (var method in ModelType.GetMethods())
            {
                if (method.GetCustomAttributes(ActionType).SingleOrDefault() == null && method.GetCustomAttributes(DebugActionType).SingleOrDefault() == null)
                    continue;

                if (method.Name != action)
                    continue;

                var parameters = method.GetParameters();

                if (parameters.Length != indexes.Length)
                    continue;

                if (parameters.Length == 0)
                {
                    selectedMethod = method;
                    break;
                }

                for (int position = 0; position < parameters.Length; position++)
                {
                    var parameter = parameters[position];
                    var paramDomain = (DomainAttribute)parameter.GetCustomAttributes(typeof(DomainAttribute)).SingleOrDefault();

                    if (paramDomain == null || paramDomain.Name != DomainName(action, position))
                        continue;

                    selectedMethod = method;
                    break;
                }

                if (selectedMethod != null)
                    break;
            }

            if (selectedMethod == null)
                throw new InvalidOperationException($"Method for action '{action}' not found.");

            selectedMethod.Invoke(null, MapParameters(action, indexes));
        }

        public IList<string> DomainNames(string action)
        {
            if (!_domainIndex.ContainsKey(action))
                throw new ArgumentException($"Action '{action}' not found.");

            return _domainIndex[action];
        }

        public string DomainName(string action, int position)
        {
            var domainNames = DomainNames(action);

            if (position < 0 || position >= domainNames.Count)
                throw new ArgumentOutOfRangeException(nameof(position));

            return domainNames[position];
        }

        public string DomainCode(string action, int position)
        {
            var domainName = DomainName(action, position);

            if (!_domainValues.ContainsKey(domainName))
                throw new ArgumentOutOfRangeException(nameof(position));

            var domainValues = _domainValues[domainName];
            return domainValues.Item1;
        }

        public object[] DomainValues(string action, int position)
        {
            var domainName = DomainName(action, position);

            if (!_domainValues.ContainsKey(domainName))
                throw new ArgumentOutOfRangeException(nameof(position));

            var domainValues = _domainValues[domainName];
            return domainValues.Item2;
        }

        public object DomainValue(string action, int position, int index)
        {
            var domainValues = DomainValues(action, position);
            if (index < 0 || index >= domainValues.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            return domainValues[index];
        }

        public DataTable CreateDataTable(string action)
        {
            var combinations = Actions[action];
            var columns = DomainNames(action);

            var dt = new DataTable();
            for (int position = 0; position < columns.Count; position++)
            {
                var column = dt.Columns.Add("column" + position, typeof(string));
                column.Caption = columns[position];
            }

            foreach (var combination in combinations)
            {
                var row = dt.NewRow();

                for (int position = 0; position < columns.Count; position++)
                {
                    row[position] = DomainValue(action, position, combination[position]);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}

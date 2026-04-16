using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Scch.Common.Reflecton
{
    public static class RuntimeTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "DynamicTypes" };
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly Dictionary<string, Type> BuiltTypes = new Dictionary<string, Type>();

        static RuntimeTypeBuilder()
        {
            ModuleBuilder = Thread.GetDomain().DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(AssemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            return fields.Aggregate(string.Empty, (current, field) => current + (field.Key + ";" + field.Value.Name + ";"));
        }

        public static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            if (null == fields)
                throw new ArgumentNullException("fields");
            if (0 == fields.Count)
                throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

            try
            {
                Monitor.Enter(BuiltTypes);
                string className = GetTypeKey(fields);

                if (BuiltTypes.ContainsKey(className))
                    return BuiltTypes[className];

                TypeBuilder typeBuilder = ModuleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                foreach (var field in fields)
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                BuiltTypes[className] = typeBuilder.CreateType();

                return BuiltTypes[className];
            }
            finally
            {
                Monitor.Exit(BuiltTypes);
            }
        }

        private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
        {
            return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;

namespace Scch.Common.Reflecton
{
    /// <summary>
    /// Helper class for type operations.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// Converts the string parameter values into their real type specified as an array of <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameterInfos">The array of <see cref="ParameterInfo"/>.</param>
        /// <param name="parameterValues">The array of parameter values.</param>
        /// <returns>An object array with the parameter values.</returns>
        public static object[] ConvertParameterTypes(ParameterInfo[] parameterInfos, string[] parameterValues)
        {
            if (parameterInfos != null && parameterValues != null && parameterInfos.Length != parameterValues.Length)
                throw new ArgumentException("Parameter lists not equal.");

            var values = new ArrayList();

            if (parameterInfos != null && parameterValues != null && parameterValues.Length > 0)
            {
                for (int i = 0; i < parameterValues.Length; i++)
                {
                    if (parameterInfos[i].ParameterType.IsEnum && parameterValues[i].GetType()==typeof(string))
                        values.Add(Enum.Parse(parameterInfos[i].ParameterType, parameterValues[i]));
                    else
                        values.Add(Convert.ChangeType(parameterValues[i], parameterInfos[i].ParameterType));
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Creates an instance of the specified <see cref="Type"/> using a matching constructor with the same number of parameters as the string parameter values.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameterValues">The array of parameter values.</param>
        /// <returns>The instance of the specified type.</returns>
        public static object CreateInstance(Type type, string[] parameterValues)
        {
            return CreateInstance<object>(type, parameterValues);
        }

        /// <summary>
        /// Creates an instance of the specified <see cref="Type"/> using a matching constructor with the same number of parameters as the string parameter values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="parameterValues">The array of parameter values.</param>
        /// <returns>The instance of the specified type.</returns>
        public static T CreateInstance<T>(Type type, string[] parameterValues)
        {
            return (T)Activator.CreateInstance(type, ConvertConstructorParameterTypes(type, parameterValues));
        }

        /// <summary>
        /// Converts the string parameter values into their real type specified as <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methodInfo">The <see cref="MethodInfo"/>.</param>
        /// <param name="parameterValues">The array of parameter values.</param>
        /// <returns>An object array with the parameter values.</returns>
        public static object[] ConvertMethodParameterTypes(MethodInfo methodInfo, string[] parameterValues)
        {
            var values = new object[0];

            if (parameterValues != null && parameterValues.Length > 1)
            {
                values = ConvertParameterTypes(methodInfo.GetParameters(), parameterValues);
            }

            return values;
        }

        /// <summary>
        /// Converts the string parameter values into their real type using a matching constructor with the same number of parameters as the string parameter values.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameterValues">The array of parameter values.</param>
        /// <returns>An object array with the parameter values.</returns>
        public static object[] ConvertConstructorParameterTypes(Type type, string[] parameterValues)
        {
            var values = new object[0];

            if (parameterValues != null && parameterValues.Length > 0)
            {
                ConstructorInfo[] cis = type.GetConstructors();
                ConstructorInfo constructor = null;

                foreach (ConstructorInfo ci in cis)
                {
                    ParameterInfo[] pis = ci.GetParameters();
                    if (pis.Length == parameterValues.Length)
                    {
                        if (constructor != null)
                            throw new AmbiguousMatchException("More than one matching constructor found.");

                        constructor = ci;
                    }
                }

                if (constructor == null)
                    throw new ArgumentException("No matching constructor found.");

                values = ConvertParameterTypes(constructor.GetParameters(), parameterValues);
            }

            return values;
        }

        /// <summary>
        /// Returns the identifier for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The identifier for the specified <see cref="Type"/>.</returns>
        public static string GetTypeIdentifier(Type type)
        {
            return type.FullName.Replace(".", "_").Replace("+", "_");
        }

        /// <summary>
        /// Builds and returns a concrete <see cref="Type"/> from the specified abstract <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The abstract <see cref="Type"/>.</param>
        /// <returns>A concrete <see cref="Type"/> from the specified abstract <see cref="Type"/>.</returns>
        public static Type BuildConcreteType(Type type)
        {
            if (type==null)
                throw new ArgumentNullException();

            if (!type.IsAbstract)
                throw new ArgumentOutOfRangeException("type", "Type is not abstract.");

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(type.Assembly.GetName(),
               AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(type.Name + "Module",
                                          type.FullName + ".dll");

            TypeBuilder typeBuilder = moduleBuilder.DefineType(type.FullName,
                                            TypeAttributes.Public | TypeAttributes.Class, type);

            // inherit from abstract type
            ConstructorBuilder constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard, Type.EmptyTypes);

            // make a default constructor, which does not call the base constructor.
            ILGenerator il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ret);

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates an example object from the specified <see cref="Type"/> and fills the properties with the specified values.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="values">The values.</param>
        /// <returns>An example object from the specified <see cref="Type"/> filled with the specified values.</returns>
        public static object CreateExampleObject(Type type, IDictionary<Type, object> values=null)
        {
            if (type.IsAbstract)
            {
                type = BuildConcreteType(type);
            }

            var exampleObject = Activator.CreateInstance(type);

            if (values != null && values.Count > 0)
            {
                foreach (PropertyInfo pi in exampleObject.GetType().GetProperties())
                {
                    if (pi.CanWrite)
                    {
                        object value;

                        if (values.TryGetValue(pi.PropertyType, out value))
                        {
                            var pd = TypeDescriptor.GetProperties(exampleObject.GetType()).Find(pi.Name, false);
                            if (value != null && pd.Converter != null && pd.Converter.CanConvertFrom(value.GetType()))
                            {
                                value = pd.Converter.ConvertFrom(value);
                            }

                            pi.SetValue(exampleObject, value, null);
                        }
                    }
                }
            }

            return exampleObject;
        }
    }
}

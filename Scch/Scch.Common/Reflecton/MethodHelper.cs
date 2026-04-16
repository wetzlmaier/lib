using System;
using System.Reflection;

namespace Scch.Common.Reflecton
{
    /// <summary>
    /// Helper methods for <see cref="MethodInfo"/>.
    /// </summary>
    public static class MethodHelper
    {
        /// <summary>
        /// Returns the implicit operator method from one type to another.
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        public static MethodInfo GetImplicitOperator(Type fromType, Type toType)
        {
            if (toType == null)
                throw new ArgumentNullException("toType");

            MethodInfo implicitOperator = toType.GetMethod("op_Implicit", new[] { fromType });

            if (implicitOperator != null)
                return implicitOperator;

            if (fromType == null)
                throw new ArgumentNullException("fromType");

            MethodInfo reverseImplicitOperator = fromType.GetMethod("op_Implicit", new[] { fromType });

            if (reverseImplicitOperator != null && reverseImplicitOperator.ReturnType == toType)
                return reverseImplicitOperator;

            return null;
        }

        /// <summary>
        /// Converts and returns a value into a destination type.
        /// </summary>
        /// <param name="fromValue">The value.</param>
        /// <param name="toType">The destination type.</param>
        /// <returns>The converted value.</returns>
        public static object InvokeImplicitOperator(object fromValue, Type toType)
        {
            MethodInfo implicitOperator = GetImplicitOperator(fromValue != null ? fromValue.GetType() : null, toType);
            return implicitOperator.Invoke(null, new[] { fromValue });
        }
    }
}

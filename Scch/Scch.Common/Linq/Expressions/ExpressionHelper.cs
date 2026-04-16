using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Scch.Common.Linq.Expressions
{
    public static class ExpressionHelper
    {
        public static object GetPropertyValue(LambdaExpression expression, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return GetPropertyValue(GetPropertyPath(expression), obj);
        }

        private static object GetPropertyValue(string propertyName, object obj)
        {
            var property = obj.GetType().GetProperty(propertyName);
            return property.GetValue(obj, null);
        }

        public static string GetPropertyPath(LambdaExpression expression)
        {
            // Retrieve member expression:  
            var members = new List<PropertyInfo>();
            CollectRelationalMembers(expression, members);

            // Build string expression:  
            var sb = new StringBuilder();
            string separator = "";
            foreach (PropertyInfo member in members)
            {
                sb.Append(separator);
                sb.Append(member.Name);
                separator = ".";
            }

            // Apply Include:  
            return sb.ToString();
        }

        private static void CollectRelationalMembers(Expression exp, IList<PropertyInfo> members)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Lambda:
                    CollectRelationalMembers(((LambdaExpression)exp).Body, members);
                    break;

                case ExpressionType.MemberAccess:
                    var mexp = (MemberExpression)exp;
                    CollectRelationalMembers(mexp.Expression, members);
                    members.Add((PropertyInfo)mexp.Member);
                    break;

                case ExpressionType.Call:
                    var cexp = (MethodCallExpression)exp;

                    if (cexp.Method.IsStatic == false)
                        throw new InvalidOperationException("Invalid type of expression.");

                    foreach (var arg in cexp.Arguments)
                        CollectRelationalMembers(arg, members);
                    break;

                case ExpressionType.Parameter:
                    break;

                case ExpressionType.Convert:
                    CollectRelationalMembers(((UnaryExpression)exp).Operand, members);
                    break;

                case ExpressionType.Constant:
                    break;

                default:
                    throw new InvalidOperationException("Invalid type of expression.");
            }
        }

        public static object GetPropertyValue<T>(Expression<Func<T, object>> property, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return GetPropertyValue(GetPropertyPath(property), obj);
        }

        public static string GetPropertyPath<T>(Expression<Func<T, object>> property)
        {
            return GetPropertyPath((LambdaExpression)property);
        }

        public static object GetPropertyValue<T>(Expression<Func<T>> property, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return GetPropertyValue(GetPropertyPath(property), obj);
        }

        public static string GetPropertyPath<T>(Expression<Func<T>> property)
        {
            return GetPropertyPath((LambdaExpression)property);
        }

        public static T2 GetPropertyValue<T1, T2>(Expression<Func<T1, T2>> property, T1 obj)
        {
            if (Equals(obj, null))
                throw new ArgumentNullException("obj");

            return (T2)GetPropertyValue(GetPropertyPath(property), obj);
        }

        public static string GetPropertyPath<T1, T2>(Expression<Func<T1, T2>> property)
        {
            return GetPropertyPath((LambdaExpression)property);
        }
    }
}

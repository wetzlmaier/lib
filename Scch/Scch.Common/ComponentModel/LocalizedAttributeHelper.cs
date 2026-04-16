using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Scch.Common.ComponentModel
{
    public static class LocalizedAttributeHelper
    {
        public static string Localize(string resourceName, Type resourceType)
        {
            if ((resourceType == null) || string.IsNullOrEmpty(resourceName))
            {
                throw new InvalidOperationException("Need both resource type and resource name");
            }

            PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            if (property != null)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                if ((getMethod == null) || (!getMethod.IsAssembly && !getMethod.IsPublic))
                {
                    property = null;
                }
            }

            if (property == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Resource type '{0}' does not have property '{1}'", resourceType.FullName, resourceName));
            }

            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Resource property '{0}' not string type '{1}'", new object[] { property.Name, resourceType.FullName }));
            }

            return (string)property.GetValue(null, null);
        }

        public static IDictionary<string, string> CreateDisplayNameDictionary<T>()
        {
            return CreateDictionary<LocalizedDisplayNameAttribute, T>(a => a.DisplayName);
        }

        public static IDictionary<string, string> CreateCategoryDictionary<T>()
        {
            return CreateDictionary<LocalizedCategoryAttribute, T>(a => a.Category);
        }

        public static IDictionary<string, string> CreateDescriptionDictionary<T>()
        {
            return CreateDictionary<LocalizedDescriptionAttribute, T>(a => a.Description);
        }

        public static IDictionary<string, string> CreateDictionary<TAttribute, T>(Expression<Func<TAttribute, string>> e)
        {
            IDictionary<string, string> displayNames = new Dictionary<string, string>();
            foreach (var property in typeof(T).GetProperties())
            {
                var attributes = (LocalizedDisplayNameAttribute[])property.GetCustomAttributes(typeof(TAttribute), true);
                var displayName = (attributes.Length == 0) ? property.Name : attributes[0].DisplayName;
                displayNames.Add(property.Name, displayName);
            }

            return displayNames;
        }
    }
}

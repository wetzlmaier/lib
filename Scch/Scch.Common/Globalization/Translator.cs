using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Scch.Common.ComponentModel;

namespace Scch.Common.Globalization
{
    public static class Translator
    {
        private static DisplayNameAttribute[] GetDisplayNameAttribute(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(LocalizedDisplayNameAttribute), true).Cast<DisplayNameAttribute>().ToArray();

            if (attributes.Length == 0)
            {
                attributes = memberInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().ToArray();
            }

            return attributes;
        }

        public static string Translate(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var attributes = GetDisplayNameAttribute(type);

            if (attributes.Length == 0)
            {
                if (type.IsGenericType && (type.GetGenericArguments().Length == 1))
                    return Translate(type.GetGenericArguments()[0]);

                return null;
            }

            return attributes[0].DisplayName;
        }

        public static string Translate(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            var attributes = GetDisplayNameAttribute(propertyInfo);

            return attributes.Length == 0 ? Translate(propertyInfo.PropertyType) : attributes[0].DisplayName;
        }

        public static string Translate(Type type, string property)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (property == null)
                throw new ArgumentNullException("property");

            PropertyInfo propertyInfo = type.GetProperty(property);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Type '{0}' does not have property '{1}'", type.FullName, property));
            }

            return Translate(propertyInfo);
        }

        public static string Translate(Enum e)
        {
            var type = e.GetType();
            var memberInfo = type.GetMember(Enum.GetName(e.GetType(), e));
            var attributes = GetDisplayNameAttribute(memberInfo[0]);
            return attributes.Length == 0 ? null : attributes[0].DisplayName;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Scch.Common.Conversion;

namespace Scch.Common.Reflecton
{
    /// <summary>
    /// Helper class for property operations.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Returns the real property type for a property. If the property type is a <see cref="Nullable{T}"/>, then the type T is returned.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyDescriptor"/> of the property.</param>
        /// <returns>The real type of the property.</returns>
        public static Type GetRealPropertyType(PropertyDescriptor pd)
        {
            return GetRealPropertyType(pd.PropertyType);
        }

        /// <summary>
        /// Returns the real property type for a property. If the property type is a <see cref="Nullable{T}"/>, then the type T is returned.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/> of the property.</param>
        /// <returns>The real type of the property.</returns>
        public static Type GetRealPropertyType(PropertyInfo pi)
        {
            return GetRealPropertyType(pi.PropertyType);
        }

        /// <summary>
        /// Returns the real type. If the type is a <see cref="Nullable{T}"/>, then the type T is returned.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The real type.</returns>
        public static Type GetRealPropertyType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>)
                       ? type.GetGenericArguments()[0]
                       : type;
        }

        #region ReadPropertyValue
        /// <summary>
        /// Returns the property value of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The property value of an object.</returns>
        public static object ReadPropertyValue(object obj, string propertyName)
        {
            return ReadPropertyValue<object>(obj, propertyName);
        }

        /// <summary>
        /// Returns the strongly typed property value of an object.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The strongly typed property value of an object.</returns>
        public static T ReadPropertyValue<T>(object obj, string propertyName)
        {
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            object value = pi.GetValue(obj, null);

            return Converter.ConvertTo<T>(value);
        }

        /// <summary>
        /// Returns the strongly typed property value of an object.
        /// </summary>
        /// <param name="returnType">The type of the return value.</param>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The strongly typed property value of an object.</returns>
        public static object ReadPropertyValue(Type returnType, object obj, string propertyName)
        {
            // Todo TypeDescriptor.GetProperties(obj.GetType())[1].Converter.ConvertToString(obj)
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            object value = pi.GetValue(obj, null);

            return Converter.ConvertTo(value, returnType);
        }

        /// <summary>
        /// Returns the property values of an object in a dictionary.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The property values of an object in a dictionary.</returns>
        public static IDictionary<PropertyInfo, object> ReadPropertyValues(object obj)
        {
            return ReadPropertyValues(obj, null);
        }
        /// <summary>
        /// Returns the property values of an object in a dictionary.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="excluded">A collection of excluded types.</param>
        /// <returns>The property values of an object in a dictionary.</returns>
        public static IDictionary<PropertyInfo, object> ReadPropertyValues(object obj, IEnumerable<Type> excluded)
        {
            var excludedTypes = new List<Type>();

            if (excluded != null)
            {
                excludedTypes.AddRange(excluded);
            }

            IDictionary<PropertyInfo, object> result = new Dictionary<PropertyInfo, object>();

            PropertyInfo[] pis = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                // if the property type is not excluded
                if (!excludedTypes.Contains(pi.PropertyType))
                {
                    result.Add(pi, pi.GetValue(obj, null));
                }
            }

            return result;
        }
        #endregion ReadPropertyValue

        #region WritePropertyValue
        /// <summary>
        /// Sets the property value of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="ignorePropertyNotFound">If true, then return, if the property is not found.</param>
        public static void WritePropertyValue(object obj, string propertyName, object value, bool ignorePropertyNotFound)
        {
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);

            if (pi == null)
            {
                if (ignorePropertyNotFound)
                    return;
                
                throw new PropertyNotFoundException(propertyName);
            }

            pi.SetValue(obj, Converter.ConvertTo(value, pi.PropertyType), null);
        }

        /// <summary>
        /// Sets the property value of an object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void WritePropertyValue(object obj, string propertyName, object value)
        {
            WritePropertyValue(obj, propertyName, value, false);
        }
        #endregion WritePropertyValue

        /// <summary>
        /// Creates and returns the projection of property values from a source collection specified by the property name.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="typeInCollection">The <see cref="Type"/> in the collection.</param>
        /// <param name="coll">The source collection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The projection of property values from a source collection specified by the property name.</returns>
        public static List<TTarget> GetProjection<TTarget>(Type typeInCollection, IEnumerable coll, string propertyName)
        {
            PropertyInfo pi = typeInCollection.GetProperty(propertyName);

            if (pi == null)
                throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName));

            return (from object obj in coll
                    select obj != null ? Converter.ConvertTo<TTarget>(pi.GetValue(obj, null)) : default(TTarget)).ToList();
        }

        /// <summary>
        /// Creates and returns the projection of property values from a source collection specified by the property name.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="coll">The source collection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The projection of property values from a source collection specified by the property name.</returns>
        public static List<TTarget> GetProjection<TSource, TTarget>(IEnumerable<TSource> coll, string propertyName)
        {
            return GetProjection<TTarget>(typeof (TSource), coll, propertyName);
        }

        /// <summary>
        /// Returns the <see cref="PropertyDescriptor"/> of a <see cref="Type"/> in a <see cref="IDictionary{TKey,TValue}"/> with the name as key.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDictionary<string, PropertyDescriptor> GetPropertyDescriptors(Type type)
        {
            return TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(pd=>pd.IsBrowsable).ToDictionary(pd => pd.DisplayName);
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyDescriptor"/> belongs to a floating point property.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyDescriptor"/>.</param>
        /// <returns>True, if the <see cref="PropertyDescriptor"/> belongs to a floating point property.</returns>
        public static bool IsFloatingPointNumber(PropertyDescriptor pd)
        {
            return IsFloatingPointNumber(pd.PropertyType);
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyInfo"/> belongs to a floating point property.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <returns>True, if the <see cref="PropertyInfo"/> belongs to a floating point property.</returns>
        public static bool IsFloatingPointNumber(PropertyInfo pi)
        {
            return IsFloatingPointNumber(pi.PropertyType);
        }

        /// <summary>
        /// Returns true, if the type is a floating point number.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>True, if the <see cref="Type"/> is a floating point number.</returns>
        public static bool IsFloatingPointNumber(Type type)
        {
            type = GetRealPropertyType(type);
            return (type == typeof(float) || type == typeof(double) ||
                 type == typeof(decimal));
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyDescriptor"/> belongs to a signed int property.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyDescriptor"/>.</param>
        /// <returns>True, if the <see cref="PropertyDescriptor"/> belongs to a signed int property.</returns>
        public static bool IsSignedIntNumber(PropertyDescriptor pd)
        {
            return IsSignedIntNumber(pd.PropertyType);
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyInfo"/> belongs to a signed int property.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <returns>True, if the <see cref="PropertyInfo"/> belongs to a signed int property.</returns>
        public static bool IsSignedIntNumber(PropertyInfo pi)
        {
            return IsSignedIntNumber(pi.PropertyType);
        }

        /// <summary>
        /// Returns true, if the <see cref="Type"/> is a  signed int number.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>True, if the <see cref="Type"/> is a  signed int number.</returns>
        public static bool IsSignedIntNumber(Type type)
        {
            type = GetRealPropertyType(type);
            return (type == typeof(short) || type == typeof(int) || type == typeof(long));
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyDescriptor"/> belongs to an unsigned int property.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyDescriptor"/>.</param>
        /// <returns>True, if the <see cref="PropertyDescriptor"/> belongs to an unsigned int property.</returns>
        public static bool IsUnsignedIntNumber(PropertyDescriptor pd)
        {
            return IsUnsignedIntNumber(pd.PropertyType);
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyInfo"/> belongs to an unsigned int property.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <returns>True, if the <see cref="PropertyInfo"/> belongs to an unsigned int property.</returns>
        public static bool IsUnsignedIntNumber(PropertyInfo pi)
        {
            return IsUnsignedIntNumber(pi.PropertyType);
        }

        /// <summary>
        /// Returns true, if the <see cref="Type"/> is an unsigned int number.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>True, if the <see cref="Type"/> is an unsigned int number.</returns>
        public static bool IsUnsignedIntNumber(Type type)
        {
            type = GetRealPropertyType(type);
            return (type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong));
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyDescriptor"/> belongs to a number property.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyDescriptor"/>.</param>
        /// <returns>True, if the <see cref="PropertyDescriptor"/> belongs to a number property.</returns>
        public static bool IsNumber(PropertyDescriptor pd)
        {
            return IsNumber(pd.PropertyType);
        }

        /// <summary>
        /// Returns true, if the <see cref="PropertyInfo"/> belongs to a number property.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <returns>True, if the <see cref="PropertyInfo"/> belongs to a number property.</returns>
        public static bool IsNumber(PropertyInfo pi)
        {
            return IsNumber(pi.PropertyType);
        }

        /// <summary>
        /// Returns true, if the type is a number.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>True, if the <see cref="Type"/> is a number.</returns>
        public static bool IsNumber(Type type)
        {
            type = GetRealPropertyType(type);
            return IsFloatingPointNumber(type) || IsSignedIntNumber(type) || IsUnsignedIntNumber(type);
        }

        /// <summary>
        /// Returns the <see cref="NumberType"/> of a <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="pd">The <see cref="PropertyDescriptor"/>.</param>
        /// <returns>The <see cref="NumberType"/> of a <see cref="PropertyDescriptor"/>.</returns>
        public static NumberType GetNumberType(PropertyDescriptor pd)
        {
            return GetNumberType(pd.PropertyType);
        }

        /// <summary>
        /// Returns the <see cref="NumberType"/> of a <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="pi">The <see cref="PropertyInfo"/>.</param>
        /// <returns>The <see cref="NumberType"/> of a <see cref="PropertyInfo"/>.</returns>
        public static NumberType GetNumberType(PropertyInfo pi)
        {
            return GetNumberType(pi.PropertyType);
        }

        /// <summary>
        /// Returns the <see cref="NumberType"/> of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The <see cref="NumberType"/> of a <see cref="Type"/>.</returns>
        public static NumberType GetNumberType(Type type)
        {
            var numberType = NumberType.NoNumber;

            if (IsFloatingPointNumber(type))
            {
                numberType = NumberType.FloatingPoint;
            }
            else if (IsUnsignedIntNumber(type))
            {
                numberType = NumberType.UnsignedInt;
            }
            else if (IsSignedIntNumber(type))
            {
                numberType = NumberType.SignedInt;
            }

            return numberType;
        }

        /// <summary>
        /// Returns the lowest property for a <see cref="Type"/> specified by its name.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="name">The property name.</param>
        /// <returns>The lowest property for a <see cref="Type"/> specified by its name.</returns>
        /// <remarks><see cref="Type.GetProperty(string,System.Reflection.BindingFlags)"/> throws a <see cref="AmbiguousMatchException"/>, if a property is overridden.</remarks>
        public static PropertyInfo GetLowestProperty(Type type, string name)
        {
            while (type != typeof(object))
            {
                var property = type.GetProperty(name, BindingFlags.DeclaredOnly |
                                                      BindingFlags.Public |
                                                      BindingFlags.Instance);
                if (property != null)
                {
                    return property;
                }
                type = type.BaseType;
            }

            return null;
        }
    }
}

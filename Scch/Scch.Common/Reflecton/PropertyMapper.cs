using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;

namespace Scch.Common.Reflecton
{
    /// <summary>
    /// Methods for mapping property values.
    /// </summary>
    public static class PropertyMapper
    {
        /// <summary>
        /// Maps the properties from the source object to the destination object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="propertyProvider">Provider type for properties.</param>
        /// <param name="excludedProperties">Excluded properties.</param>
        public static void Map(object source, object destination, Type propertyProvider, params string[] excludedProperties)
        {
            Map(source, destination, propertyProvider, false, excludedProperties);
        }

        /// <summary>
        /// Maps the properties from the source object to the destination object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="propertyProvider">Provider type for properties.</param>
        /// <param name="inherited">If true, the inherited properties would also be used. Otherwise not.</param>
        /// <param name="excludedProperties">Excluded properties.</param>
        public static void Map(object source, object destination, Type propertyProvider, bool inherited, params string[] excludedProperties)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (destination == null)
                throw new ArgumentNullException("destination");

            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            if (!inherited)
                flags |= BindingFlags.DeclaredOnly;

            List<Exception> exceptions = new List<Exception>();

            foreach (PropertyInfo providerProperty in propertyProvider.GetProperties(flags))
            {
                try
                {
                    if (!excludedProperties.Contains(providerProperty.Name))
                    {
                        Map(source, destination, providerProperty.Name);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Maps the specified propertiy from the source object to the destination object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="propertyName">The property.</param>
        public static void Map(object source, object destination, string propertyName)
        {
            PropertyInfo destinationProperty = destination.GetType().GetProperty(propertyName);
            PropertyInfo sourceProperty = source.GetType().GetProperty(propertyName);

            if (sourceProperty != null && destinationProperty != null && destinationProperty.CanWrite && destinationProperty.GetSetMethod(false) != null)
            {
                MethodInfo implicitOperator = null;

                if (sourceProperty.PropertyType != destinationProperty.PropertyType)
                    implicitOperator = MethodHelper.GetImplicitOperator(sourceProperty.PropertyType, destinationProperty.PropertyType);

                object value = sourceProperty.GetValue(source, null);
                if (implicitOperator != null)
                {
                    value = implicitOperator.Invoke(null, new[] { value });
                }

                destinationProperty.SetValue(destination, value, null);
            }
        }
    }
}

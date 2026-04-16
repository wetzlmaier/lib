using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace Scch.Common
{
    public static class ObjectCloner
    {
        public static T ShallowClone<T>(T obj) 
        {
            var clone = Activator.CreateInstance<T>();
            Copy(obj, clone);
            return clone;
        }

        public static void Copy<T>(T source, T destination) 
        {
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (!propertyInfo.CanWrite)
                    continue;

                object sourceValue = propertyInfo.GetValue(source, null);
                object destinationValue = propertyInfo.GetValue(destination, null);

                if (!Equals(sourceValue, destinationValue))
                    propertyInfo.SetValue(destination, sourceValue, null);
            }
        }

        /// <summary>
        /// Clone the object, and returning a reference to a cloned object.
        /// </summary>
        /// <param name="template">The template object.</param>
        /// <returns>Reference to the new cloned object.</returns>
        public static object DeepClone(object template)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(template.GetType());
                serializer.WriteObject(stream, template);
                stream.Seek(0, SeekOrigin.Begin);
                return serializer.ReadObject(stream);
            }
        }
    }
}

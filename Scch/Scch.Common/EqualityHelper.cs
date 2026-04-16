using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Scch.Common
{
    public static class EqualityHelper
    {
        public static bool EntityEquals<T>(T source, T destination)
        {
            bool equal = true;
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (!propertyInfo.CanWrite)
                    continue;

                if (propertyInfo.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0)
                    continue;

                object sourceValue = propertyInfo.GetValue(source, null);
                object destinationValue = propertyInfo.GetValue(destination, null);

                if (sourceValue == destinationValue)
                    continue;

                if (propertyInfo.PropertyType.IsArray)
                {
                    var sourceArray = ((Array)sourceValue);
                    var destinationArray = ((Array)sourceValue);

                    equal = sourceArray != null && destinationArray != null && (sourceArray.Length == destinationArray.Length);
                    if (equal)
                    {
                        for (long i = 0; i < sourceArray.Length; i++)
                        {
                            equal = Equals(sourceArray.GetValue(i), destinationArray.GetValue(i));
                            if (!equal)
                                break;
                        }
                    }
                }
                else
                    equal = Equals(sourceValue, destinationValue);

                if (!equal)
                    break;
            }

            return equal;
        }
    }
}

using System;

namespace Scch.Common.Conversion
{
    /// <summary>
    /// Offers conversion functions.
    /// </summary>
    public static class Converter
    {
        #region ConvertTo
        /// <summary>
        /// Converts the value to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(object value)
        {
            return (T)ConvertTo(value, typeof(T));
        }

        /// <summary>
        /// Converts the value to a specified <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="conversionType">The <see cref="Type"/>.</param>
        /// <returns>The converted value.</returns>
        public static object ConvertTo(object value, Type conversionType)
        {
            // if the value is IConvertible use the ChangeType method, otherwise simply return the value.
            return (value is IConvertible) ? Convert.ChangeType(value, conversionType, null) : value;
        }

        #endregion ConvertTo
    }
}

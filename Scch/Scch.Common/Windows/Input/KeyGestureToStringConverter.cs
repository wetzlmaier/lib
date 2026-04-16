using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Scch.Common.Windows.Input
{
    /// <summary>
    /// This converter class will convert <see cref="KeyGesture"/> objects to strings.
    /// </summary>
    public class KeyGestureToStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts from a <see cref="KeyGesture"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is KeyGesture))
                throw new ArgumentOutOfRangeException("value");

            if (value == null)
                return string.Empty;

            return ((KeyGesture)value).GetDisplayStringForCulture(culture);
        }

        /// <summary>
        /// Cannopt convert back from a <see cref="string"/> to a <see cref="KeyGesture"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

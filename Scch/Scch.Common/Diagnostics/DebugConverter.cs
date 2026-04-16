using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Scch.Common.Diagnostics
{
    /// <summary>
    /// <see cref="IValueConverter"/> for debugging purposes.
    /// </summary>
    public class DebugConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="IValueConverter.Convert"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.WriteLine("Convert: Value '{0}', TargetType '{1}', Parameter '{2}'", value, targetType, parameter);
            Debugger.Break();
            return value; 
        }

        /// <summary>
        /// <see cref="IValueConverter.ConvertBack"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Convert Level to left margin.
    /// </summary>
    public class LevelToIndentConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// The identifier for the <see cref="Intent"/> dependency property. 
        /// </summary>
        public static DependencyProperty IntentProperty = DependencyProperty.Register("Intent", typeof(int), typeof(LevelToIndentConverter));

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToBrushConverter"/> class.
        /// </summary>
        public LevelToIndentConverter()
        {
            Intent = 19;
        }

        /// <summary>
        /// The intent.
        /// </summary>
        public int Intent
        {
            get
            {
                return (int)GetValue(IntentProperty);
            }
            set
            {
                SetValue(IntentProperty, value);
            }
        }

        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            return new Thickness((int)o * Intent, 0, 0, 0);
        }

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

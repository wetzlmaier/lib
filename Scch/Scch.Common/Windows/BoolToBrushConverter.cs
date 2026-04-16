using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Scch.Common.Windows
{
    /// <summary>
    /// <see cref="IValueConverter"/> for converting <see cref="bool"/> to <see cref="Brush"/> values.
    /// </summary>
    public class BoolToBrushConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// The identifier for the <see cref="FalseColor"/> dependency property. 
        /// </summary>
        public static DependencyProperty FalseColorProperty = DependencyProperty.Register("FalseColor", typeof(Brush), typeof(BoolToBrushConverter));
        /// <summary>
        /// The identifier for the <see cref="TrueColor"/> dependency property. 
        /// </summary>
        public static DependencyProperty TrueColorProperty = DependencyProperty.Register("TrueColor", typeof(Brush), typeof(BoolToBrushConverter));

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToBrushConverter"/> class.
        /// </summary>
        public BoolToBrushConverter()
        {
            FalseColor = Brushes.Black;
            TrueColor = Brushes.White;
        }

        /// <summary>
        /// The <see cref="Brush"/> for true values.
        /// </summary>
        public Brush FalseColor
        {
            get
            {
                return (Brush)GetValue(FalseColorProperty);
            }
            set
            {
                SetValue(FalseColorProperty, value);
            }
        }

        /// <summary>
        /// The <see cref="Brush"/> for false values.
        /// </summary>
        public Brush TrueColor
        {
            get
            {
                return (Brush)GetValue(TrueColorProperty);
            }
            set
            {
                SetValue(TrueColorProperty, value);
            }
        }

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
            return ((bool)value) ? TrueColor : FalseColor;
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
            throw new NotSupportedException();
            //return TrueColor.Color.Equals(((SolidColorBrush)value).Color);
        }
    }
}

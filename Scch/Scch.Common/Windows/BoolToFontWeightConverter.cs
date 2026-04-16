using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Scch.Common.Windows
{
    /// <summary>
    /// <see cref="IValueConverter"/> for converting <see cref="bool"/> to <see cref="FontWeight"/> values.
    /// </summary>
    public class BoolToFontWeightConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// The identifier for the <see cref="FalseFontWeight"/> dependency property. 
        /// </summary>
        public static DependencyProperty FalseFontWeightProperty = DependencyProperty.Register("FalseFontWeight", typeof(FontWeight), typeof(BoolToFontWeightConverter));
        /// <summary>
        /// The identifier for the <see cref="TrueFontWeight"/> dependency property. 
        /// </summary>
        public static DependencyProperty TrueFontWeightProperty = DependencyProperty.Register("TrueFontWeight", typeof(FontWeight), typeof(BoolToFontWeightConverter));

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToFontWeightConverter"/> class.
        /// </summary>
        public BoolToFontWeightConverter()
        {
            FalseFontWeight = FontWeights.Normal;
            TrueFontWeight = FontWeights.Bold;
        }

        /// <summary>
        /// The <see cref="FontWeight"/> for true values.
        /// </summary>
        public FontWeight FalseFontWeight
        {
            get
            {
                return (FontWeight)GetValue(FalseFontWeightProperty);
            }
            set
            {
                SetValue(FalseFontWeightProperty, value);
            }
        }

        /// <summary>
        /// The <see cref="FontWeight"/> for false values.
        /// </summary>
        public FontWeight TrueFontWeight
        {
            get
            {
                return (FontWeight)GetValue(TrueFontWeightProperty);
            }
            set
            {
                SetValue(TrueFontWeightProperty, value);
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
            return ((bool)value) ? TrueFontWeight : FalseFontWeight;
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
            return (FontWeight)value == TrueFontWeight;
        }
    }
}

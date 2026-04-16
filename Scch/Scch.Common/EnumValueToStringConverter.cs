using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using Scch.Common.Globalization;

namespace Scch.Common
{
    public class EnumValueToStringConverter<T> : IValueConverter
    {
        private const char Delimitter = '|';

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, null))
                throw new ArgumentNullException();

            if (value.GetType() != typeof(T))
                throw new ArgumentOutOfRangeException(nameof(value));

            if (targetType != typeof(string))
                throw new ArgumentOutOfRangeException(nameof(targetType));

            var hasFlags = value.GetType().GetCustomAttributes(typeof(FlagsAttribute)).Any();

            StringBuilder result = new StringBuilder();
            foreach (Enum enumValue in Enum.GetValues(typeof(T)))
            {
                if (hasFlags)
                {
                    if (((Enum)value).HasFlag(enumValue))
                    {
                        result.Append(" " + Delimitter + " " + TranslateValue(enumValue));
                    }
                }
                else
                {
                    if (Equals(enumValue, value))
                    {
                        return TranslateValue(enumValue);
                    }
                }
            }

            if (result.Length > 0)
                result.Remove(0, 3);

            return result.ToString();
        }

        private string TranslateValue(Enum value)
        {
            var localized = Translator.Translate(value);
            return localized ?? Enum.GetName(value.GetType(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                throw new ArgumentOutOfRangeException(nameof(value));

            if (targetType != typeof(T))
                throw new ArgumentOutOfRangeException(nameof(targetType));

            var strValue = (string)value;
            Type underlyingType = Enum.GetUnderlyingType(targetType);
            dynamic valueAsInt = System.Convert.ChangeType(0, underlyingType);

            foreach (Enum enumValue in Enum.GetValues(typeof(T)))
            {
                foreach (var flag in strValue.Split(Delimitter))
                {
                    var localized = Translator.Translate(enumValue);
                    if (Equals(localized ?? Enum.GetName(typeof(T), enumValue), flag.Trim()))
                    {
                        dynamic flagAsInt = System.Convert.ChangeType(enumValue, underlyingType);

                        valueAsInt |= flagAsInt;
                    }
                }
            }

            return (T)valueAsInt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Scch.Common.Reflecton;

namespace Scch.Common.Configuration
{
    public class ConfigurationHelper
    {
        private const char ArgPrefix = '/';
        private const char KeyValueDelimiter = ':';
        private static readonly ConfigurationHelper _current;
        private readonly System.Configuration.Configuration _configuration;
        private static readonly Color[] _namedColors;

        static ConfigurationHelper()
        {
            _current = new ConfigurationHelper(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            _namedColors = new ColorConverter().GetStandardValues(null).Cast<Color>().ToArray();
        }

        public static ConfigurationHelper Current
        {
            get { return _current; }
        }

        private ConfigurationHelper(System.Configuration.Configuration configuration)
        {
            _configuration = configuration;
        }

        public static ConfigurationHelper OpenConfiguration(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            var exeMap = new ExeConfigurationFileMap { ExeConfigFilename = path };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(exeMap, ConfigurationUserLevel.None);

            return new ConfigurationHelper(configuration);
        }

        public static ConfigurationHelper OpenAssemblyConfiguration(Type type)
        {
            return OpenAssemblyConfiguration(Assembly.GetAssembly(type));
        }

        public static ConfigurationHelper OpenAssemblyConfiguration(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            return OpenConfiguration(AssemblyHelper.GetAssemblyFileName(assembly) + ".config");
        }

        public string[] AllKeys
        {
            get { return _configuration.AppSettings.Settings.AllKeys; }
        }

        private string ReadValue(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            var args = Environment.GetCommandLineArgs();
            for (int i = 1; i < args.Length; i++)
            {
                var arg = ArgPrefix + name;
                if (!args[i].StartsWith(arg))
                    continue;

                if (args[i].Length == arg.Length)
                    return "-1";

                if (args[i][arg.Length] != KeyValueDelimiter)
                    continue;

                return args[i].Substring(arg.Length + 1);
            }

            return null;
        }

        private string[] SplitString(string value, params char[] separators)
        {
            return value == null ? new string[0] : value.Split(separators).Select(str => str.Trim()).ToArray();
        }

        #region ReadString
        public string ReadStringFromArgs(string name)
        {
            return ReadStringFromArgs(name, null);
        }

        public string ReadStringFromArgs(string name, string defaultValue)
        {
            string result = ReadValue(name);
            return String.IsNullOrEmpty(result) ? defaultValue : result;
        }

        public string ReadStringFromConfig(string key)
        {
            return ReadStringFromConfig(key, null);
        }

        public string ReadStringFromConfig(string key, string defaultValue)
        {
            var settings = _configuration.AppSettings.Settings;

            if (settings[key] == null)
                return defaultValue;

            string result = settings[key].Value;
            return String.IsNullOrEmpty(result) ? defaultValue : result;
        }

        public string ReadString(string name)
        {
            return ReadString(name, null);
        }

        public string[] ReadStringArray(string name)
        {
            return ReadStringArray(name, StringHelper.CommaSemicolon);
        }

        public string[] ReadStringArray(string name, char[] separators)
        {
            return ReadStringArray(name, new string[0], separators);
        }

        public string ReadString(string name, string defaultValue)
        {
            string result = ReadStringFromArgs(name);
            result = string.IsNullOrEmpty(result) ? ReadStringFromConfig(name) : result;
            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }

        public string[] ReadStringArray(string name, string[] defaultValue)
        {
            return ReadStringArray(name, defaultValue, StringHelper.CommaSemicolon);
        }

        public string[] ReadStringArray(string name, string[] defaultValue, char[] separators)
        {
            var value = ReadString(name);

            if (value == null)
                return defaultValue;

            return SplitString(value, StringHelper.CommaSemicolon);
        }
        #endregion ReadString

        #region ReadInt
        public int ReadIntFromArgs(string name)
        {
            return ReadIntFromArgs(name, 0);
        }

        public int ReadIntFromArgs(string name, int defaultValue)
        {
            int result;
            return !Int32.TryParse(ReadStringFromArgs(name), out result) ? defaultValue : result;
        }

        public int ReadIntFromConfig(string key, int defaultValue = 0)
        {
            int result;
            return !Int32.TryParse(ReadStringFromConfig(key), out result) ? defaultValue : result;
        }

        public int ReadInt(string name, int defaultValue = 0)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            int result;
            return !Int32.TryParse(str, out result) ? defaultValue : result;
        }
        #endregion ReadInt

        #region ReadDouble
        public double ReadDoubleFromArgs(string name, double defaultValue = 0)
        {
            double result;
            return !Double.TryParse(ReadStringFromArgs(name), out result) ? defaultValue : result;
        }

        public double ReadDoubleFromConfig(string key, double defaultValue = 0)
        {
            double result;
            return !Double.TryParse(ReadStringFromConfig(key), out result) ? defaultValue : result;
        }

        public double ReadDouble(string name, double defaultValue = 0)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            double result;
            return !Double.TryParse(str, out result) ? defaultValue : result;
        }
        #endregion ReadDouble

        #region ReadFloat
        public float ReadFloatFromArgs(string name, float defaultValue = 0)
        {
            float result;
            return !float.TryParse(ReadStringFromArgs(name), out result) ? defaultValue : result;
        }

        public float ReadFloatFromConfig(string key, float defaultValue = 0)
        {
            float result;
            return !float.TryParse(ReadStringFromConfig(key), out result) ? defaultValue : result;
        }

        public float ReadFloat(string name, float defaultValue = 0)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            float result;
            return !float.TryParse(str, out result) ? defaultValue : result;
        }
        #endregion ReadFloat

        #region ReadLong
        public long ReadLongFromArgs(string name, long defaultValue = 0)
        {
            long result;
            return !Int64.TryParse(ReadStringFromArgs(name), out result) ? defaultValue : result;
        }

        public long ReadLongFromConfig(string key, long defaultValue = 0)
        {
            long result;
            return !Int64.TryParse(ReadStringFromConfig(key), out result) ? defaultValue : result;
        }

        public long ReadLong(string name, long defaultValue = 0)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            long result;
            return !Int64.TryParse(str, out result) ? defaultValue : result;
        }
        #endregion ReadLong

        #region ReadBool
        public bool ReadBoolFromArgs(string name, bool defaultValue = false)
        {
            bool result;
            return !Boolean.TryParse(ReadStringFromArgs(name), out result) ? defaultValue : result;
        }

        public bool ReadBoolFromConfig(string key, bool defaultValue = false)
        {
            bool result;
            return !Boolean.TryParse(ReadStringFromConfig(key), out result) ? defaultValue : result;
        }

        public bool ReadBool(string name, bool defaultValue = false)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            bool result;
            return !Boolean.TryParse(str, out result) ? defaultValue : result;
        }
        #endregion ReadBool

        #region ReadDateTime
        public DateTime ReadDateTimeFromArgs(string name)
        {
            return ReadDateTimeFromArgs(name, DateTime.MinValue);
        }

        public DateTime ReadDateTimeFromArgs(string name, DateTime defaultValue)
        {
            return TryParse(ReadStringFromArgs(name), defaultValue);
        }

        public DateTime ReadDateTimeFromConfig(string key)
        {
            return ReadDateTimeFromConfig(key, DateTime.MinValue);
        }

        public DateTime ReadDateTimeFromConfig(string key, DateTime defaultValue)
        {
            return TryParse(ReadStringFromConfig(key), defaultValue);
        }

        public DateTime ReadDateTime(string name)
        {
            return ReadDateTime(name, DateTime.MinValue);
        }

        public DateTime ReadDateTime(string name, DateTime defaultValue)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            return TryParse(str, defaultValue);
        }

        private DateTime TryParse(string str, DateTime defaultValue)
        {
            DateTime result;
            return !DateTime.TryParse(str, out result) ? defaultValue : result;
        }
        #endregion ReadDateTime

        #region ReadExactDateTime
        public DateTime ReadExactDateTimeFromArgs(string name, string format)
        {
            return ReadExactDateTimeFromArgs(name, format, DateTime.MinValue);
        }

        public DateTime ReadExactDateTimeFromArgs(string name, string format, DateTime defaultValue)
        {
            return TryParseExact(ReadStringFromArgs(name), format, defaultValue);
        }

        public DateTime ReadExactDateTimeFromConfig(string key, string format)
        {
            return ReadExactDateTimeFromConfig(key, format, DateTime.MinValue);
        }

        public DateTime ReadExactDateTimeFromConfig(string key, string format, DateTime defaultValue)
        {
            return TryParseExact(ReadStringFromConfig(key), format, defaultValue);
        }

        public DateTime ReadExactDateTime(string name, string format)
        {
            return ReadExactDateTime(name, format, DateTime.MinValue);
        }

        public DateTime ReadExactDateTime(string name, string format, DateTime defaultValue)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            return TryParseExact(str, format, defaultValue);
        }

        private DateTime TryParseExact(string str, string format, DateTime defaultValue)
        {
            DateTime result;
            return !DateTime.TryParseExact(str, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? defaultValue : result;
        }
        #endregion ReadDateTime

        #region ReadRectangle
        public Rectangle ReadRectangleFromArgs(string name)
        {
            return ReadRectangleFromArgs(name, Rectangle.Empty);
        }

        public Rectangle ReadRectangleFromArgs(string name, Rectangle defaultValue)
        {
            return TryParse(ReadStringFromArgs(name), defaultValue);
        }

        public Rectangle ReadRectangleFromConfig(string key)
        {
            return ReadRectangleFromConfig(key, Rectangle.Empty);
        }

        public Rectangle ReadRectangleFromConfig(string key, Rectangle defaultValue)
        {
            return TryParse(ReadStringFromConfig(key), defaultValue);
        }

        public Rectangle ReadRectangle(string name)
        {
            return ReadRectangle(name, Rectangle.Empty);
        }

        public Rectangle ReadRectangle(string name, Rectangle defaultValue)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            return TryParse(str, defaultValue);
        }

        private Rectangle TryParse(string str, Rectangle defaultValue)
        {
            var parts = SplitString(str, StringHelper.CommaSemicolon);

            if (parts.Length != 4)
                return defaultValue;

            int x, y, width, height;
            if (!int.TryParse(parts[0], out x) || !int.TryParse(parts[1], out y) || !int.TryParse(parts[2], out width) || !int.TryParse(parts[3], out height))
                return defaultValue;

            return new Rectangle(x, y, width, height);
        }
        #endregion ReadRectangle

        #region ReadColor
        public Color ReadColorFromArgs(string name)
        {
            return ReadColorFromArgs(name, Color.Empty);
        }

        public Color ReadColorFromArgs(string name, Color defaultValue)
        {
            return TryParse(ReadStringFromArgs(name), defaultValue);
        }

        public Color ReadColorFromConfig(string key)
        {
            return ReadColorFromConfig(key, Color.Empty);
        }

        public Color ReadColorFromConfig(string key, Color defaultValue)
        {
            return TryParse(ReadStringFromConfig(key), defaultValue);
        }

        public Color ReadColor(string name)
        {
            return ReadColor(name, Color.Empty);
        }

        public Color ReadColor(string name, Color defaultValue)
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            return TryParse(str, defaultValue);
        }

        private Color TryParse(string str, Color defaultValue)
        {
            var color = (from c in _namedColors where c.Name == str select c).FirstOrDefault();

            if (!color.IsEmpty)
                return color;

            var parts = SplitString(str, StringHelper.Comma);

            if (parts.Length != 4)
                return defaultValue;

            int a, r, g, b;
            if (!int.TryParse(parts[0], out a) || !int.TryParse(parts[1], out r) ||
                !int.TryParse(parts[2], out g) || !int.TryParse(parts[3], out b))
                return defaultValue;

            return Color.FromArgb(a, r, g, b);
        }

        #endregion ReadColor

        #region ReadEnum
        public TEnum ReadEnumFromArgs<TEnum>(string name) where TEnum : struct
        {
            return ReadEnumFromArgs(name, default(TEnum));
        }

        public TEnum ReadEnumFromArgs<TEnum>(string name, TEnum defaultValue) where TEnum : struct
        {
            return TryParse(ReadStringFromArgs(name), defaultValue);
        }

        public TEnum ReadEnumFromConfig<TEnum>(string key) where TEnum : struct
        {
            return ReadEnumFromConfig(key, default(TEnum));
        }

        public TEnum ReadEnumFromConfig<TEnum>(string key, TEnum defaultValue) where TEnum : struct
        {
            return TryParse(ReadStringFromConfig(key), defaultValue);
        }

        public TEnum ReadEnum<TEnum>(string name) where TEnum : struct
        {
            return ReadEnum(name, default(TEnum));
        }

        public TEnum ReadEnum<TEnum>(string name, TEnum defaultValue) where TEnum : struct
        {
            string str = ReadStringFromArgs(name);
            str = String.IsNullOrEmpty(str) ? ReadStringFromConfig(name) : str;

            return TryParse(str, defaultValue);
        }

        private TEnum TryParse<TEnum>(string str, TEnum defaultValue) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse(str, out result) ? result : defaultValue;
        }
        #endregion

        public void Save()
        {
            _configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(_configuration.AppSettings.SectionInformation.Name);
        }

        public void AddKey(string key, string value)
        {
            _configuration.AppSettings.Settings.Remove(key);
        }

        public void RemoveKey(string key, string value)
        {
            _configuration.AppSettings.Settings.Remove(key);
        }

        public void SetValue(string key, string value)
        {
            var settings = _configuration.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
        }

        public void WriteStringToConfig(string key, string value)
        {
            SetValue(key, value);
        }

        public void WriteIntToConfig(string key, int value, string format = null)
        {
            WriteStringToConfig(key, value.ToString(format));
        }
        public void WriteIntToConfig(string key, int value, IFormatProvider provider = null)
        {
            WriteStringToConfig(key, value.ToString(provider));
        }

        public void WriteDoubleToConfig(string key, double value, string format = null)
        {
            WriteStringToConfig(key, value.ToString(format));
        }

        public void WriteDoubleToConfig(string key, double value, IFormatProvider provider = null)
        {
            WriteStringToConfig(key, value.ToString(provider));
        }

        public void WriteFloatToConfig(string key, float value, string format = null)
        {
            WriteStringToConfig(key, value.ToString(format));
        }

        public void WriteFloatToConfig(string key, float value, IFormatProvider provider = null)
        {
            WriteStringToConfig(key, value.ToString(provider));
        }

        public void WriteLongToConfig(string key, long value, string format = null)
        {
            WriteStringToConfig(key, value.ToString(format));
        }

        public void WriteLongToConfig(string key, long value, IFormatProvider provider = null)
        {
            WriteStringToConfig(key, value.ToString(provider));
        }

        public void WriteBoolToConfig(string key, bool value, IFormatProvider provider = null)
        {
            WriteStringToConfig(key, value.ToString(provider));
        }

        public void WriteDateTimeToConfig(string key, DateTime value, string format = null, IFormatProvider provider = null)
        {
            if (provider == null)
                provider = DateTimeFormatInfo.CurrentInfo;

            WriteStringToConfig(key, value.ToString(format, provider));
        }

        public void WriteRectanglelToConfig(string key, Rectangle value)
        {
            WriteStringToConfig(key, string.Format("{0}, {1}, {2}, {3}", value.X, value.Y, value.Width, value.Height));
        }

        public static string CreateArgs(IDictionary<string, object> args)
        {
            var sb = new StringBuilder();

            foreach (var kvp in args)
            {
                var arg = CreateArg(kvp.Key, kvp.Value);
                sb.Append(" " + arg);
            }

            if (sb.Length > 0)
                sb.Remove(0, 1);

            return sb.ToString();
        }

        private static string CreateArg(string key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (value == null)
                return string.Empty;

            if (value as string == string.Empty)
                return string.Empty;

            return string.Format("{0}{1}{2}{3}", ArgPrefix, key, KeyValueDelimiter, value);
        }

        public static string CreateArgs(NameValueCollection args)
        {
            var dictionary = args.AllKeys.ToDictionary<string, string, object>(key => key, key => args[key]);
            return CreateArgs(dictionary);
        }

        public string FilePath
        {
            get { return _configuration.FilePath; }
        }
    }
}

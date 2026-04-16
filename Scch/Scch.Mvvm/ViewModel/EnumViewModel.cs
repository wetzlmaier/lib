using System;
using Scch.Common.ComponentModel;
using Scch.Common.Globalization;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Viewmodel for <see cref="Enum"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumViewModel<T> : NotifyPropertyChanged
    {
        private Enum _enum;
        private T _value;
        private string _name;

        /// <summary>
        /// Gets or sets the enum.
        /// </summary>
        public Enum Enum
        {
            get
            {
                return _enum;
            }
            set
            {
                if (_enum == value)
                    return;

                _enum = value;
                RaisePropertyChanged(() => Enum);

                var localized= Translator.Translate(value);
                Name = localized ?? Enum.GetName(value.GetType(), value);
                Value = (T)((object)value);
            }
        }

        /// <summary>
        /// The value of the <see cref="Enum"/>.
        /// </summary>
        public T Value
        {
            get { return _value; }
            private set
            {
                if (Equals(_value, value))
                    return;

                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        /// <summary>
        /// The name of the <see cref="Enum"/>
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }
    }
}

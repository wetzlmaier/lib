using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using Scch.Common.Globalization;

namespace Scch.Common.ComponentModel
{
    [Serializable]
    public abstract class DataErrorInfo : NotifyPropertyChanged, IExtendedDataErrorInfo
    {
        /// <summary>
        /// Name of the <see cref="IsValid"/> property.
        /// </summary>
        public const string IsValidPropertyName = "IsValid";

        [NonSerialized]
        private readonly Dictionary<string, Func<DataErrorInfo, object>> _propertyGetters;
        [NonSerialized]
        private readonly Dictionary<string, ValidationAttribute[]> _validators;
        [NonSerialized]
        private bool? _isValid;

        protected DataErrorInfo()
        {
            _validators = GetType()
                .GetProperties()
                .Where(p => GetValidations(p).Length != 0)
                .ToDictionary(p => p.Name, GetValidations);

            _propertyGetters = GetType()
                .GetProperties()
                .Where(p => GetValidations(p).Length != 0)
                .ToDictionary(p => p.Name, GetValueGetter);
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        [Browsable(false)]
        public virtual string this[string propertyName]
        {
            get
            {
                if (_propertyGetters.ContainsKey(propertyName))
                {
                    var propertyValue = _propertyGetters[propertyName](this);
                    var errorMessages = _validators[propertyName]
                        .Where(v => !v.IsValid(propertyValue))
                        .Select(v => v.FormatErrorMessage(Translator.Translate(GetType(), propertyName))).ToArray();

                    return string.Join(Environment.NewLine, errorMessages);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        [Browsable(false)]
        public virtual string Error
        {
            get
            {
                var errors = from validator in _validators
                             from attribute in validator.Value
                             where !attribute.IsValid(_propertyGetters[validator.Key](this))
                             select attribute.FormatErrorMessage(Translator.Translate(GetType(), validator.Key));

                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        private ValidationAttribute[] GetValidations(PropertyInfo property)
        {
            return (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
        }

        private Func<DataErrorInfo, object> GetValueGetter(PropertyInfo property)
        {
            return viewmodel => property.GetValue(viewmodel, null);
        }

        protected override void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            base.RaisePropertyChanged(property);

            UpdateValid();
        }

        protected void UpdateValid()
        {
            IsValid = (string.IsNullOrEmpty(((IDataErrorInfo)this).Error));
        }

        [Browsable(false)]
        [XmlIgnore]
        public virtual bool IsValid
        {
            get
            {
                if (!_isValid.HasValue)
                    UpdateValid();

                return _isValid.HasValue && _isValid.Value;
            }
            protected set
            {
                if (_isValid == value)
                    return;

                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }
    }
}

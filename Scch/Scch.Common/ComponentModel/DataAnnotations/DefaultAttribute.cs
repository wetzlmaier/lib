using System;
using System.Globalization;

namespace Scch.Common.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Attribute that allows to specify a default value for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultAttribute : Attribute
    {
        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultAttribute(double defaultValueExpression)
            : this(defaultValueExpression.ToString(CultureInfo.InvariantCulture))
        {
        }

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultAttribute(long defaultValueExpression):this(defaultValueExpression.ToString(CultureInfo.InvariantCulture))
        {
        }

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultAttribute(decimal defaultValueExpression)
            : this(defaultValueExpression.ToString(CultureInfo.InvariantCulture))
        {
        }

        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultAttribute(string defaultValueExpression)
        {
            DefaultValueExpression = defaultValueExpression;
        }

        /// <summary>
        /// String expression to be used as default on database
        /// </summary>
        public string DefaultValueExpression { get; private set; }
    }
}

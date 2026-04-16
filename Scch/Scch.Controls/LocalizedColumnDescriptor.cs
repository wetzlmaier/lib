using System;
using System.Linq.Expressions;
using System.Windows.Controls;
using System.Windows.Data;
using Scch.Common.Globalization;
using Scch.Common.Linq.Expressions;

namespace Scch.Controls
{
    /// <summary>
    /// <see cref="ColumnDescriptor"/> with localization support.
    /// </summary>
    public class LocalizedColumnDescriptor : ColumnDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor"/> class.
        /// </summary>
        /// <param name="member">The item <see cref="Type"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        public LocalizedColumnDescriptor(Type member, string displayMember)
            : base(Localize(member, displayMember), displayMember)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor"/> class.
        /// </summary>
        /// <param name="member">The item <see cref="Type"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public LocalizedColumnDescriptor(Type member, string displayMember, IValueConverter converter)
            : base(Localize(member, displayMember), displayMember, converter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor"/> class.
        /// </summary>
        /// <param name="member">The item <see cref="Type"/>.</param>
        /// <param name="headerText">The text for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        public LocalizedColumnDescriptor(Type member, string headerText, string displayMember)
            : base(Localize(member, headerText), displayMember)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor"/> class.
        /// </summary>
        /// <param name="member">The item <see cref="Type"/>.</param>
        /// <param name="headerText">The text for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public LocalizedColumnDescriptor(Type member, string headerText, string displayMember, IValueConverter converter)
            : base(Localize(member, headerText), displayMember, converter)
        {
        }

        private static string Localize(Type member, string displayMember)
        {
            return Translator.Translate(member, displayMember);
        }
    }

    /// <summary>
    /// Strongly typed version of <see cref="LocalizedColumnDescriptor"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocalizedColumnDescriptor<T> : LocalizedColumnDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="displayMemberProperty">The display member property for the row values.</param>
        public LocalizedColumnDescriptor(Expression<Func<T, object>> displayMemberProperty)
            : this(ExpressionHelper.GetPropertyPath(displayMemberProperty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="headerTextProperty">The property for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMemberProperty">The display member property for the row values.</param>
        public LocalizedColumnDescriptor(Expression<Func<T, object>> headerTextProperty, Expression<Func<T, object>> displayMemberProperty)
            : this(ExpressionHelper.GetPropertyPath(headerTextProperty), ExpressionHelper.GetPropertyPath(displayMemberProperty))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="displayMember">The display member property for the row values.</param>
        public LocalizedColumnDescriptor(string displayMember)
            : base(typeof(T), displayMember)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="headerText">The text for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        public LocalizedColumnDescriptor(string headerText, string displayMember)
            : base(typeof(T), headerText, displayMember)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="displayMemberProperty">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public LocalizedColumnDescriptor(Expression<Func<T, object>> displayMemberProperty, IValueConverter converter)
            : this(ExpressionHelper.GetPropertyPath(displayMemberProperty), converter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="headerTextProperty">The property for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMemberProperty">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public LocalizedColumnDescriptor(Expression<Func<T, object>> headerTextProperty, Expression<Func<T, object>> displayMemberProperty, IValueConverter converter)
            : this(ExpressionHelper.GetPropertyPath(headerTextProperty), ExpressionHelper.GetPropertyPath(displayMemberProperty), converter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="displayMember">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public LocalizedColumnDescriptor(string displayMember, IValueConverter converter)
            : base(typeof(T), displayMember, converter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedColumnDescriptor{T}"/> class.
        /// </summary>
        /// <param name="headerText">The text for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public LocalizedColumnDescriptor(string headerText, string displayMember, IValueConverter converter)
            : base(typeof(T), headerText, displayMember, converter)
        {
        }
    }
}

using System.Windows.Controls;
using System.Windows.Data;

namespace Scch.Controls
{
    /// <summary>
    /// Columns for <see cref="ListView"/> attached by <see cref="GridViewColumns"/>.
    /// </summary>
    public class ColumnDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDescriptor"/> class.
        /// </summary>
        /// <param name="headerText">The text for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        public ColumnDescriptor(string headerText, string displayMember)
            : this(headerText, displayMember, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDescriptor"/> class.
        /// </summary>
        /// <param name="headerText">The text for <see cref="GridViewColumn.Header"/>.</param>
        /// <param name="displayMember">The display member property for the row values.</param>
        /// <param name="converter">The <see cref="IValueConverter"/> for the row value.</param>
        public ColumnDescriptor(string headerText, string displayMember, IValueConverter converter)
        {
            HeaderText = headerText;
            DisplayMember = displayMember;
            Converter = converter;
        }

        /// <summary>
        /// The text for <see cref="GridViewColumn.Header"/>.
        /// </summary>
        public string HeaderText { get; private set; }

        /// <summary>
        /// The display member property for the row values.
        /// </summary>
        public string DisplayMember { get; private set; }

        /// <summary>
        /// The <see cref="IValueConverter"/> for the row value.
        /// </summary>
        public IValueConverter Converter { get; private set; }
    }
}

using System;
using System.Windows;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Contains <see cref="Style"/> metadata.
    /// </summary>
    public class StyleInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleInfo"/> class. 
        /// </summary>
        public StyleInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleInfo"/> class. 
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="style">The <see cref="Style"/>.</param>
        public StyleInfo(string name, Type targetType, Style style)
        {
            Name = name;
            TargetType = targetType;
            Style = style;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public Type TargetType { get; set; }

        /// <summary>
        /// Gets the <see cref="Style"/>.
        /// </summary>
        public Style Style { get; set; }
    }
}

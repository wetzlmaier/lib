using System.Collections.Generic;

namespace Scch.Common.ComponentModel
{
    /// <summary>
    /// Provides the display names for properties.
    /// </summary>
    public interface IDisplayNamesProvider
    {
        /// <summary>
        /// The dictionary of available display names.
        /// </summary>
        IDictionary<string, string> DisplayNames { get; }
    }
}

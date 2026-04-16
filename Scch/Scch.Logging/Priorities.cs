using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Scch.Logging
{
    /// <summary>
    /// Priorities for the <see cref="LogEntry"/>.
    /// </summary>
    public enum Priorities
    {
        /// <summary>
        /// Lowest priority.
        /// </summary>
        Lowest = 0,
        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 10,
        /// <summary>
        /// Normal priority.
        /// </summary>
        Normal = 20,
        /// <summary>
        /// High priority.
        /// </summary>
        High = 30,
        /// <summary>
        /// Highest priority.
        /// </summary>
        Highest = 40
    };
}

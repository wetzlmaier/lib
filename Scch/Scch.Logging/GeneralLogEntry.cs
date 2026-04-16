using System.Diagnostics;

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.General"/> catrgory.
    /// </summary>
    public class GeneralLogEntry : LogEntryBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="GeneralLogEntry"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments.</param>
        public GeneralLogEntry(string format, params object[] arg)
            : this(string.Format(format, arg)) { }

        /// <summary>
        /// Creates a new instance of <see cref="GeneralLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public GeneralLogEntry(string message) : this(message, Priorities.Normal) { }

        /// <summary>
        /// Creates a new instance of <see cref="GeneralLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="priority">The priority of the message.</param>
        public GeneralLogEntry(string message, Priorities priority)
            : base("GeneralLogEntry", message, priority, TraceEventType.Information)
        {
            Categories.Add(Category.General);
        }
    }
}

using System.Diagnostics;
using Scch.Common.Reflecton;

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.Debug"/> catrgory.
    /// </summary>
    public class DebugLogEntry : LogEntryBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="DebugLogEntry"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments.</param>
        public DebugLogEntry(string format, params object[] arg)
            : base("DebugLogEntry", string.Format(format, arg), Priorities.Normal, TraceEventType.Information)
        {
            SetFields();
        }

        /// <summary>
        /// Creates a new instance of <see cref="DebugLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public DebugLogEntry(string message) : base("DebugLogEntry", message, Priorities.Normal, TraceEventType.Information)
        {
            SetFields();
        }

        /// <summary>
        /// Creates a new instance of <see cref="DebugLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="priority">The priority of the message.</param>
        public DebugLogEntry(string message, Priorities priority)
            : base("DebugLogEntry", message, priority, TraceEventType.Information)
        {
            SetFields();
        }

        void SetFields()
        {
            ManagedThreadName = DebugHelper.GetFullMethodName(3);
            Categories.Add(Category.Debug);
        }
    }
}

using System.Diagnostics;

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.Trace"/> catrgory.
    /// </summary>
    public class TraceLogEntry : LogEntryBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="TraceLogEntry"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments.</param>
        public TraceLogEntry(string format, params object[] arg)
            : this(string.Format(format, arg)) { }

        /// <summary>
        /// Creates a new instance of <see cref="TraceLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public TraceLogEntry(string message) : this(message, Priorities.Low) { }

        /// <summary>
        /// Creates a new instance of <see cref="TraceLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="priority">The priority of the message.</param>
        public TraceLogEntry(string message, Priorities priority)
            : base("TraceLogEntry", message, priority, TraceEventType.Information)
        {
            Categories.Add(Category.Trace);
        }

        /// <summary>
        /// Returns the callstack.
        /// </summary>
        public static string Callstack
        {
            get
            {
                var cache = new TraceEventCache();
                return cache.Callstack;
            }
        }
    }
}

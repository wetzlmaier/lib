using System;
using System.Diagnostics;

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.Exception"/> catrgory.
    /// </summary>
    public class ExceptionLogEntry : LogEntryBase
    {
         /// <summary>
        /// Creates a new instance of <see cref="ExceptionLogEntry"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments.</param>
        public ExceptionLogEntry(string format, params object[] arg)
            : this(string.Format(format, arg)) { }
        
        /// <summary>
        /// Creates a new instance of <see cref="ExceptionLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public ExceptionLogEntry(string message) : this(message, Priorities.High) { }

        /// <summary>
        /// Creates a new instance of <see cref="ExceptionLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="priority">The priority of the message.</param>
        public ExceptionLogEntry(string message, Priorities priority)
            : base("ExceptionLogEntry", message, priority, TraceEventType.Error)
        {
            Categories.Add(Category.Exception);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExceptionLogEntry"/>.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public ExceptionLogEntry(Exception ex)
            : this(ex.Message, Priorities.Highest)
        {
            Message=ExceptionHelper.LogMessage(ex);
        }
    }
}

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.Security"/> catrgory.
    /// </summary>
    public class SecurityLogEntry : ExceptionLogEntry
    {
        /// <summary>
        /// Creates a new instance of <see cref="SecurityLogEntry"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments.</param>
        public SecurityLogEntry(string format, params object[] arg)
            : base(string.Format(format, arg)) { }

        /// <summary>
        /// Creates a new instance of <see cref="SecurityLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public SecurityLogEntry(string message) : this(message, Priorities.High) { }

        /// <summary>
        /// Creates a new instance of <see cref="SecurityLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="priority">The priority of the message.</param>
        public SecurityLogEntry(string message, Priorities priority)
            : base(message, priority)
        {
            Categories.Add(Category.Security);
        }
    }
}

using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Scch.Logging
{
    /// <summary>
    /// Base class for logging entries.
    /// </summary>
    public abstract class LogEntryBase : LogEntry
    {
        /// <summary>
        /// Creates a new instance of <see cref="LogEntryBase"/>.
        /// </summary>
        /// <param name="title">The title of the message.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="priority">The priority of the message.</param>
        /// <param name="severity">The severity of the message.</param>
        protected LogEntryBase(string title, string message, Priorities priority, TraceEventType severity)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = "Default";
            }
            if (string.IsNullOrEmpty(message))
            {
                message = "Default";
            }

            Priority = (int)priority;
            Severity = severity;
            Message = message;
            Title = title;
            ManagedThreadName = Thread.CurrentThread.Name;
        }

        /// <summary>
        /// Adds an extended property value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected void AddExtendedProperty(string key, object value)
        {
            if (ExtendedProperties.ContainsKey(key))
                ExtendedProperties[key] = value;
            else
                ExtendedProperties.Add(key, value);
        }
    }
}

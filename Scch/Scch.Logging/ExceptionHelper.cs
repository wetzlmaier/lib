using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Scch.Logging
{
    /// <summary>
    /// Helper functions for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Returns the log message for the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The log message for the specified exception.</returns>
        public static string LogMessage(Exception exception)
        {
            return LogMessage(exception, Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// Returns the log message for the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="threadId">The thread id.</param>
        /// <returns>The log message for the specified exception.</returns>
        public static string LogMessage(Exception exception, int threadId)
        {
            var message = new StringBuilder(string.Empty);
            Exception inner = exception;

            var intent = new StringBuilder(string.Empty);
            while (inner != null)
            {
                message.AppendFormat(intent + "Zeitstempel = \"{0}\"" + Environment.NewLine, DateTime.Now);
                message.AppendFormat(intent + "Eine '{0}' Ausnahme ist aufgetreten." + Environment.NewLine, inner.GetType().FullName);

                foreach (PropertyInfo property in inner.GetType().GetProperties())
                {
                    if (property.Name != "InnerException")
                    {
                        string displayString;

                        object value=property.GetValue(inner, null);
                        if (value != null && value is IEnumerable && value.GetType() != typeof(string))
                        {
                            var sb = new StringBuilder();

                            if (value is IDictionary)
                            {
                                foreach (object key in ((IDictionary)value).Keys)
                                {
                                    object dictValue = ((IDictionary)value)[key];
                                    sb.AppendLine(key + " = " + dictValue == null ? "(null)" : dictValue.ToString());
                                }
                            }
                            else
                            {
                                foreach (object obj in (IEnumerable)value)
                                {
                                    sb.AppendLine(obj == null ? "(null)" : obj.ToString());
                                }
                            }

                            displayString = sb.ToString();
                        }
                        else
                        {
                            displayString = (value == null) ? "(null)" : value.ToString();
                        }

                        string[] lines = displayString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        message.Append(intent + property.Name + " = ");

                        if (lines.Length > 1)
                        {
                            message.AppendLine();
                        }

                        foreach (string line in lines)
                        {
                            message.AppendLine(intent + line);
                        }
                    }
                }

                message.AppendFormat(intent + "Thread ID = \"{0}\"" + Environment.NewLine, threadId);

                intent.Append("\t");
                inner = inner.InnerException;
            }

            return message.ToString();
        }
    }
}
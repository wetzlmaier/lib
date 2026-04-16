using System;
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Scch.Logging
{
    /// <summary>
    /// Facade for writing a log entry to one or more log sinks. 
    /// </summary>
    public static class Logger
    {
        private static bool initialized;

        /// <summary>
        /// Initializes the <see cref="Logger"/>.
        /// </summary>
        /// <param name="fileConfigSource"></param>
        public static void Initialize(string fileConfigSource = null)
        {
            if (initialized)
            {
                throw new InvalidOperationException("Already initialized.");
            }

            IConfigurationSource configurationSource = fileConfigSource == null ? ConfigurationSourceFactory.Create() : new FileConfigurationSource(fileConfigSource);

            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logWriterFactory.Create());
            initialized = true;
        }

        /*
        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(LogEntry)"/>
        /// </summary>
        /// <param name="log"></param>
        public static void Write(LogEntry log)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object)"/>
        /// </summary>
        /// <param name="message"></param>
        public static void Write(object message)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        public static void Write(object message, ICollection<string> categories)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, IDictionary<string, object>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        public static void Write(object message, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, properties);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public static void Write(object message, string category)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, IDictionary<string, object>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="properties"></param>
        public static void Write(object message, ICollection<string> categories, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, properties);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, int)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="priority"></param>
        public static void Write(object message, ICollection<string> categories, int priority)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, IDictionary<string, object>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="properties"></param>
        public static void Write(object message, string category, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, properties);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, int)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        public static void Write(object message, string category, int priority)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, int, IDictionary<string, object>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="priority"></param>
        /// <param name="properties"></param>
        public static void Write(object message, ICollection<string> categories, int priority, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, properties);
        }


        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, int, int)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, int, IDictionary<string, object>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="properties"></param>
        public static void Write(object message, string category, int priority, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, properties);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, int, int)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        public static void Write(object message, string category, int priority, int eventId)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, int, int, TraceEventType)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId, TraceEventType severity)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId, severity);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, int, int, TraceEventType)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        public static void Write(object message, string category, int priority, int eventId, TraceEventType severity)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, int, int, TraceEventType, string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        /// <param name="title"></param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId, TraceEventType severity, string title)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId, severity,
                                                                       title);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, int, int, TraceEventType, string)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        /// <param name="title"></param>
        public static void Write(object message, string category, int priority, int eventId, TraceEventType severity, string title)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity,
                                                                       title);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, ICollection<string>, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="categories"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        /// <param name="title"></param>
        /// <param name="properties"></param>
        public static void Write(object message, ICollection<string> categories, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, categories, priority, eventId, severity,
                                                                       title, properties);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string, int, int, TraceEventType, string, IDictionary<string, object>)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="eventId"></param>
        /// <param name="severity"></param>
        /// <param name="title"></param>
        /// <param name="properties"></param>
        public static void Write(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message, category, priority, eventId, severity,
                                                                       title, properties);
        }
        */
        public static void Write(LogEntryBase log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(DebugLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(ExceptionLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(GeneralLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(PerformanceLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(SecurityLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(TraceLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public static void Write(SqlLogEntry log)
        {
            if (!initialized)
                Initialize();

            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled"/>
        /// </summary>
        /// <returns></returns>
        public static bool IsLoggingEnabled()
        {
            if (!initialized)
                Initialize();

            return Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled();
        }
    }
}
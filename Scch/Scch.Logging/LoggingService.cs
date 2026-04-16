using System.ComponentModel.Composition;

namespace Scch.Logging
{
    [Export(typeof(ILoggingService))]

    public class LoggingService : ILoggingService
    {

        /// <summary>
        /// Initializes the <see cref="Logger"/>.
        /// </summary>
        /// <param name="fileConfigSource"></param>
        public void Initialize(string fileConfigSource = null)
        {
            Logger.Initialize(fileConfigSource);
        }

        public void Write(LogEntryBase log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(DebugLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(ExceptionLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(GeneralLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(PerformanceLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(SecurityLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(TraceLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        public void Write(SqlLogEntry log)
        {
            Logger.Write(log);
        }

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled"/>
        /// </summary>
        /// <returns></returns>
        public bool IsLoggingEnabled()
        {
            return Logger.IsLoggingEnabled();
        }
    }
}

namespace Scch.Logging
{
    interface ILoggingService
    {
        /// <summary>
        /// Initializes the <see cref="Logger"/>.
        /// </summary>
        /// <param name="fileConfigSource"></param>
        void Initialize(string fileConfigSource = null);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(DebugLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(ExceptionLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(GeneralLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(PerformanceLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(SecurityLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(TraceLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(object, string)"/>
        /// </summary>
        void Write(SqlLogEntry log);

        /// <summary>
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Logger.IsLoggingEnabled"/>
        /// </summary>
        /// <returns></returns>
        bool IsLoggingEnabled();
    }
}

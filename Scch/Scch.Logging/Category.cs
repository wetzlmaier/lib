using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Scch.Logging
{
    /// <summary>
    /// Categories for for the <see cref="LogEntry"/>
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Debug messages.
        /// </summary>
        public const string Debug = "Debug";
        /// <summary>
        /// Error messages.
        /// </summary>
        public const string Exception = "Exception";
        /// <summary>
        /// General messages.
        /// </summary>
        public const string General = "General";
        /// <summary>
        /// Performance messages.
        /// </summary>
        public const string Performance = "Performance";
        /// <summary>
        /// Security messages.
        /// </summary>
        public const string Security = "Security";
        /// <summary>
        /// Trace messages.
        /// </summary>
        public const string Trace = "Trace";
        /// <summary>
        /// Sql statements.
        /// </summary>
        public const string Sql = "Sql";
    }
}

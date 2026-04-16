using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using Scch.Common.Reflecton;

namespace Scch.Logging
{
    /// <summary>
    /// LogEntry for the <see cref="Category.Sql"/> catrgory.
    /// </summary>
    public class SqlLogEntry : LogEntryBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="SqlLogEntry"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="arg">The arguments.</param>
        public SqlLogEntry(string format, params object[] arg)
            : base("SqlLogEntry", string.Format(format, arg), Priorities.Normal, TraceEventType.Information)
        {
            SetFields();
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlLogEntry"/>.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public SqlLogEntry(string message) : base("SqlLogEntry", message, Priorities.Normal, TraceEventType.Information)
        {
            SetFields();
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlLogEntry"/>.
        /// </summary>
        /// <param name="cmd">The message to log.</param>
        public SqlLogEntry(SqlCommand cmd)
            : base("SqlLogEntry", FormatSql(cmd), Priorities.Normal, TraceEventType.Information)
        {
            SetFields();
        }

        void SetFields()
        {
            Categories.Add(Category.Sql);
            ManagedThreadName = DebugHelper.GetFullMethodName(3);
        }

        #region FormatSql
        /// <summary>
        /// Formats the <see cref="SqlCommand"/> with its <see cref="SqlParameter"/> as sql string.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        static string FormatSql(SqlCommand cmd)
        {
            if (cmd == null)
                return string.Empty;

            string declaration = "DECLARE {0} {1}" + Environment.NewLine + "SET {0} = '{2}'";

            return FormatSql(cmd, declaration);
        }

        /// <summary>
        /// Formats the <see cref="SqlCommand"/> with its <see cref="SqlParameter"/> as sql string.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="declaration">Variable declaration</param>
        /// <returns></returns>
        static string FormatSql(SqlCommand cmd, string declaration)
        {
            if (cmd == null)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (SqlParameter parameter in cmd.Parameters)
            {
                sb.AppendLine(string.Format(declaration, parameter.ParameterName,
                                            parameter.SqlDbType, parameter.SqlValue));
            }

            sb.AppendLine(cmd.CommandText);

            return sb.ToString();
        }
        #endregion FormatSql

    }
}

using System;

namespace Scch.DataAccess.EntityFramework
{
    /// <summary>
    /// Class that hold default information for consumption
    /// </summary>
    public class DefaultInfo : MetadataInfo
    {
        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="tableName">Table name for the default value</param>
        /// <param name="columnName">Column name for the default value</param>
        /// <param name="columnType">Type of the column.</param>
        /// <param name="defaultValueExpression">String expression to be used as default on database</param>
        public DefaultInfo(string tableName, string columnName, Type columnType, string defaultValueExpression):base(tableName, columnName)
        {
            ColumnType = columnType;
            DefaultValueExpression = defaultValueExpression;
        }

        /// <summary>
        /// Type of the column.
        /// </summary>
        public Type ColumnType { get; private set; }

        /// <summary>
        /// String expression to be used as default on database
        /// </summary>
        public string DefaultValueExpression { get; private set; }
    }
}

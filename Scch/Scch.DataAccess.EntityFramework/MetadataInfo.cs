namespace Scch.DataAccess.EntityFramework
{
    public class MetadataInfo
    {
        public MetadataInfo(string tableName, string columnName)
        {
            TableName = tableName;
            ColumnName = columnName;
        }

        /// <summary>
        /// Table name for the default value
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Column name for the default value
        /// </summary>
        public string ColumnName { get; private set; }
    }
}

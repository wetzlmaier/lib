namespace Scch.DataAccess.EntityFramework
{
    public class CheckInfo : MetadataInfo
    {
        /// <summary>
        /// New instance of the attribute
        /// </summary>
        /// <param name="tableName">Table name for the default value</param>
        /// <param name="columnName">Column name for the default value</param>
        /// <param name="checkValueExpression">String expression to be used as check expression</param>
        public CheckInfo(string tableName, string columnName, string checkValueExpression)
            : base(tableName, columnName)
        {
            CheckExpression = checkValueExpression;
        }

        /// <summary>
        /// String expression to be used as check expression
        /// </summary>
        public string CheckExpression { get; private set; }
    }
}

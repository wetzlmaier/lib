using Scch.Common.ComponentModel.DataAnnotations;

namespace Scch.DataAccess.EntityFramework
{
    /// <summary>
    /// Contains information about an index
    /// </summary>
    public class IndexInfo : MetadataInfo
    {
        /// <summary>
        /// Create new instance of the indexed attribute
        /// </summary>
        /// <param name="indexName">Name of the index</param>
        /// <param name="ordinalPosition">Position of the column in an index</param>
        /// <param name="columnName">Column name for the index</param>
        /// <param name="direction">Direction of the column sorting in an index</param>
        /// <param name="tableName">Table name for the index</param>
        /// <param name="isUnique">Uniqueness of the index.</param>
        public IndexInfo(string indexName, int ordinalPosition, string columnName, IndexDirection direction, string tableName, bool isUnique):base(tableName, columnName)
        {
            IndexName = indexName;
            OrdinalPoistion = ordinalPosition;
            Direction = direction;
            IsUnique = isUnique;
        }

        /// <summary>
        /// Position of the column in an index
        /// </summary>
        public int OrdinalPoistion { get; private set; }

        /// <summary>
        /// Direction of the column sorting in an index
        /// </summary>
        public IndexDirection Direction { get; private set; }

        /// <summary>
        /// Name of the index
        /// </summary>
        public string IndexName { get; private set; }

        /// <summary>
        /// Uniqueness of the index.
        /// </summary>
        public bool IsUnique { get; private set; }
    }
}

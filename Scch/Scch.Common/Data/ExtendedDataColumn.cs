using System;
using System.ComponentModel;
using System.Data;

namespace Scch.Common.Data
{
    /// <summary>
    /// <see cref="DataColumn"/> with some extensions.
    /// </summary>
    public class ExtendedDataColumn : DataColumn
    {
        #region Members

        #endregion Members

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="ExtendedDataColumn"/>.
        /// </summary>
        public ExtendedDataColumn()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExtendedDataColumn"/>.
        /// </summary>
        /// <param name="converter"><see cref="TypeConverter"/> for this column.</param>
        public ExtendedDataColumn(TypeConverter converter):this(converter, null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExtendedDataColumn"/>.
        /// </summary>
        /// <param name="converter"><see cref="TypeConverter"/> for this column.</param>
        /// <param name="columnName"><see cref="DataColumn.ColumnName"/></param>
        public ExtendedDataColumn(TypeConverter converter, string columnName)
            : this(converter, columnName, typeof(string))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExtendedDataColumn"/>.
        /// </summary>
        /// <param name="converter"><see cref="TypeConverter"/> for this column.</param>
        /// <param name="columnName"><see cref="DataColumn.ColumnName"/></param>
        /// <param name="dataType"><see cref="DataColumn.DataType"/></param>
        public ExtendedDataColumn(TypeConverter converter, string columnName, Type dataType)
            : this(converter, columnName, dataType, null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExtendedDataColumn"/>.
        /// </summary>
        /// <param name="converter"><see cref="TypeConverter"/> for this column.</param>
        /// <param name="columnName"><see cref="DataColumn.ColumnName"/></param>
        /// <param name="dataType"><see cref="DataColumn.DataType"/></param>
        /// <param name="expr"><see cref="DataColumn.Expression"/></param>
        public ExtendedDataColumn(TypeConverter converter, string columnName, Type dataType, String expr)
            : this(converter, columnName, dataType, expr, MappingType.Element)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExtendedDataColumn"/>.
        /// </summary>
        /// <param name="converter"><see cref="TypeConverter"/> for this column.</param>
        /// <param name="columnName"><see cref="DataColumn.ColumnName"/></param>
        /// <param name="dataType"><see cref="DataColumn.DataType"/></param>
        /// <param name="expr"><see cref="DataColumn.Expression"/></param>
        /// <param name="type"><see cref="DataColumn.ColumnMapping"/></param>
        public ExtendedDataColumn(TypeConverter converter, string columnName, Type dataType, String expr, MappingType type)
            : base(columnName, dataType, expr, type)
        {
            Converter = converter;
        }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// <see cref="TypeConverter"/> for this column.
        /// </summary>
        public TypeConverter Converter { get; set; }

        #endregion Properties
    }
}

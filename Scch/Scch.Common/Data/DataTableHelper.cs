using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Scch.Common.Reflecton;

namespace Scch.Common.Data
{
    /// <summary>
    /// Hepler functions for <see cref="DataTable"/>.
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// Name for the object reference column
        /// </summary>
        public const string ThisColumn = "this";

        public const char CsvDelimiter = ';';

        public const char TextDelimiter = '|';

        public const string DotDotDot = "...";

        /// <summary>
        /// Moves the <see cref="DataRow"/> to another <see cref="DataTable"/>.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="sourceRow">The source row.</param>
        /// <param name="destinationTable">The destination table.</param>
        /// <returns>The moved <see cref="DataRow"/>.</returns>
        public static DataRow MoveRow(DataTable sourceTable, DataRow sourceRow, DataTable destinationTable)
        {
            if (sourceTable == null)
                throw new ArgumentNullException(nameof(sourceTable));

            if (sourceRow == null)
                throw new ArgumentNullException(nameof(sourceRow));

            if (destinationTable == null)
                throw new ArgumentNullException(nameof(destinationTable));

            DataRow destinationRow = destinationTable.NewRow();
            destinationRow.ItemArray = sourceRow.ItemArray;
            sourceTable.Rows.Remove(sourceRow);
            destinationTable.Rows.Add(destinationRow);
            return destinationRow;
        }

        /// <summary>
        /// Moves the <see cref="DataRow"/> to another <see cref="DataTable"/>.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="sourceRows">The source row.</param>
        /// <param name="destinationTable">The destination table.</param>
        /// <returns></returns>
        public static void MoveRows(DataTable sourceTable, DataRowCollection sourceRows, DataTable destinationTable)
        {
            if (sourceTable == null)
                throw new ArgumentNullException(nameof(sourceTable));

            if (sourceRows == null)
                throw new ArgumentNullException(nameof(sourceRows));

            if (destinationTable == null)
                throw new ArgumentNullException(nameof(destinationTable));

            // make a copy of the collection for the move operation
            var rows = new ArrayList(sourceRows);

            foreach (DataRow sourceRow in rows)
            {
                MoveRow(sourceTable, sourceRow, destinationTable);
            }
        }

        #region DataTable mapping
        /// <summary>
        /// Creates a <see cref="DataTable"/> with a column for each property of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="properties">The names of the properties.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable CreateDataTable(Type type, params string[] properties)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var dt = new DataTable(type.Name);

            IDictionary<string, PropertyDescriptor> propertyDescriptors = PropertyHelper.GetPropertyDescriptors(type);

            if (properties == null || properties.Length == 0)
                properties = new List<string>(propertyDescriptors.Keys).ToArray();

            // sort the properties, so it's easier to use DataRowCollection.Add(object[])
            Array.Sort(properties);
            var columns = properties.Select(property => propertyDescriptors[property]).Select(pd => new ExtendedDataColumn(pd.Converter, pd.Name, PropertyHelper.GetRealPropertyType(pd))
            {
                Caption = pd.DisplayName
            }).Cast<DataColumn>().ToList();

            // do not convert the data object: converter==null
            columns.Add(new ExtendedDataColumn(null, ThisColumn, type));

            dt.Columns.AddRange(columns.ToArray());
            return dt;
        }

        /// <summary>
        /// Fills a <see cref="DataTable"/> created by <see cref="CreateDataTable(Type, string[])"/> with the property values of a collection of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <param name="coll">The <see cref="ICollection{T}"/> of object.</param>
        public static void FillDataTable<T>(DataTable dt, ICollection<T> coll)
        {
            FillDataTable(dt, coll, typeof(T));
        }

        /// <summary>
        /// Fills a <see cref="DataTable"/> created by <see cref="CreateDataTable(Type, string[])"/> with the property values of a collection of objects.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <param name="coll">The collection of objects.</param>
        /// <param name="type">The <see cref="Type"/> of the objects.</param>
        public static void FillDataTable(DataTable dt, IEnumerable coll, Type type)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (coll == null)
                throw new ArgumentNullException(nameof(coll));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            foreach (object obj in coll)
            {
                if (obj == null)
                    throw new ArgumentException("obj is null.");

                if (!type.IsInstanceOfType(obj))
                    throw new ArgumentException("Type mismatch.");

                DataRow row = CreateAndFillDataRow(dt, obj);
                dt.Rows.Add(row);
            }
        }

        /// <summary>
        /// Creates a <see cref="DataRow"/> for a <see cref="DataTable"/> and fills the columns with the objects properties.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <returns>The created <see cref="DataRow"/>.</returns>
        public static DataRow CreateDataRow(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            return dt.NewRow();
        }

        /// <summary>
        /// Creates a <see cref="DataRow"/> for a <see cref="DataTable"/> and fills the columns with the objects properties.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <param name="obj">The object.</param>
        /// <returns>The created <see cref="DataRow"/>.</returns>
        public static DataRow CreateAndFillDataRow(DataTable dt, object obj)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            DataRow row = CreateDataRow(dt);

            row[ThisColumn] = obj;
            FillDataRow(row, dt.Columns);

            return row;
        }

        /// <summary>
        /// fills the columns of the <see cref="DataRow"/> with the objects properties.
        /// </summary>
        /// <param name="row">The <see cref="DataRow"/>.</param>
        /// <param name="columns">The columns to fill.</param>
        public static void FillDataRow(DataRow row, DataColumnCollection columns)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            object obj = row[ThisColumn];

            foreach (DataColumn col in columns)
            {
                if (col.ColumnName != ThisColumn)
                {
                    // Todo: später einbauen
                    /*TypeConverter converter = null;
                    if (col is ExtendedDataColumn)
                        converter = ((ExtendedDataColumn) col).Converter;*/

                    object value = PropertyHelper.ReadPropertyValue(col.DataType, obj, col.ColumnName);
                    /*
                    if (converter!=null&&converter.CanConvertTo(typeof(string)))
                        row[col] = converter.ConvertToString(value);
                    else */
                    row[col] = value ?? DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Creates and fills the <see cref="DataTable"/> with the property values of a collection of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static DataTable CreateAndFillDataTable<T>(ICollection<T> coll)
        {
            if (coll == null)
                throw new ArgumentNullException(nameof(coll));

            return CreateAndFillDataTable(coll, typeof(T));
        }

        /// <summary>
        /// Creates and fills the <see cref="DataTable"/> with the property values of a collection of objects.
        /// </summary>
        /// <param name="objects">The collection of objects.</param>
        /// <param name="type">The <see cref="Type"/> of the objects.</param>
        /// <param name="properties">The names of the properties.</param>
        /// <returns></returns>
        public static DataTable CreateAndFillDataTable(IEnumerable objects, Type type, params string[] properties)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            DataTable dt = CreateDataTable(type, properties);

            FillDataTable(dt, objects, type);

            return dt;
        }

        /// <summary>
        /// Returns the objects from a <see cref="DataTable"/> created by <see cref="CreateDataTable(Type, string[])"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <returns>The objects from a <see cref="DataTable"/> created by <see cref="CreateDataTable(Type, string[])"/>.</returns>
        public static IList GetObjectsFromDataTable(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            var objects = new ArrayList();

            foreach (DataRow row in dt.Rows)
            {
                objects.Add(GetObjectFromDataRow(row));
            }

            return objects;
        }

        /// <summary>
        /// Returns the object from a <see cref="DataRow"/> created by <see cref="CreateDataTable(Type, string[])"/>.
        /// </summary>
        /// <param name="dr">The <see cref="DataRow"/>.</param>
        /// <returns>The objects from a <see cref="DataRow"/> created by <see cref="CreateDataTable(Type, string[])"/>.</returns>
        public static object GetObjectFromDataRow(DataRow dr)
        {
            if (dr == null)
                throw new ArgumentNullException(nameof(dr));

            return dr[ThisColumn];
        }

        /// <summary>
        /// Sets the object in a <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">The <see cref="DataRow"/>.</param>
        /// <param name="value">The object.</param>
        public static void SetObjectInDataRow(DataRow dr, object value)
        {
            if (dr == null)
                throw new ArgumentNullException(nameof(dr));

            dr[ThisColumn] = value;
        }

        /// <summary>
        /// Creates a <see cref="DataTable"/> with each property of <see cref="PropertyDescriptor"/> as a column.
        /// </summary>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable CreateDataTable()
        {
            DataTable dt = CreateDataTable(typeof(PropertyDescriptor));
            dt.PrimaryKey = new[] { dt.Columns["Name"] };
            return dt;
        }

        /// <summary>
        /// Fills each row of the <see cref="DataTable"/> with the property values of a <see cref="PropertyDescriptor"/> in the <see cref="PropertyDescriptorCollection"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <param name="coll">The <see cref="PropertyDescriptorCollection"/>.</param>
        public static void FillDataTable(DataTable dt, PropertyDescriptorCollection coll)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (coll == null)
                throw new ArgumentNullException(nameof(coll));

            FillDataTable(dt, coll, typeof(PropertyDescriptor));
        }

        /// <summary>
        /// Fills each row of the <see cref="DataTable"/> with the property values of a <see cref="PropertyDescriptor"/> in a <see cref="Type"/>.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        public static void FillDataTable(DataTable dt, Type type)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            FillDataTable(dt, TypeDescriptor.GetProperties(type));
        }

        /// <summary>
        /// Creates a <see cref="DataTable"/> and fills each row of the <see cref="DataTable"/> with the property values of a <see cref="PropertyDescriptor"/> in the <see cref="PropertyDescriptorCollection"/>.
        /// </summary>
        /// <param name="coll">The <see cref="PropertyDescriptorCollection"/>.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable CreateAndFillDataTable(PropertyDescriptorCollection coll)
        {
            if (coll == null)
                throw new ArgumentNullException(nameof(coll));

            DataTable dt = CreateDataTable();
            FillDataTable(dt, coll, typeof(PropertyDescriptor));
            return dt; //PropertyHelper.CreateAndFillDataTable(coll, typeof(PropertyDescriptor));
        }

        /// <summary>
        /// Creates a <see cref="DataTable"/> and fills each row of the <see cref="DataTable"/> with the property values of a <see cref="PropertyDescriptor"/> in a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable CreateAndFillDataTable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return CreateAndFillDataTable(TypeDescriptor.GetProperties(type));
        }

        #endregion DataTable mapping

        /// <summary>
        /// Creates a <see cref="DataTable"/> and fills it with the specified number of example objects.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="numberOfObjects">The number of example objects to create.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="DataTable"/> and filled with example objects.</returns>
        public static DataTable CreateAndFillExampleTable(Type type, int numberOfObjects = 1, IDictionary<Type, object> values = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (numberOfObjects < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfObjects));

            DataTable dt = CreateDataTable(type);

            var objects = new ArrayList();

            for (int i = 0; i < numberOfObjects; i++)
                objects.Add(TypeHelper.CreateExampleObject(type, values));

            try
            {
                // some properties may throw an exception
                FillDataTable(dt, objects, type);
            }
            catch (Exception)
            {
                // then add an empty row
                dt.Rows.Add(dt.NewRow());
            }

            return dt;
        }

        /// <summary>
        /// Saves the contents of a <see cref="DataTable"/> as CSV file.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <param name="fileName">The CSV file name.</param>
        public static void SaveAsCsvFile(DataTable dt, string fileName)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            File.WriteAllText(fileName, GenerateCsvHeader(dt) + GenerateCsvContent(dt));
        }

        /// <summary>
        /// Appends the contents of a <see cref="DataTable"/> to a CSV file.
        /// </summary>
        /// <param name="dt">The <see cref="DataTable"/>.</param>
        /// <param name="fileName">The CSV file name.</param>
        public static void AppendToCsvFile(DataTable dt, string fileName)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            File.AppendAllText(fileName, GenerateCsvContent(dt));
        }

        public static string GenerateCsvHeader(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            var headerBuilder = new StringBuilder();

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == ThisColumn)
                    continue;

                headerBuilder.Append(CsvDelimiter + "\"" + col.Caption + "\"");
            }

            if (headerBuilder.Length > 0)
                headerBuilder.Remove(0, 1);

            return headerBuilder + Environment.NewLine;
        }

        public static string GenerateCsvContent(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            var csvContent = new StringBuilder();

            foreach (DataRow row in dt.Rows)
            {
                var rowBuilder = new StringBuilder();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == ThisColumn)
                        continue;

                    object value = row[col];
                    var isNumber = PropertyHelper.IsNumber(col.DataType);

                    rowBuilder.Append(CsvDelimiter);
                    if (!isNumber)
                        rowBuilder.Append("\"");

                    rowBuilder.Append(Convert.IsDBNull(value) ? string.Empty : value.ToString());

                    if (!isNumber)
                        rowBuilder.Append("\"");
                }

                if (rowBuilder.Length > 0)
                    rowBuilder.Remove(0, 1);

                csvContent.AppendLine(rowBuilder.ToString());
            }

            return csvContent.ToString();
        }

        public static string GenerateHtmlTable(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            return "<table>" + Environment.NewLine + GenerateHtmlTableHeader(dt) + GenerateHtmlTableContent(dt) + "</table>" + Environment.NewLine;
        }

        public static string GenerateHtmlTableHeader(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            var headerBuilder = new StringBuilder("<tr>");

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == ThisColumn)
                    continue;

                headerBuilder.AppendLine("<th>" + col.Caption + "</th>");
            }

            headerBuilder.AppendLine("</tr>");
            return headerBuilder.ToString();
        }

        public static string GenerateHtmlTableContent(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            var htmlContent = new StringBuilder();

            foreach (DataRow row in dt.Rows)
            {
                htmlContent.AppendLine("<tr>");

                var rowBuilder = new StringBuilder();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == ThisColumn)
                        continue;

                    object value = row[col];
                    rowBuilder.Append("<td>");
                    rowBuilder.Append((Convert.IsDBNull(value) ? string.Empty : value.ToString()));
                    rowBuilder.AppendLine("</td>");
                }

                htmlContent.AppendLine(rowBuilder.ToString());
                htmlContent.AppendLine("</tr>");
            }

            return htmlContent.ToString();
        }

        /// <summary>
        /// Loads a <see cref="DataTable"/> from a CSV file.
        /// </summary>
        /// <param name="file">The CSV file name.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable LoadFromCsvFile(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (!file.Exists)
                throw new FileNotFoundException("File not found.", file.FullName);

            return LoadFromCsvFile(file, Encoding.UTF8);
        }

        /// <summary>
        /// Loads a <see cref="DataTable"/> from a CSV file.
        /// </summary>
        /// <param name="file">The CSV file name.</param>
        /// <param name="encoding">The <see cref="Encoding"/> of the file.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable LoadFromCsvFile(FileInfo file, Encoding encoding)
        {
            if (!file.Exists)
                throw new FileNotFoundException("File not found.", file.FullName);

            return LoadFromCsv(File.ReadAllText(file.FullName, encoding));
        }

        /// <summary>
        /// Loads a <see cref="DataTable"/> from CSV data.
        /// </summary>
        /// <param name="csv">The CSV data.</param>
        /// <returns>The <see cref="DataTable"/>.</returns>
        public static DataTable LoadFromCsv(string csv)
        {
            if (csv == null)
                throw new ArgumentNullException(nameof(csv));

            var dt = new DataTable();

            var lines = csv.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var header = lines[0].Split(CsvDelimiter);

            foreach (var column in header)
            {
                var columnName = column.Trim();

                if (columnName.StartsWith("\"") && columnName.EndsWith("\""))
                    columnName = columnName.Substring(1, columnName.Length - 2);

                dt.Columns.Add(columnName, typeof(string));
            }

            for (int i = 1; i < lines.Length; i++)
            {
                var row = dt.NewRow();

                var data = lines[i].Split(CsvDelimiter);
                for (int column = 0; column < data.Length && column < dt.Columns.Count; column++)
                {
                    data[column] = data[column].Trim();

                    if (data[column].StartsWith("\"") && data[column].EndsWith("\""))
                        data[column] = data[column].Substring(1, data[column].Length - 2);

                    row[column] = data[column];
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        public static string GenerateText(DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            var columnWidths = new int[dt.Columns.Count];

            for (int column = 0; column < dt.Columns.Count; column++)
            {
                var col = dt.Columns[column];
                columnWidths[column] = Math.Max(columnWidths[column], string.IsNullOrEmpty(col.Caption) ? col.ColumnName.Length : col.Caption.Length);
            }

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    columnWidths[column] = Math.Max(columnWidths[column], dt.Rows[row].ItemArray[column].ToString().Length);
                }
            }

            return GenerateText(dt, columnWidths);
        }

        public static string GenerateText(DataTable dt, int[] columnWidths)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (dt.Columns.Count != columnWidths.Length)
                throw new ArgumentOutOfRangeException(nameof(columnWidths));

            return GenerateTextHeader(dt, columnWidths) + GenerateTextContent(dt, columnWidths);
        }

        private static string FormatTextRow(string[] texts, int[] columnWidths)
        {
            if (texts.Length != columnWidths.Length)
                throw new ArgumentOutOfRangeException(nameof(columnWidths));

            var sb = new StringBuilder();

            for (int column = 0; column < texts.Length; column++)
            {
                var text = new StringBuilder(texts[column]);
                var width = columnWidths[column];

                if (text.Length > width)
                {
                    text.Remove(width - DotDotDot.Length, text.Length - width + DotDotDot.Length);
                    text.Append(DotDotDot);
                }

                for (int i = text.Length; i < width; i++)
                    text.Append(" ");

                sb.AppendFormat(" {0} {1}", TextDelimiter, text);
            }

            if (sb.Length > 0)
            {
                sb.Remove(0, 1);
                sb.AppendFormat(" {0}" + Environment.NewLine, TextDelimiter);
            }

            return sb.ToString();
        }

        public static string GenerateTextHeader(DataTable dt, int[] columnWidths)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (dt.Columns.Count != columnWidths.Length)
                throw new ArgumentOutOfRangeException(nameof(columnWidths));

            var texts = new List<string>();
            for (int column = 0; column < dt.Columns.Count; column++)
            {
                var col = dt.Columns[column];
                texts.Add(string.IsNullOrEmpty(col.Caption) ? col.ColumnName : col.Caption);
            }

            return FormatTextRow(texts.ToArray(), columnWidths);
        }

        public static string GenerateTextContent(DataTable dt, int[] columnWidths)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            if (dt.Columns.Count != columnWidths.Length)
                throw new ArgumentOutOfRangeException(nameof(columnWidths));

            var sb = new StringBuilder();

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                var texts = new List<string>();
                for (int column = 0; column < dt.Columns.Count; column++)
                {
                    texts.Add(dt.Rows[row].ItemArray[column].ToString());
                }

                sb.Append(FormatTextRow(texts.ToArray(), columnWidths));
            }

            return sb.ToString();
        }
    }
}

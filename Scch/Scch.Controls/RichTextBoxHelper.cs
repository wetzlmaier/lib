using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Scch.Controls
{
    /// <summary>
    /// Helper class for <see cref="RichTextBox"/>.
    /// </summary>
    public class RichTextBoxHelper
    {
        /// <summary>
        /// Updates a <see cref="Table"/> with a given number of rows and columns.
        /// </summary>
        /// <param name="table">The <see cref="Table"/>.</param>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="borderBrush">The <see cref="Brush"/> of the border.</param>
        /// <param name="borderThickness">The <see cref="Thickness"/> of the border.</param>
        /// <param name="dLineHeight">The width of the <see cref="TableColumn"/>.</param>
        /// <param name="tableType">The <see cref="TableType"/>.</param>
        /// <returns>The updated <see cref="Table"/>.</returns>
        public static Table UpdateTable(Table table, int rowCount, int columnCount, Brush borderBrush, Thickness borderThickness, double dLineHeight, TableType tableType)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (borderBrush == null)
                throw new ArgumentNullException("borderBrush");
            if (borderThickness == null)
                throw new ArgumentNullException("borderThickness");

            table.Tag = tableType;
            table.CellSpacing = 2;
            table.BorderBrush = borderBrush;
            table.BorderThickness = borderThickness;
            table.MouseEnter += TableMouseEnter;
            table.MouseLeave += TableMouseLeave;

            if (0 >= table.Columns.Count)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    var tableColumn = new TableColumn { Width = double.IsNaN(dLineHeight) ? GridLength.Auto : new GridLength(dLineHeight) };
                    table.Columns.Add(tableColumn);
                }
            }
            else
            {
                foreach (TableColumn tableColumn in table.Columns)
                {
                    tableColumn.Width = double.IsNaN(dLineHeight) ? GridLength.Auto : new GridLength(dLineHeight);
                }
            }

            foreach (TableRowGroup rowGroup in table.RowGroups)
            {
                foreach (TableRow row in rowGroup.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.BorderBrush = borderBrush;
                        cell.BorderThickness = borderThickness;
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Builds a <see cref="Table"/> with a given number of rows and columns.
        /// </summary>
        /// <param name="rowCount">The number of rows.</param>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="borderBrush">The <see cref="Brush"/> of the border.</param>
        /// <param name="borderThickness">The <see cref="Thickness"/> of the border.</param>
        /// <param name="dLineHeight">The width of the <see cref="TableColumn"/>.</param>
        /// <param name="tableType">The <see cref="TableType"/>.</param>
        /// <returns>The created <see cref="Table"/>.</returns>
        public static Table BuildTable(int rowCount, int columnCount, Brush borderBrush, Thickness borderThickness, double dLineHeight, TableType tableType)
        {
            if (borderBrush == null)
                throw new ArgumentNullException("borderBrush");
            if (borderThickness == null)
                throw new ArgumentNullException("borderThickness");

            var table = new Table
                            {
                                Tag = tableType,
                                CellSpacing = 2,
                                BorderBrush = borderBrush,
                                BorderThickness = borderThickness
                            };
            table.MouseEnter += TableMouseEnter;
            table.MouseLeave += TableMouseLeave;

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                var tableColumn = new TableColumn
                                      {
                                          Width = double.IsNaN(dLineHeight) ? GridLength.Auto : new GridLength(dLineHeight)
                                      };
                table.Columns.Add(tableColumn);
            }

            var rowGroup = new TableRowGroup();
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                TableRow row = BuildTableRow(columnCount, borderBrush, borderThickness, dLineHeight);
                rowGroup.Rows.Add(row);
            }
            table.RowGroups.Add(rowGroup);
            return table;
        }

        private static void TableMouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("table_MouseLeave");
            if (null != TableDragHandle.LastEnteredObjectTable)
            {
                Debug.WriteLine("table_MouseLeave - reset cursor");
                TableDragHandle.LastEnteredObjectTable.Cursor = Cursors.SizeWE;
                TableDragHandle.Reset();
                e.Handled = true;
            }
            Debug.WriteLine("table_MouseLeave - exit");
        }

        private static void TableMouseEnter(object sender, MouseEventArgs e)
        {
            var table = sender as Table;
            if (null != table)
            {
                TableDragHandle.LastEnteredObjectTable = sender as Table;
            }
        }

        private static void CellMouseEnter(object sender, MouseEventArgs e)
        {
            var tableCell = sender as TableCell;
            //Debug.WriteLine("cell_MouseEnter dirOver: " + tableCell.IsMouseDirectlyOver + ", "+ e.GetPosition(tableCell).X + " " + e.GetPosition(tableCell).Y);
            if (null != tableCell && tableCell.IsMouseDirectlyOver)
            {
                //Point point = e.GetPosition(tableCell);

                tableCell.Cursor = Cursors.SizeWE;
            }
        }

        private static void CellMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (null != TableDragHandle.LastEnteredObjectTable)
            {
                //TableDragHandle.LastEnteredObjectTable.Cursor = Cursors.Arrow;
                TableDragHandle.Reset();
                var tableCell = sender as TableCell;
                if (null != tableCell)
                {
                    tableCell.Focus();
                    tableCell.Cursor = Cursors.Arrow;
                }
            }
        }

        private static void CellMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != TableDragHandle.LastEnteredObjectTable)
            {
                try
                {
                    var tableCell = sender as TableCell;
                    if (null == tableCell || Cursors.SizeWE != tableCell.Cursor)
                    {
                        return;
                    }
                    var tableRow = tableCell.Parent as TableRow;
                    if (null == tableRow)
                    {
                        return;
                    }

                    TableDragHandle.RowIndex = tableRow.Cells.IndexOf(tableCell);
                    TableDragHandle.DragStartPoint = e.GetPosition(tableCell);
                    GridLength gridLength = TableDragHandle.LastEnteredObjectTable.Columns[TableDragHandle.RowIndex].Width;
                    if (GridLength.Auto == gridLength)
                    {
                        Debug.WriteLine("cell_MouseLeftButtonDown - width is auto - return ");
                        return;
                    }
                    TableDragHandle.InitialCellWidth = (GridLength.Auto == gridLength ? 100 : gridLength.Value);
                    e.Handled = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("### Exception: " + ex);
                }
            }
        }

        private static void CellMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                TableDragHandle.IsHandleValid)
            {
                try
                {
                    var tableCell = sender as TableCell;
                    if (null == tableCell)
                    {
                        return;
                    }
                    Point point = e.GetPosition(tableCell);
                    double dDistance = TableDragHandle.DragStartPoint.Value.X - point.X;
                    double dCurrLen = TableDragHandle.LastEnteredObjectTable.Columns[TableDragHandle.RowIndex].Width.Value;
                    double dNewSize = TableDragHandle.InitialCellWidth - dDistance;
                    TableDragHandle.LastEnteredObjectTable.Columns[TableDragHandle.RowIndex].Width = new GridLength(1 > dNewSize ? 1 : dNewSize);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("### Exception: " + ex);
                }
            }
        }

        /// <summary>
        /// Builds a <see cref="TableRow"/> with the given number of columns. 
        /// </summary>
        /// <param name="columnCount">The number of columns.</param>
        /// <param name="borderBrush">The border <see cref="Brush"/>.</param>
        /// <param name="borderThickness">The border <see cref="Thickness"/>.</param>
        /// <param name="dLineHeight">The width of the columns.</param>
        /// <returns>The created <see cref="TableRow"/>.</returns>
        public static TableRow BuildTableRow(int columnCount, Brush borderBrush, Thickness borderThickness, double dLineHeight)
        {
            if (borderBrush == null)
                throw new ArgumentNullException("borderBrush");
            if (borderThickness == null)
                throw new ArgumentNullException("borderThickness");

            var row = new TableRow();

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                TableCell cell = BuildTableCell(borderBrush, borderThickness, dLineHeight);
                row.Cells.Add(cell);
            }

            return row;
        }

        /// <summary>
        /// Builds a <see cref="TableCell"/>.
        /// </summary>
        /// <param name="borderBrush">The border <see cref="Brush"/>.</param>
        /// <param name="borderThickness">The border <see cref="Thickness"/>.</param>
        /// <param name="dLineHeight">The cell width.</param>
        /// <returns></returns>
        public static TableCell BuildTableCell(Brush borderBrush, Thickness borderThickness, double dLineHeight)
        {
            if (borderBrush == null)
                throw new ArgumentNullException("borderBrush");
            if (borderThickness == null)
                throw new ArgumentNullException("borderThickness");

            var cell = new TableCell(new Paragraph()) { BorderBrush = borderBrush, BorderThickness = borderThickness };

            cell.MouseLeftButtonDown += CellMouseLeftButtonDown;
            cell.MouseLeftButtonUp += CellMouseLeftButtonUp;
            cell.MouseMove += CellMouseMove;
            cell.MouseEnter += CellMouseEnter;
            return cell;
        }

        /// <summary>
        /// Returns true, if the given <see cref="TextPointer"/> has an ancestor of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <param name="ancestorType">The ancestor <see cref="Type"/>.</param>
        /// <returns>True, if the given <see cref="TextPointer"/> has an ancestor of the given <see cref="Type"/>.</returns>
        public static bool HasAncestor(TextPointer position, Type ancestorType)
        {
            if (position==null)
                throw new ArgumentNullException("position");
            if (ancestorType == null)
                throw new ArgumentNullException("ancestorType");

            return GetAncestor(position, ancestorType) != null;
        }

        /// <summary>
        /// Gets the ancestor at the given <see cref="TextPointer"/> of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <param name="ancestorType">The ancestor <see cref="Type"/>.</param>
        /// <returns>The ancestor at the given <see cref="TextPointer"/> of the given <see cref="Type"/>.</returns>
        public static TextElement GetAncestor(TextPointer position, Type ancestorType)
        {
            if (position == null)
                throw new ArgumentNullException("position");
            if (ancestorType == null)
                throw new ArgumentNullException("ancestorType");

            var parent = position.Parent as TextElement;
            while (parent != null)
            {
                if (ancestorType.IsInstanceOfType(parent))
                {
                    return parent;
                }
                parent = parent.Parent as TextElement;
            }
            return null;
        }

        /// <summary>
        /// Gets the ancestor <see cref="InlineUIContainer"/> at the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The ancestor <see cref="InlineUIContainer"/> at the given <see cref="TextPointer"/>.</returns>
        public static InlineUIContainer GetAncestorInline(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            InlineUIContainer cont = null;
            var parent = position.Parent as Paragraph;
            var run = position.Parent as Inline;

            if (parent != null)
            {
                foreach (Inline inline in parent.Inlines)
                {
                    if (inline is InlineUIContainer)
                    {
                        cont = inline as InlineUIContainer;
                        //cont = parent.Inlines.FirstInline as InlineUIContainer;
                    }
                    else
                    {
                        cont = GetInlineUICont(inline as Span);
                    }
                    if (null != cont)
                    {
                        break;
                    }
                }
            }
            else if (run != null && run.NextInline != null)
            {
                //while(run.NextInline != null)
                cont = GetInlineFromCollection(run.SiblingInlines);
            }
            return cont;
        }

        /// <summary>
        /// Gets the first <see cref="InlineUIContainer"/> from the given <see cref="IEnumerable{Inline}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{Inline}"/>.</param>
        /// <returns>The first <see cref="InlineUIContainer"/> from the given <see cref="IEnumerable{Inline}"/>.</returns>
        private static InlineUIContainer GetInlineFromCollection(IEnumerable<Inline> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            InlineUIContainer cont = null;

            foreach (Inline inline in collection)
            {
                cont = inline as InlineUIContainer ?? GetInlineUICont(inline as Span);

                if (null != cont)
                {
                    break;
                }
            }
            return cont;
        }

        private static InlineUIContainer GetInlineUICont(Span parent)
        {
            InlineUIContainer cont = null;
            if (null != parent)
            {
                foreach (Inline inline in parent.Inlines)
                {
                    var inlineUIContainer = inline as InlineUIContainer;
                    if (inlineUIContainer != null)
                    {
                        cont = inlineUIContainer;
                        return cont;
                    }
                    cont = GetInlineUICont(inline as Span);
                    if (null != cont)
                    {
                        break;
                    }
                }
            }
            return cont;
        }

        /// <summary>
        /// Gets the <see cref="Table"/> ancestor at the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The <see cref="Table"/> ancestor at the given <see cref="TextPointer"/>.</returns>
        public static Table GetTableAncestor(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            TextElement textElement = GetAncestor(position, typeof(Table));
            return textElement as Table;
        }

        /// <summary>
        /// Gets the <see cref="TableRow"/> ancestor at the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The <see cref="TableRow"/> ancestor at the given <see cref="TextPointer"/>.</returns>
        public static TableRow GetTableRowAncestor(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            TextElement textElement = GetAncestor(position, typeof(TableRow));
            return textElement as TableRow;
        }

        /// <summary>
        /// Gets the <see cref="InlineUIContainer"/> ancestor at the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The <see cref="InlineUIContainer"/> ancestor at the given <see cref="TextPointer"/>.</returns>
        public static InlineUIContainer GetInlineUIContainer(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            return GetAncestorInline(position);
        }

        /// <summary>
        /// Gets the <see cref="Image"/> ancestor at the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The <see cref="Image"/> ancestor at the given <see cref="TextPointer"/>.</returns>
        public static Image GetImageAncestor(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            InlineUIContainer inlineUIContainer = GetInlineUIContainer(position);
            return (null == inlineUIContainer ? null : inlineUIContainer.Child as Image);
        }

        /// <summary>
        /// Gets the <see cref="TableCell"/> ancestor at the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The <see cref="TableCell"/> ancestor at the given <see cref="TextPointer"/>.</returns>
        public static TableCell GetTableCellAncestor(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            TextElement textElement = GetAncestor(position, typeof(TableCell));
            return textElement as TableCell;
        }

        /// <summary>
        /// Gets the line number of the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The line number of the given <see cref="TextPointer"/>.</returns>
        public static int GetLineNumber(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            if (position == null)
                throw new ArgumentNullException("position");

            int lineNumber = 0;
            int linesMoved;
            do
            {
                position = position.GetLineStartPosition(-1, out linesMoved);
                lineNumber++;
            }
            while (linesMoved != 0);

            return lineNumber;
        }

        /// <summary>
        /// Gets the column number of the given <see cref="TextPointer"/>.
        /// </summary>
        /// <param name="position">The <see cref="TextPointer"/>.</param>
        /// <returns>The column number of the given <see cref="TextPointer"/>.</returns>
        public static int GetColumnNumber(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            int linesMoved;
            TextPointer lineStartPosition = position.GetLineStartPosition(0, out linesMoved);

            int columnNumber = 0;
            do
            {
                columnNumber++;
                position = position.GetNextInsertionPosition(LogicalDirection.Backward);
            }
            while (position != null && position.CompareTo(lineStartPosition) > 0);

            return columnNumber;
        }


        /// <summary>
        /// Returns a <see cref="TextRange"/> covering a word containing or following this <see cref="TextPointer"/>.
        /// </summary>
        /// <remarks>
        /// If this TextPointer is within a word or at start of word, the containing word range is returned.
        /// If this TextPointer is between two words, the following word range is returned.
        /// If this TextPointer is at trailing word boundary, the following word range is returned.
        /// </remarks>
        public static TextRange GetWordRange(TextPointer position)
        {
            if (position == null)
                throw new ArgumentNullException("position");

            TextRange wordRange = null;
            TextPointer wordStartPosition = null;

            // Go forward first, to find word end position.
            TextPointer wordEndPosition = GetPositionAtWordBoundary(position, /*wordBreakDirection*/LogicalDirection.Forward);

            if (wordEndPosition != null)
            {
                // Then travel backwards, to find word start position.
                wordStartPosition = GetPositionAtWordBoundary(wordEndPosition, /*wordBreakDirection*/LogicalDirection.Backward);
            }

            if (wordStartPosition != null)
            {
                wordRange = new TextRange(wordStartPosition, wordEndPosition);
            }

            return wordRange;
        }

        /// <summary>
        /// 1.  When wordBreakDirection = Forward, returns a position at the end of the word,
        ///     i.e. a position with a wordBreak character (space) following it.
        /// 2.  When wordBreakDirection = Backward, returns a position at the start of the word,
        ///     i.e. a position with a wordBreak character (space) preceeding it.
        /// 3.  Returns null when there is no workbreak in the requested direction.
        /// </summary>
        private static TextPointer GetPositionAtWordBoundary(TextPointer position, LogicalDirection wordBreakDirection)
        {
            if (!position.IsAtInsertionPosition)
            {
                position = position.GetInsertionPosition(wordBreakDirection);
            }

            TextPointer navigator = position;

            while (navigator != null && !IsPositionNextToWordBreak(navigator, wordBreakDirection))
            {
                navigator = navigator.GetNextInsertionPosition(wordBreakDirection);
            }

            return navigator;
        }

        // Helper for GetPositionAtWordBoundary.
        // Returns true when passed TextPointer is next to a wordBreak in requested direction.
        private static bool IsPositionNextToWordBreak(TextPointer position, LogicalDirection wordBreakDirection)
        {
            bool isAtWordBoundary = false;

            // Skip over any formatting.
            if (position.GetPointerContext(wordBreakDirection) != TextPointerContext.Text)
            {
                position = position.GetInsertionPosition(wordBreakDirection);
            }

            if (position.GetPointerContext(wordBreakDirection) == TextPointerContext.Text)
            {
                LogicalDirection oppositeDirection = (wordBreakDirection == LogicalDirection.Forward) ?
                    LogicalDirection.Backward : LogicalDirection.Forward;

                var runBuffer = new char[1];
                var oppositeRunBuffer = new char[1];

                position.GetTextInRun(wordBreakDirection, runBuffer, /*startIndex*/0, /*count*/1);
                position.GetTextInRun(oppositeDirection, oppositeRunBuffer, /*startIndex*/0, /*count*/1);

                if (runBuffer[0] == ' ' && oppositeRunBuffer[0] != ' ')
                {
                    isAtWordBoundary = true;
                }
            }
            else
            {
                // If we're not adjacent to text then we always want to consider this position a "word break". 
                // In practice, we're most likely next to an embedded object or a block boundary.
                isAtWordBoundary = true;
            }

            return isAtWordBoundary;
        }
    }
}

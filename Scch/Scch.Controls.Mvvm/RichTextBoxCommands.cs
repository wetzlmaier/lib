using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Scch.Common.Windows.Media.Imaging;
using Scch.Controls.Mvvm.ViewModel;
using Scch.Mvvm;

namespace Scch.Controls.Mvvm
{
    /// <summary>
    /// Predefines <see cref="ICommand"/> for <see cref="RichTextBox"/>.
    /// </summary>
    public static class RichTextBoxCommands
    {
        /// <summary>
        /// Inserts an <see cref="MediaTypeNames.Image"/>.
        /// </summary>
        public static ICommand InsertPictureCommand { get; private set; }
        /// <summary>
        /// Inserts a <see cref="Table"/>.
        /// </summary>
        public static ICommand InsertTableCommand { get; private set; }
        /// <summary>
        /// Inserts a horizontal line.
        /// </summary>
        public static ICommand InsertLineCommand { get; private set; }

        /// <summary>
        /// Inserts a <see cref="TableRow"/> below the current row.
        /// </summary>
        public static ICommand InsertRowBelowCommand { get; private set; }
        /// <summary>
        /// Inserts a <see cref="TableRow"/> above the current row.
        /// </summary>
        public static ICommand InsertRowAboveCommand { get; private set; }
        /// <summary>
        /// Inserts a <see cref="TableColumn"/> left to the current column.
        /// </summary>
        public static ICommand InsertColumnLeftCommand { get; private set; }
        /// <summary>
        /// Inserts a <see cref="TableColumn"/> right to the current column.
        /// </summary>
        public static ICommand InsertColumnRightCommand { get; private set; }
        /// <summary>
        /// Deletes the current <see cref="TableRow"/>.
        /// </summary>
        public static ICommand DeleteRowCommand { get; private set; }
        /// <summary>
        /// Deletes the current <see cref="TableColumn"/>.
        /// </summary>
        public static ICommand DeleteColumnCommand { get; private set; }
        /// <summary>
        /// Deletes the current <see cref="Table"/>.
        /// </summary>
        public static ICommand DeleteTableCommand { get; private set; }
        /// <summary>
        /// Shows the properties dialog for either <see cref="Table"/> or <see cref="Image"/>.
        /// </summary>
        public static ICommand EditPropertiesCommand { get; private set; }

        static RichTextBoxCommands()
        {
            InsertPictureCommand = new RoutedCommand("InsertPicture", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertPictureCommand, InsertPicture, CanInsertTableOrPicture));
            InsertTableCommand = new RoutedCommand("InsertTable", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertTableCommand, InsertTable, CanInsertTableOrPicture));
            InsertLineCommand = new RoutedCommand("InsertLine", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertLineCommand, InsertLine, CanInsertLine));

            InsertRowBelowCommand = new RoutedCommand("InsertRowBelow", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertRowBelowCommand, InsertRowBelow, CanExecuteTableCommand));
            InsertRowAboveCommand = new RoutedCommand("InsertRowAbove", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertRowAboveCommand, InsertRowAbove, CanExecuteTableCommand));
            InsertColumnLeftCommand = new RoutedCommand("InsertColumnLeft", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertColumnLeftCommand, InsertColumnLeft, CanExecuteTableCommand));
            InsertColumnRightCommand = new RoutedCommand("InsertColumnRight", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(InsertColumnRightCommand, InsertColumnRight, CanExecuteTableCommand));
            DeleteRowCommand = new RoutedCommand("DeleteRow", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(DeleteRowCommand, DeleteRow, CanExecuteTableCommand));
            DeleteColumnCommand = new RoutedCommand("DeleteColumn", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(DeleteColumnCommand, DeleteColumn, CanExecuteTableCommand));
            DeleteTableCommand = new RoutedCommand("DeleteTable", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(DeleteTableCommand, DeleteTable, CanExecuteTableCommand));

            EditPropertiesCommand = new RoutedCommand("EditProperties", typeof(RichTextBox));
            CommandManager.RegisterClassCommandBinding(typeof(RichTextBox), new CommandBinding(EditPropertiesCommand, EditProperties, CanEditPropertiesCommand));
        }

        private static void CanEditPropertiesCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;

            TextPointer textPosition = richTextBox.Selection.Start;

            e.CanExecute = ((null != RichTextBoxHelper.GetImageAncestor(textPosition) ||
                             null != RichTextBoxHelper.GetTableAncestor(textPosition)));
        }

        private static void EditProperties(object sender, ExecutedRoutedEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;

            TextPointer textPosition = richTextBox.Selection.Start;

            if (null != RichTextBoxHelper.GetImageAncestor(textPosition))
            {
                EditPictureProperties(richTextBox, e);
            }
            else if (null != RichTextBoxHelper.GetTableAncestor(textPosition))
            {
                EditTableProperties(richTextBox, e);
            }
        }

        private static void EditTableProperties(RichTextBox richTextBox, ExecutedRoutedEventArgs e)
        {
            /*
            TextPointer textPosition = richTextBox.Selection.Start;

            Table table = RichTextBoxHelper.GetTableAncestor(textPosition);
            if (null != table)
            {
                TablePropertiesDialog tablePropertiesDialog = new TablePropertiesDialog(table);
                tablePropertiesDialog.ShowDialog();
                if (true == tablePropertiesDialog.DialogResult)
                {
                    RichTextBoxHelper.UpdateTable(table,
                                        tablePropertiesDialog.Rows,
                                        tablePropertiesDialog.Columns,
                                        tablePropertiesDialog.TableBorderBrush,
                                        tablePropertiesDialog.TableBorderThickness,
                                        tablePropertiesDialog.TableCellWidth,
                                        tablePropertiesDialog.TableType);
                }
            }*/
        }

        private static void EditPictureProperties(RichTextBox richTextBox, ExecutedRoutedEventArgs e)
        {
            /*
            TextPointer insertionPosition = richTextBox.Selection.Start;

            Image image = RichTextBoxHelper.GetImageAncestor(insertionPosition);
            InlineUIContainer inlineUIContainer = RichTextBoxHelper.GetInlineUIContainer(insertionPosition);
            if (image != null)
            {
                ImagePropertiesDialog imageProperties = new ImagePropertiesDialog(image);
                imageProperties.ShowDialog();
                if (true == imageProperties.DialogResult)
                {
                    try
                    {
                        if (imageProperties.PictureSourceChanged)
                        {
                            AcmBitmapImageHolder acmBitmapImageHolder = new AcmBitmapImageHolder(image, new Uri(imageProperties.ImagePath));
                        }
                        if (!String.IsNullOrEmpty(imageProperties.ImageHyperlink))
                        {
                            //### Set hyperlink
                            image.Tag = imageProperties.ImageHyperlink;
                            Hyperlink hyperlink = new Hyperlink(inlineUIContainer, insertionPosition);
                            hyperlink.NavigateUri = new Uri(imageProperties.ImageHyperlink);
                        }
                        control.OnRichTextBox_TextChanged(control, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format("{0} ({1}) :\n\r{2}", Res.txtLoadPictureFailed, imageProperties.ImageHyperlink, ex), Res.txtEditorName);
                    }
                }
            }*/
        }

        private static void DeleteTable(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            Table table = RichTextBoxHelper.GetTableAncestor(insertionPosition);
            if (table != null && table.SiblingBlocks != null)
            {
                table.SiblingBlocks.Remove(table);
            }
        }

        private static void CanExecuteTableCommand(object target, CanExecuteRoutedEventArgs e)
        {
            var richTextBox = (RichTextBox)target;

            e.CanExecute = false;
            if (RichTextBoxHelper.HasAncestor(richTextBox.Selection.Start, typeof(TableCell)))
            {
                e.CanExecute = true;
            }
        }

        private static void DeleteColumn(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;

            TextPointer selectionStartPosition = richTextBox.Selection.Start;
            TableCell startTableCell = RichTextBoxHelper.GetTableCellAncestor(selectionStartPosition);
            if (startTableCell == null)
            {
                return;
            }

            var startTableRow = startTableCell.Parent as TableRow;
            if (startTableRow == null)
            {
                return;
            }

            var startTableRowGroup = startTableRow.Parent as TableRowGroup;
            if (startTableRowGroup == null)
            {
                return;
            }

            int startColumnIndex = startTableRow.Cells.IndexOf(startTableCell);
            int endColumnIndex = startColumnIndex;

            if (!richTextBox.Selection.IsEmpty)
            {
                TextPointer selectionEndPosition = richTextBox.Selection.End.GetNextInsertionPosition(LogicalDirection.Backward);
                TableCell endTableCell = RichTextBoxHelper.GetTableCellAncestor(selectionEndPosition);
                if (endTableCell == null)
                {
                    return;
                }
                TableRow endTableRow = RichTextBoxHelper.GetTableRowAncestor(selectionEndPosition);
                if (endTableRow == null)
                {
                    return;
                }
                var endTableRowGroup = endTableRow.Parent as TableRowGroup;
                if (startTableRowGroup != endTableRowGroup)
                {
                    return;
                }
                endColumnIndex = endTableRow.Cells.IndexOf(endTableCell);
            }

            using (richTextBox.DeclareChangeBlock())
            {
                foreach (TableRow t in startTableRowGroup.Rows)
                {
                    for (int j = startColumnIndex; j <= endColumnIndex; j++)
                    {
                        TableCellCollection cells = t.Cells;
                        TableCell cellToDelete = cells[startColumnIndex];
                        cells.Remove(cellToDelete);
                    }
                }

                if (startTableRow.Cells.Count == 0)
                {
                    var table = startTableRowGroup.Parent as Table;
                    if (table != null && table.SiblingBlocks != null)
                        table.SiblingBlocks.Remove(table);
                }
            }
        }

        private static void DeleteRow(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;

            TextPointer selectionStartPosition = richTextBox.Selection.Start;
            TableRow startTableRow = RichTextBoxHelper.GetTableRowAncestor(selectionStartPosition);
            if (startTableRow == null)
            {
                return;
            }

            var startTableRowGroup = startTableRow.Parent as TableRowGroup;
            if (startTableRowGroup == null)
            {
                return;
            }

            int startRowIndex = startTableRowGroup.Rows.IndexOf(startTableRow);
            int endRowIndex = startRowIndex;

            if (!richTextBox.Selection.IsEmpty)
            {
                TextPointer selectionEndPosition = richTextBox.Selection.End.GetNextInsertionPosition(LogicalDirection.Backward);
                TableRow endTableRow = RichTextBoxHelper.GetTableRowAncestor(selectionEndPosition);
                if (endTableRow == null)
                {
                    return;
                }
                var endTableRowGroup = endTableRow.Parent as TableRowGroup;
                if (startTableRowGroup != endTableRowGroup)
                {
                    return;
                }
                endRowIndex = endTableRowGroup.Rows.IndexOf(endTableRow);
            }

            using (richTextBox.DeclareChangeBlock())
            {
                for (int i = startRowIndex; i <= endRowIndex; i++)
                {
                    startTableRowGroup.Rows.RemoveAt(i);
                }

                if (startTableRowGroup.Rows.Count == 0)
                {
                    var table = startTableRowGroup.Parent as Table;
                    if (table != null && table.SiblingBlocks != null)
                        table.SiblingBlocks.Remove(table);
                }
            }
        }

        private static void InsertColumnRight(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            TableCell tableCell = RichTextBoxHelper.GetTableCellAncestor(insertionPosition);
            if (tableCell == null)
            {
                return;
            }

            var tableRow = tableCell.Parent as TableRow;
            if (tableRow == null)
            {
                return;
            }

            var tableRowGroup = tableRow.Parent as TableRowGroup;
            if (tableRowGroup == null)
            {
                return;
            }

            int columnIndex = tableRow.Cells.IndexOf(tableCell);
            using (richTextBox.DeclareChangeBlock())
            {
                foreach (TableRow row in tableRowGroup.Rows)
                {
                    TableCell newTableCell = RichTextBoxHelper.BuildTableCell(tableCell.BorderBrush, tableCell.BorderThickness, double.NaN);
                    row.Cells.Insert(columnIndex + 1, newTableCell);
                }
            }
        }

        private static void InsertColumnLeft(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            TableCell tableCell = RichTextBoxHelper.GetTableCellAncestor(insertionPosition);
            if (tableCell == null)
            {
                return;
            }

            var tableRow = tableCell.Parent as TableRow;
            if (tableRow == null)
            {
                return;
            }

            var tableRowGroup = tableRow.Parent as TableRowGroup;
            if (tableRowGroup == null)
            {
                return;
            }

            int columnIndex = tableRow.Cells.IndexOf(tableCell);
            using (richTextBox.DeclareChangeBlock())
            {
                foreach (TableRow row in tableRowGroup.Rows)
                {
                    TableCell newTableCell = RichTextBoxHelper.BuildTableCell(tableCell.BorderBrush, tableCell.BorderThickness, double.NaN);
                    row.Cells.Insert(columnIndex, newTableCell);
                }
            }
        }

        private static void InsertRowAbove(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;

            TextPointer insertionPosition = richTextBox.Selection.Start;
            TableRow tableRow = RichTextBoxHelper.GetTableRowAncestor(insertionPosition);
            if (tableRow == null)
            {
                return;
            }

            var tableRowGroup = tableRow.Parent as TableRowGroup;
            if (tableRowGroup == null)
            {
                return;
            }
            var table = (Table)tableRowGroup.Parent;

            int rowIndex = tableRowGroup.Rows.IndexOf(tableRow);
            TableRow newTableRow = RichTextBoxHelper.BuildTableRow(tableRow.Cells.Count,
                                                (null != table ? table.BorderBrush : Brushes.Black),
                                                (null != table ? table.BorderThickness : new Thickness(0.5, 0.5, 0.5, 0.5)),
                                                double.NaN);
            tableRowGroup.Rows.Insert(rowIndex, newTableRow);
        }

        private static void InsertRowBelow(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            TableRow tableRow = RichTextBoxHelper.GetTableRowAncestor(insertionPosition);
            if (tableRow == null)
            {
                return;
            }

            var tableRowGroup = tableRow.Parent as TableRowGroup;
            if (tableRowGroup == null)
            {
                return;
            }
            var table = (Table)tableRowGroup.Parent;

            int rowIndex = tableRowGroup.Rows.IndexOf(tableRow);
            TableRow newTableRow = RichTextBoxHelper.BuildTableRow(tableRow.Cells.Count,
                                                (null != table ? table.BorderBrush : Brushes.Black),
                                                (null != table ? table.BorderThickness : new Thickness(0.5, 0.5, 0.5, 0.5)),
                                                double.NaN);
            tableRowGroup.Rows.Insert(rowIndex + 1, newTableRow);
        }

        private static void CanInsertLine(object sender, CanExecuteRoutedEventArgs canExecuteRoutedEventArgs)
        {
            canExecuteRoutedEventArgs.CanExecute = true;
        }

        private static void InsertLine(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = String.Empty;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;

            var myPathFigure = new PathFigure { StartPoint = new Point(7, 1) };

            var myLineSegment = new LineSegment { Point = new Point(800, 1) };

            var myPathSegmentCollection = new PathSegmentCollection { myLineSegment };

            myPathFigure.Segments = myPathSegmentCollection;

            var myPathFigureCollection = new PathFigureCollection { myPathFigure };

            var myPathGeometry = new PathGeometry { Figures = myPathFigureCollection };

            var myPath = new System.Windows.Shapes.Path { Stroke = Brushes.Black, StrokeThickness = 1, Data = myPathGeometry };
            // myPath.Height = 1;

            //<Path  Height="10" Fill="Black" Stretch="Fill" Stroke="Black" StrokeThickness="1" Visibility="Visible" Data="M7,34 L390,34" />
            if (insertionPosition.Paragraph != null)
                insertionPosition.Paragraph.Inlines.Add(myPath);
        }

        private static void InsertTable(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = String.Empty;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;

            // Split current paragraph at insertion position
            insertionPosition = insertionPosition.InsertParagraphBreak();

            var viewModel = new TablePropertiesViewModel();
            var modalViewModel = new ModalReferenceEditor(viewModel).EditViewModel;
            var dialog = new ModalDialogManager
                             {
                                 DataContext = modalViewModel,
                                 Title = viewModel.DisplayName,
                                 Icon = BitmapSourceHelper.LoadBitmap(Properties.Resources.Table_32x32),
                                 WindowStartupLocation = WindowStartupLocation.CenterOwner,
                                 DialogResizeMode = ResizeMode.NoResize,
                                 DialogHeight = 500,
                                 DialogWidth = 500
                             };

            dialog.SetBinding(ModalDialogManager.IsOpenProperty, new Binding(ModalDialogManager.IsOpenProperty.Name) { Source = modalViewModel });
            dialog.SetBinding(ModalDialogManager.DialogResultProperty, new Binding(ModalDialogManager.DialogResultProperty.Name) { Source = modalViewModel });
            modalViewModel.IsOpen = true;

            if (dialog.DialogResult == true)
            {
                //Table table = Helper.BuildTable(2, 5);
                Table table = RichTextBoxHelper.BuildTable(viewModel.RowCount,
                    viewModel.ColumnCount,
                    viewModel.BorderBrush,
                    viewModel.BorderThickness,
                    viewModel.ColumnWidth,
                    TableType.FullBorder);
                //table.LineHeight = tablePropertiesDialog.CellWidth;
                //table.Margin = new Thickness(30,30,30,30);
                if (insertionPosition.Paragraph != null && insertionPosition.Paragraph.SiblingBlocks != null)
                    insertionPosition.Paragraph.SiblingBlocks.InsertBefore(insertionPosition.Paragraph, table);
            }
        }

        private static void CanInsertTableOrPicture(object sender, CanExecuteRoutedEventArgs executedRoutedEventArgs)
        {
            var richTextBox = (RichTextBox)sender;
            TextPointer insertionPosition = richTextBox.Selection.Start;

            // Disable tables inside lists and hyperlinks
            executedRoutedEventArgs.CanExecute = (!RichTextBoxHelper.HasAncestor(insertionPosition, typeof(List)) &&
                                                  !RichTextBoxHelper.HasAncestor(insertionPosition, typeof(Hyperlink)));
        }

        private static void InsertPicture(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            /*
            var richTextBox = (RichTextBox)sender;

            TextPointer textPosition = richTextBox.Selection.Start;

            if (null != RichTextBoxHelper.GetImageAncestor(textPosition))
            {
                EditPictureProperties(richTextBox, executedRoutedEventArgs);
                return;
            }

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = String.Empty;
            }

            TextPointer insertionPosition = richTextBox.Selection.Start;

            ImagePropertiesDialog imageProperties = new ImagePropertiesDialog(null);
            imageProperties.ShowDialog();

            if (true == imageProperties.DialogResult)
            {
                // Split current paragraph at insertion position
                insertionPosition = insertionPosition.InsertParagraphBreak();
                Paragraph paragraph = insertionPosition.Paragraph;

                AcmBitmapImageHolder acmBitmapImageHolder = new AcmBitmapImageHolder(new Uri(imageProperties.ImagePath));

                paragraph.Inlines.Add(acmBitmapImageHolder.Image);
            }*/
        }
    }
}

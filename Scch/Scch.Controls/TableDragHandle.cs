using System.Windows;
using System.Windows.Documents;

namespace Scch.Controls
{
    internal static class TableDragHandle
    {
        private static double _initialWidth = double.NaN;
        private static int _rowIndex = -1;

        public static void Reset()
        {
            LastEnteredObjectTable = null;
            DragStartPoint = null;
            InitialCellWidth = double.NaN;
            RowIndex = -1;
        }

        public static bool IsHandleValid
        {
            get
            {
                bool bRetVal =
                (null != LastEnteredObjectTable &&
                 null != DragStartPoint &&
                 !double.IsNaN(InitialCellWidth) &&
                 -1 != RowIndex);
                //Debug.WriteLine("IsHandleValid: " + bRetVal);
                return bRetVal;
            }
        }

        public static int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        public static double InitialCellWidth
        {
            get { return _initialWidth; }
            set { _initialWidth = value; }
        }

        public static Table LastEnteredObjectTable { get; set; }

        public static Point? DragStartPoint { get; set; }
    }
}

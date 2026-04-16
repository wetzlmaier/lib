using System.Windows;
using System.Windows.Media;
using Scch.Controls.Mvvm.ViewModel;

namespace Scch.Controls.Mvvm.Design
{
    public class TablePropertiesViewDesignData : TablePropertiesViewModel
    {
        public TablePropertiesViewDesignData()
        {
            AutoWidth = false;
            BorderBrush = Brushes.Red;
            BorderThickness=new Thickness(1.1);
            ColumnCount = 3;
            ColumnWidth = 100;
            RowCount = 3;
        }
    }
}

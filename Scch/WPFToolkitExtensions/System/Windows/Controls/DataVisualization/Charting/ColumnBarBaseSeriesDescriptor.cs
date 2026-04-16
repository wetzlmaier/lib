using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public abstract class ColumnBarBaseSeriesDescriptor<T, TSeries> : DataPointSingleSeriesWithAxesDescriptor<T> where T : ColumnBarBaseSeries<TSeries>, new() where TSeries : DataPoint, new()
    {
        protected ColumnBarBaseSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }
}

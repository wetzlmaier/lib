using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public abstract class DataPointSeriesWithAxesDescriptor<T> : DataPointSeriesDescriptor<T> where T : DataPointSeriesWithAxes, new()
    {
        protected DataPointSeriesWithAxesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }
}

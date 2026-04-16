using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public abstract class DataPointSingleSeriesWithAxesDescriptor<T> : DataPointSeriesWithAxesDescriptor<T> where T : DataPointSingleSeriesWithAxes, new()
    {
        protected DataPointSingleSeriesWithAxesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }
}

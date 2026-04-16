using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public class ScatterSeriesDescriptor: DataPointSingleSeriesWithAxesDescriptor<ScatterSeries>
    {
        public ScatterSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }
}

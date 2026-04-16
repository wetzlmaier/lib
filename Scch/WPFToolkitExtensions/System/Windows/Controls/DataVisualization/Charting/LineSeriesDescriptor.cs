using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public class LineSeriesDescriptor : LineAreaBaseSeriesDescriptor<LineSeries, LineDataPoint>
    {
        public LineSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }
}

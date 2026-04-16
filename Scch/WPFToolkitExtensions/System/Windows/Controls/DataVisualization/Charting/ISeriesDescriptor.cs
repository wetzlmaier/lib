using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public interface ISeriesDescriptor
    {
        Series CreateSeries();
    }
}

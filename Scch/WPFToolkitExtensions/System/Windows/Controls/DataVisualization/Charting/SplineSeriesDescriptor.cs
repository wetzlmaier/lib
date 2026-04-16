using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public class SplineSeriesDescriptor : LineAreaBaseSeriesDescriptor<SplineSeries, LineDataPoint>
    {
        public SplineSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled, double splineTension) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
            Init(splineTension);
        }

        private void Init(double splineTension)
        {
            SplineTension = splineTension;
        }

        public double SplineTension { get; private set; }

        public override SplineSeries CreateSeries()
        {
            var series = base.CreateSeries();
            series.SplineTension = SplineTension;
            return series;
        }
    }
}

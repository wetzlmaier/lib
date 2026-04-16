using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public class BubbleSeriesDescriptor : DataPointSingleSeriesWithAxesDescriptor<BubbleSeries>
    {
        public BubbleSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled, string sizeValuePath) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
            Init(sizeValuePath);
        }

        public void Init(string sizeValuePath)
        {
            SizeValuePath = sizeValuePath;
        }

        public string SizeValuePath { get; private set; }

        public override BubbleSeries CreateSeries()
        {
            var series = base.CreateSeries();
            return series;
        }
    }
}

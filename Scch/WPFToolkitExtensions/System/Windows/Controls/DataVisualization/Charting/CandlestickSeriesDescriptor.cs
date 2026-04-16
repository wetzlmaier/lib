using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public class CandlestickSeriesDescriptor : DataPointSingleSeriesWithAxesDescriptor<CandlestickSeries>
    {
        public CandlestickSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled, string openValuePath, string lowValuePath, string highValuePath, string closeValuePath) : base(title, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
            Init(openValuePath, lowValuePath, highValuePath, closeValuePath);
        }

        public void Init(string openValuePath, string lowValuePath, string highValuePath, string closeValuePath)
        {
            OpenValuePath = openValuePath;
            LowValuePath = lowValuePath;
            HighValuePath = highValuePath;
            CloseValuePath = closeValuePath;
        }

        public string OpenValuePath { get; private set; }
        public string LowValuePath { get; private set; }
        public string HighValuePath { get; private set; }
        public string CloseValuePath { get; private set; }

        public override CandlestickSeries CreateSeries()
        {
            var series = base.CreateSeries();

            series.OpenValueBinding = null;
            series.LowValueBinding = null;
            series.HighValueBinding = null;
            series.CloseValueBinding = null;
            return series;
        }
    }
}

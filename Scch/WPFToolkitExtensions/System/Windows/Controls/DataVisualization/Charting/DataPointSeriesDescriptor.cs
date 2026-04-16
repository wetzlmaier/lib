using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public abstract class DataPointSeriesDescriptor<T> : SeriesDescriptor<T> where T : DataPointSeries, new()
    {
        protected DataPointSeriesDescriptor(string title, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(title)
        {
            Init(itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled);
        }

        private void Init(string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled)
        {
            DependentValuePath = dependentValuePath;
            IndependentValuePath = independentValuePath;
            IsSelectionEnabled = isSelectionEnabled;
            ItemsSource = itemsSource;
        }

        public string DependentValuePath { get; private set; }
        public string IndependentValuePath { get; private set; }
        public bool IsSelectionEnabled { get; private set; }
        public string ItemsSource { get; private set; }

        public override T CreateSeries()
        {
            var series = base.CreateSeries();

            series.DependentValuePath = DependentValuePath;
            series.IndependentValuePath = IndependentValuePath;
            series.IsSelectionEnabled = IsSelectionEnabled;
            series.SetBinding(DataPointSeries.ItemsSourceProperty, ItemsSource);

            return series;
        }
    }
}

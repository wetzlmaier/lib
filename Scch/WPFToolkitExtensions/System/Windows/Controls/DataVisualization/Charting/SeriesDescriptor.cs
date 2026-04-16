using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public abstract class SeriesDescriptor<T> : ISeriesDescriptor where T : Series, new()
    {
        protected SeriesDescriptor(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public virtual T CreateSeries()
        {
            var series = new T { Title = Title };
            return series;
        }

        Series ISeriesDescriptor.CreateSeries()
        {
            return CreateSeries();
        }
    }
}

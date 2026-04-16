using System.Collections;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public static class ChartSeries
    {
        [AttachedPropertyBrowsableForType(typeof(Chart))]
        public static object GetSeriesSource(DependencyObject obj)
        {
            return obj.GetValue(SeriesSourceProperty);
        }

        public static void SetSeriesSource(DependencyObject obj, object value)
        {
            obj.SetValue(SeriesSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for SeriesSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeriesSourceProperty = DependencyProperty.RegisterAttached("SeriesSource", typeof(IEnumerable), typeof(ChartSeries),
                new UIPropertyMetadata(null, SeriesSourceChanged));

        private static void SeriesSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var chart = obj as Chart;
            if (chart != null)
            {
                chart.Series.Clear();

                if (e.NewValue != null)
                {
                    foreach (ISeriesDescriptor descriptor in (IEnumerable)e.NewValue)
                    {
                        var series = descriptor.CreateSeries();

                        chart.Series.Add(series);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public static class ChartHelper
    {
        #region SeriesSource

        public static readonly DependencyProperty SeriesSourceProperty =
            DependencyProperty.RegisterAttached("SeriesSource",
                typeof (IEnumerable),
                typeof (ChartHelper),
                new PropertyMetadata(SeriesSourceChanged));

        public static IEnumerable GetSeriesSource(DependencyObject d)
        {
            return (IEnumerable) d.GetValue(SeriesSourceProperty);
        }

        public static void SetSeriesSource(DependencyObject d, IEnumerable value)
        {
            d.SetValue(SeriesSourceProperty, value);
        }

        #endregion

        #region DependentValueBinding

        public static readonly DependencyProperty DependentValueBindingProperty =
            DependencyProperty.RegisterAttached("DependentValueBinding",
                typeof (string),
                typeof (ChartHelper),
                null);

        public static string GetDependentValueBinding(DependencyObject d)
        {
            return (string) d.GetValue(DependentValueBindingProperty);
        }

        public static void SetDependentValueBinding(DependencyObject d, string value)
        {
            d.SetValue(DependentValueBindingProperty, value);
        }

        #endregion

        #region IndependentValueBinding

        public static readonly DependencyProperty IndependentValueBindingProperty =
            DependencyProperty.RegisterAttached("IndependentValueBinding",
                typeof (string),
                typeof (ChartHelper),
                null);

        public static string GetIndependentValueBinding(DependencyObject d)
        {
            return (string) d.GetValue(IndependentValueBindingProperty);
        }

        public static void SetIndependentValueBinding(DependencyObject d, string value)
        {
            d.SetValue(IndependentValueBindingProperty, value);
        }

        #endregion

        #region Title

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached("Title",
                typeof (string),
                typeof (ChartHelper),
                null);

        public static string GetTitle(DependencyObject d)
        {
            return (string) d.GetValue(TitleProperty);
        }

        public static void SetTitle(DependencyObject d, string value)
        {
            d.SetValue(TitleProperty, value);
        }

        #endregion

        #region SeriesType

        public static readonly DependencyProperty SeriesTypeProperty =
            DependencyProperty.RegisterAttached("SeriesType",
                typeof (SeriesType),
                typeof (ChartHelper),
                new PropertyMetadata(SeriesType.Bar));

        public static SeriesType GetSeriesType(DependencyObject d)
        {
            return (SeriesType) d.GetValue(SeriesTypeProperty);
        }

        public static void SetSeriesType(DependencyObject d, SeriesType value)
        {
            d.SetValue(SeriesTypeProperty, value);
        }

        #endregion

        #region SeriesStyle

        public static readonly DependencyProperty SeriesStyleProperty =
            DependencyProperty.RegisterAttached("SeriesStyle",
                typeof (Style),
                typeof (ChartHelper),
                null);

        public static Style GetSeriesStyle(DependencyObject d)
        {
            return (Style) d.GetValue(SeriesStyleProperty);
        }

        public static void SetSeriesStyle(DependencyObject d, Style value)
        {
            d.SetValue(SeriesStyleProperty, value);
        }

        #endregion

        private static void SeriesSourceChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Chart))
            {
                throw new Exception("Series attached property only works on a Chart type");
            }

            var chart = (Chart) d;

            /* Clear out any old series in the chart */
            chart.Series.Clear();

            /* Get our collection of data we need for each series */
            var chartSeriesSource = e.NewValue as IEnumerable;

            if (chartSeriesSource == null)
                throw new Exception("The SeriesSource does not support IEnumerable");

            /* Loop over each collection of data */
            foreach (var dataSource in chartSeriesSource)
            {
                DataPointSeries series;

                /* Find out what type of series we want to use */
                var seriesType = GetSeriesType(chart);

                switch (seriesType)
                {
                    case SeriesType.Line:
                        series = new LineSeries();
                        break;
                    case SeriesType.Bar:
                        series = new BarSeries();
                        break;
                    case SeriesType.Column:
                        series = new ColumnSeries();
                        break;
                    case SeriesType.Pie:
                        series = new PieSeries();
                        break;
                    case SeriesType.Scatter:
                        series = new ScatterSeries();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                /* Get and set the style of the newly created series */
                var seriesStyle = GetSeriesStyle(chart);
                series.Style = seriesStyle;

                string titleBindingName = GetTitle(chart);

                if (!string.IsNullOrEmpty(titleBindingName))
                {
                    /* Do some binding of the Title property */
                    var titleBinding = new Binding(titleBindingName)
                    {
                        Source = series.Title,
                        Mode = BindingMode.TwoWay
                    };

                    series.SetBinding(Series.TitleProperty, titleBinding);
                }

                /* Setup the bindings configured in the attached properties */
                series.DependentValueBinding = new Binding(GetDependentValueBinding(chart));
                series.IndependentValueBinding = new Binding(GetIndependentValueBinding(chart));

                /*Set the ItemsSource property, which gives the data to the series to be rendered */
                series.ItemsSource = dataSource as IEnumerable;

                /* Add the series to the chart */
                chart.Series.Add(series);
            }
        }
    }
}
using System;
using Scch.Common.Globalization;

namespace WPFToolkitExtensions.System.Windows.Controls.DataVisualization.Charting
{
    public class LocalizedLineSeriesDescriptor : LineSeriesDescriptor
    {
        public LocalizedLineSeriesDescriptor(Type member, string displayMember, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(Translator.Translate(member, displayMember), itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }

    public class LocalizedLineSeriesDescriptor<T> : LocalizedLineSeriesDescriptor
    {
        public LocalizedLineSeriesDescriptor(string displayMember, string itemsSource, string dependentValuePath, string independentValuePath, bool isSelectionEnabled) : base(typeof(T), displayMember, itemsSource, dependentValuePath, independentValuePath, isSelectionEnabled)
        {
        }
    }
}

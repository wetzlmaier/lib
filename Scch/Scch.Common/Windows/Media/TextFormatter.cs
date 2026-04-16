using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scch.Common.Windows.Media
{
    public static class TextFormatter
    {
        public static FormattedText CreateFormattedText(string text, Control control)
        {
            return new FormattedText(text, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(control.FontFamily, control.FontStyle, control.FontWeight, control.FontStretch), control.FontSize, control.Foreground, VisualTreeHelper.GetDpi(control).PixelsPerDip);
        }
        /*
        public static FormattedText CreateFormattedText(string text, Typeface typeface, double fontSize, Brush foreground)
        {
            return new FormattedText(text, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                typeface, fontSize, foreground);
        }

        public static FormattedText CreateFormattedText(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize, Brush foreground)
        {
            return new FormattedText(text, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface(fontFamily, fontStyle, fontWeight, fontStretch), fontSize, foreground);
        }*/
    }
}

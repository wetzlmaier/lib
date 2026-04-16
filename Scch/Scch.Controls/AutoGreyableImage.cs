using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Scch.Controls
{
    public class AutoGreyableImage : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoGreyableImage"/> class.
        /// </summary>
        static AutoGreyableImage()
        {
            // Override the metadata of the IsEnabled property.
            IsEnabledProperty.OverrideMetadata(typeof(AutoGreyableImage), new FrameworkPropertyMetadata(true, OnAutoGreyScaleImageIsEnabledPropertyChanged));
            SourceProperty.OverrideMetadata(typeof(AutoGreyableImage), new FrameworkPropertyMetadata(null, OnAutoGreyScaleImageSourcePropertyChanged));
        }

        /// <summary>
        /// Necessary, if Image.IsEnabled property is set before Image.Source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private static void OnAutoGreyScaleImageSourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var autoGreyScaleImg = (AutoGreyableImage)source;

            if (args.NewValue != null)
            {
                if (!autoGreyScaleImg.IsEnabled && args.OldValue==null)
                {
                    var bitmapImage = (BitmapSource)autoGreyScaleImg.Source.Clone();

                    autoGreyScaleImg.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                    autoGreyScaleImg.OpacityMask = new ImageBrush(bitmapImage);
                }
            }
        }

        /// <summary>
        /// Called when [auto grey scale image is enabled property changed].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAutoGreyScaleImageIsEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            var autoGreyScaleImg = (AutoGreyableImage)source;
            var isEnable = Convert.ToBoolean(args.NewValue);

            if (autoGreyScaleImg.Source!=null)
            {
                if (!isEnable)
                {
                    var bitmapImage = (BitmapSource)autoGreyScaleImg.Source.Clone();

                    autoGreyScaleImg.Source = new FormatConvertedBitmap(bitmapImage, PixelFormats.Gray32Float, null, 0);
                    autoGreyScaleImg.OpacityMask = new ImageBrush(bitmapImage);
                }
                else
                {
                    autoGreyScaleImg.Source = ((FormatConvertedBitmap)autoGreyScaleImg.Source).Source;
                    autoGreyScaleImg.OpacityMask = null;
                }
            }
        }
    }
}

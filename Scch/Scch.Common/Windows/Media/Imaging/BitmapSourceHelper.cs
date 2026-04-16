using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Scch.Common.Windows.Media.Imaging
{
    /// <summary>
    /// Helper class for <see cref="BitmapSource"/>.
    /// </summary>
    public static class BitmapSourceHelper
    {
        /// <summary>
        /// Loads an <see cref="Image"/> and returns it as a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="source">The <see cref="Image"/>.</param>
        /// <returns>The <see cref="BitmapSource"/>.</returns>
        public static BitmapSource LoadImage(Image source)
        {
            if (source == null)
                return null;

            return LoadBitmap(new Bitmap(source));
        }

        /// <summary>
        /// Loads a <see cref="Bitmap"/> and returns it as a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="source">The <see cref="Bitmap"/>.</param>
        /// <returns>The <see cref="BitmapSource"/>.</returns>
        public static BitmapSource LoadBitmap(Bitmap source)
        {
            if (source == null)
                return null;

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// Saves the <see cref="BitmapSource"/> to a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="source">The <see cref="BitmapSource"/>.</param>
        /// <returns>The <see cref="Bitmap"/>.</returns>
        public static Bitmap SaveBitmap(BitmapSource source)
        {
            if (source == null)
                return null;

            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(source));
                enc.Save(outStream);
                var bitmap = new Bitmap(outStream);

                // return bitmap; <-- leads to problems, stream is closed/closing ...
                return new Bitmap(bitmap);
            }
        }

        /// <summary>
        /// Loads a <see cref="BitmapSource"/> from a file.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>The <see cref="BitmapSource"/>.</returns>
        public static BitmapSource LoadBitmap(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                return LoadBitmap(stream);
            }
        }

        /// <summary>
        /// Loads a <see cref="BitmapSource"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/>.</param>
        /// <returns>The <see cref="BitmapSource"/>.</returns>
        public static BitmapSource LoadBitmap(Stream stream)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();

            return image;
        }
    }
}


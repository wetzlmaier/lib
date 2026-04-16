using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Scch.Common.Drawing
{
    /// <summary>
    /// Helper class for <see cref="ImageList"/> and <see cref="Image"/>.
    /// </summary>
    public static class ImageHelper
    {
        /// <summary>
        /// Converts a base64 encoded string to an <see cref="Image"/>.
        /// </summary>
        /// <param name="base64">The base64 encoded string.</param>
        /// <returns>The <see cref="Image"/>.</returns>
        public static Image ImageFromBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return null;

            var stream = new MemoryStream(Convert.FromBase64String(base64));
            return Image.FromStream(stream);
        }

        /// <summary>
        /// Converts an image to a base64 encoded string.
        /// </summary>
        /// <param name="image">The <see cref="Image"/>.</param>
        /// <returns>The base64 encoded string.</returns>
        public static string ImageToBase64String(Image image)
        {
            if (image == null)
                return string.Empty;

            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return Convert.ToBase64String(stream.ToArray(), Base64FormattingOptions.None);
        }

        /// <summary>
        /// Adds an <see cref="Image"/> for the specified <see cref="Type"/> to the <see cref="ImageList"/> and returns the key under which the image can be found in the list.
        /// </summary>
        /// <param name="imageList">The <see cref="ImageList"/>.</param>
        /// <param name="type">The specified <see cref="Type"/>.</param>
        /// <param name="image">The <see cref="Image"/> to add.</param>
        /// <returns>Returns the key under which the image can be found in the list.</returns>
        public static string AddImage(ImageList imageList, Type type, Image image)
        {
            string imageKey = string.Empty;

            if (image != null)
            {
                imageKey = type.FullName;

                if (!imageList.Images.ContainsKey(imageKey))
                {
                    imageList.Images.Add(imageKey, image);
                }
            }

            return imageKey;
        }

        /// <summary>
        /// Updates an <see cref="Image"/> for the specified <see cref="Type"/> in the <see cref="ImageList"/> and returns the key under which the image can be found in the list.
        /// </summary>
        /// <param name="imageList">The <see cref="ImageList"/>.</param>
        /// <param name="type">The specified <see cref="Type"/>.</param>
        /// <param name="image">The <see cref="Image"/> to add.</param>
        /// <returns>Returns the key under which the image can be found in the list.</returns>
        public static string UpdateImage(ImageList imageList, Type type, Image image)
        {
            string imageKey = type.FullName;

            if (imageList.Images.ContainsKey(imageKey))
            {
                imageList.Images.RemoveByKey(imageKey);
            }

            return AddImage(imageList, type, image);
        }

        public static void Save(Image image, string fileName, ImageFormat format = null)
        {
            using (Bitmap bmp = new Bitmap(image))
            {
                if (format == null)
                    format = bmp.RawFormat;

                bmp.Save(fileName, format);
            }
        }

        public static double AggregateColorValues(Image image)
        {
            long sumColorValues = 0;

            using (var bitmap = new Bitmap(image))
            {
                var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                var pixelSize = data.PixelFormat == PixelFormat.Format32bppArgb ? 4 : 3;
                var padding = data.Stride - (data.Width * pixelSize);
                var bytes = new byte[data.Height * data.Stride];

                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

                var index = 0;

                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        var a = pixelSize == 3 ? 255 : bytes[index + 3];
                        var r = bytes[index + 2];
                        var g = bytes[index + 1];
                        var b = bytes[index];

                        sumColorValues = sumColorValues + r + g + b;
                        index += pixelSize;
                    }

                    index += padding;
                }

                return (double)sumColorValues / 255 / pixelSize / bitmap.Width / bitmap.Height;
            }
        }
    }
}
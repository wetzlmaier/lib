using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Scch.Common.Windows.Media.Imaging
{
    public abstract class BitmapSourceBase : BitmapSource
    {
        public override event EventHandler<ExceptionEventArgs> DecodeFailed;
        public override event EventHandler<DownloadProgressEventArgs> DownloadProgress;
        public override event EventHandler<ExceptionEventArgs> DownloadFailed;
        public override event EventHandler DownloadCompleted;

        private BitmapImage _bitmapSource;

        protected BitmapSourceBase()
        {
            BitmapSource = new BitmapImage();
        }

        protected virtual void OnDecodeFailed(ExceptionEventArgs e)
        {
            EventHandler<ExceptionEventArgs> decodeFailed = DecodeFailed;
            if (decodeFailed != null)
            {
                decodeFailed(this, e);
            }
        }

        protected virtual void OnDownloadCompleted()
        {
            EventHandler downloadCompleted = DownloadCompleted;
            if (downloadCompleted != null)
            {
                downloadCompleted(this, EventArgs.Empty);
            }
        }

        protected virtual void OnDownloadProgress(DownloadProgressEventArgs e)
        {
            EventHandler<DownloadProgressEventArgs> downloadProgress = DownloadProgress;
            if (downloadProgress != null)
            {
                downloadProgress(this, e);
            }
        }

        protected virtual void OnDownloadFailed(ExceptionEventArgs e)
        {
            EventHandler<ExceptionEventArgs> downloadFailed = DownloadFailed;
            if (downloadFailed != null)
            {
                downloadFailed(this, e);
            }
        }

        protected BitmapImage BitmapSource
        {
            get
            {
                return _bitmapSource;
            }
            set
            {
                if (BitmapSource != null && !BitmapSource.IsFrozen)
                {
                    BitmapSource.DownloadCompleted -= BitmapSourceDownloadCompleted;
                    BitmapSource.DownloadProgress -= BitmapSourceDownloadProgress;
                    BitmapSource.DownloadFailed -= BitmapSourceDownloadFailed;
                    BitmapSource.DecodeFailed -= BitmapSourceDecodeFailed;
                }

                _bitmapSource = value;

                if (BitmapSource != null && !BitmapSource.IsFrozen)
                {
                    BitmapSource.DownloadCompleted += BitmapSourceDownloadCompleted;
                    BitmapSource.DownloadProgress += BitmapSourceDownloadProgress;
                    BitmapSource.DownloadFailed += BitmapSourceDownloadFailed;
                    BitmapSource.DecodeFailed += BitmapSourceDecodeFailed;
                }
            }
        }

        private void BitmapSourceDecodeFailed(object sender, ExceptionEventArgs e)
        {
            OnDecodeFailed(e);
        }

        private void BitmapSourceDownloadFailed(object sender, ExceptionEventArgs e)
        {
            OnDownloadFailed(e);
        }

        private void BitmapSourceDownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            OnDownloadProgress(e);
        }

        private void BitmapSourceDownloadCompleted(object sender, EventArgs e)
        {
            OnDownloadCompleted();
        }

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType(), null);
        }

        public override PixelFormat Format
        {
            get
            {
                return BitmapSource.Format;
            }
        }

        public override int PixelHeight
        {
            get
            {
                return BitmapSource.PixelHeight;
            }
        }

        public override int PixelWidth
        {
            get
            {
                return BitmapSource.PixelWidth;
            }
        }

        public override double DpiX
        {
            get
            {
                return BitmapSource.DpiX;
            }
        }

        public override double DpiY
        {
            get
            {
                return BitmapSource.DpiY;
            }
        }

        public override BitmapPalette Palette
        {
            get
            {
                return BitmapSource.Palette;
            }
        }

        public override void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            BitmapSource.CopyPixels(sourceRect, pixels, stride, offset);
        }
    }
}

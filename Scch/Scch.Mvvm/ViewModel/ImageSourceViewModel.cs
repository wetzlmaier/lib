using System;
using System.Drawing;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using Scch.Common.Windows.Media.Imaging;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// <see cref="IViewModel"/> that provides an <see cref="ImageSource"/>.
    /// </summary>
    public abstract class ImageSourceViewModel : ViewModelBase, IImageSourceProvider
    {
        private ImageSource _imageSource;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        protected ImageSourceViewModel(string displayName)
            : base(displayName, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex)
            : base(displayName, displayIndex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="messenger"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, IMessenger messenger)
            : base(displayName, displayIndex, messenger)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="image"></param>
        protected ImageSourceViewModel(string displayName, Image image)
            : base(displayName)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            Initialize(BitmapSourceHelper.LoadImage(image));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="image"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, Image image)
            : base(displayName, displayIndex)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            Initialize(image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="messenger"></param>
        /// <param name="image"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, IMessenger messenger, Image image)
            : base(displayName, displayIndex, messenger)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            Initialize(image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="icon"></param>
        protected ImageSourceViewModel(string displayName, Icon icon)
            : base(displayName)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            Initialize(icon.ToBitmap());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="icon"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, Icon icon)
            : base(displayName, displayIndex)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            Initialize(icon.ToBitmap());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="messenger"></param>
        /// <param name="icon"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, IMessenger messenger, Icon icon)
            : base(displayName, displayIndex, messenger)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            Initialize(icon.ToBitmap());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="imageSource"></param>
        protected ImageSourceViewModel(string displayName, ImageSource imageSource)
            : base(displayName, 0)
        {
            if (imageSource == null)
                throw new ArgumentNullException("imageSource");

            Initialize(imageSource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="imageSource"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, ImageSource imageSource)
            : base(displayName, displayIndex)
        {
            if (imageSource == null)
                throw new ArgumentNullException("imageSource");

            Initialize(imageSource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceViewModel"/> class. 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="displayIndex"></param>
        /// <param name="messenger"></param>
        /// <param name="imageSource"></param>
        protected ImageSourceViewModel(string displayName, int displayIndex, IMessenger messenger, ImageSource imageSource)
            : base(displayName, displayIndex, messenger)
        {
            if (imageSource == null)
                throw new ArgumentNullException("imageSource");

            Initialize(imageSource);
        }

        private void Initialize(Image image)
        {
            Initialize(BitmapSourceHelper.LoadImage(image));
        }

        private void Initialize(ImageSource imageSource)
        {
            if (!imageSource.IsFrozen)
                imageSource.Freeze();

            _imageSource = imageSource;
        }

        /// <summary>
        /// The <see cref="ImageSource"/>.
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                if (_imageSource == value)
                    return;

                _imageSource = value;
                RaisePropertyChanged(() => ImageSource);
            }
        }
    }
}

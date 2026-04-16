using System.Collections;
using System.Drawing;
using System.Windows.Media;
using Scch.Common.Collections.ObjectModel;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// <see cref="IViewModel"/> for <see cref="System.Windows.Controls.TreeViewItem"/>.
    /// </summary>
    public class TreeViewItemViewModel<T> : ImageSourceViewModel, ITreeViewItemViewModel where T : TreeViewItemViewModel<T>
    {
        private bool _isExpanded;
        private bool _isSelected;
        private ThreadSafeObservableCollection<T> _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="displayName"></param>
        public TreeViewItemViewModel(string displayName)
            : base(displayName)
        {
            _items = new ThreadSafeObservableCollection<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="image"></param>
        public TreeViewItemViewModel(string displayName, Image image)
            : base(displayName, image)
        {
            _items = new ThreadSafeObservableCollection<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="icon"></param>
        public TreeViewItemViewModel(string displayName, Icon icon)
            : base(displayName, icon.ToBitmap())
        {
            _items = new ThreadSafeObservableCollection<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="imageSource"></param>
        public TreeViewItemViewModel(string displayName, ImageSource imageSource)
            : base(displayName, imageSource)
        {
            _items = new ThreadSafeObservableCollection<T>();
        }

        /// <summary>
        /// <see cref="System.Windows.Controls.TreeViewItem.IsExpanded"/>
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                    return;

                _isExpanded = value;
                RaisePropertyChanged(() => IsExpanded);
            }
        }

        /// <summary>
        /// <see cref="System.Windows.Controls.TreeViewItem.IsSelected"/>
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        IList ITreeViewItemViewModel.Items
        {
            get { return Items; } 
        }

        /// <summary>
        /// <see cref="System.Windows.Controls.ItemsControl.Items"/>
        /// </summary>
        public ThreadSafeObservableCollection<T> Items
        {
            get { return _items; }
            set
            {
                if (_items == value)
                    return;

                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }
    }
}

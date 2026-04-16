using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Scch.Common.Windows.Media.Imaging;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Interface for <see cref="DataTemplate"/> that use <see cref="TreeViewItemViewModel{T}"/>.
    /// </summary>
    public interface ITreeViewItemViewModel : IImageSourceProvider, IViewModel
    {
        /// <summary>
        /// <see cref="TreeViewItem.IsExpanded"/>
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// <see cref="TreeViewItem.IsSelected"/>
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// <see cref="ItemsControl.Items"/>
        /// </summary>
        IList Items { get; }
    }
}

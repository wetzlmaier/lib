using System.Windows;
using System.Windows.Input;
using Scch.Common.Windows.Media.Imaging;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Interface for command based viewmodels
    /// </summary>
    public interface ICommandViewModel : IImageSourceProvider
    {
        /// <summary>
        /// The tooltip
        /// </summary>
        string ToolTip { get; set; }

        /// <summary>
        /// <see cref="Visibility"/> of the <see cref="DisplayName"/>.
        /// </summary>
        Visibility TextVisibility { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ICommand"/>.
        /// </summary>
        ICommand Command { get; }

        /// <summary>
        /// The name to display.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets or sets the activatable flag.
        /// </summary>
        bool IsActivatable { get; }

        /// <summary>
        /// Gets or sets the active flag.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// The shortcut.
        /// </summary>
        KeyGesture Shortcut { get; set; }
    }
}

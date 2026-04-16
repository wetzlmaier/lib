using System.Windows.Media;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Interface extension for <see cref="IViewModel"/> for the main view.
    /// </summary>
    public interface IMainViewModel : IViewModel
    {
        /// <summary>
        /// Gets the Icon for the main window.
        /// </summary>
        ImageSource Icon { get; }

        void CloseWindow();

        void Shutdown(int exitCode);
    }
}

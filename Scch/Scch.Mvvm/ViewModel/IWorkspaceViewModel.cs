using Scch.Common.Windows.Media.Imaging;

namespace Scch.Mvvm.ViewModel
{
    /// <summary>
    /// Interface for workspace <see cref="IViewModel"/>
    /// </summary>
    public interface IWorkspaceViewModel : IViewModel, IImageSourceProvider
    {
        /// <summary>
        /// Returns the command that, when invoked, attempts to remove this workspace from the user interface.
        /// </summary>
        ICommandViewModel CloseCommand { get; }

        /// <summary>
        /// Returns the command that, when invoked, refreshes the workspace.
        /// </summary>
        ICommandViewModel RefreshCommand { get; }
    }
}

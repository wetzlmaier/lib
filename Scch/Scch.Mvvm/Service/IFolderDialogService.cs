using System;
using System.Windows;

namespace Scch.Mvvm.Service
{
    /// <summary>
    /// This service allows a user to select a folder.
    /// </summary>
    public interface IFolderDialogService
    {
        /// <summary>
        /// Shows the select folder dialog.
        /// </summary>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="rootFolder">The root folder where the browsing starts from.</param>
        /// <param name="selectedPath">The path preselected by the user.</param>
        /// <param name="browseFiles">A value indicating whether browsing files is allowed.</param>
        /// <param name="browseShares">A value indicating whether browsing shares is allowed.</param>
        /// <param name="showEditBox">A value indicating whether to show an edit box.</param>
        /// <param name="showStatusText">A value indicating whether to show status text.</param>
        /// <returns>The path selected by the user.</returns>
        FolderDialogResult ShowSelectFolderDialog(Window owner, Environment.SpecialFolder rootFolder, string selectedPath, bool browseFiles, bool browseShares, bool showEditBox, bool showStatusText);
    }
}

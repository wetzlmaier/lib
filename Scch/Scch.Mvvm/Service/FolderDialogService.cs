using System;
using System.ComponentModel.Composition;
using System.Windows;
using Scch.Common.Windows;
using Scch.Mvvm.Properties;

namespace Scch.Mvvm.Service
{
    /// <summary>
    /// This service allows a user to select a folder.
    /// </summary>
    [Export(typeof(IFolderDialogService))]
    public class FolderDialogService : IFolderDialogService
    {
        private readonly FolderBrowserDialog _dialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderDialogService"/> class.
        /// </summary>
        public FolderDialogService()
        {
            _dialog = new FolderBrowserDialog();
        }

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
        public FolderDialogResult ShowSelectFolderDialog(Window owner, Environment.SpecialFolder rootFolder, string selectedPath, bool browseFiles, bool browseShares, bool showEditBox, bool showStatusText)
        {
            if (selectedPath == null) { throw new ArgumentNullException("selectedPath"); }

            _dialog.SelectedPath = selectedPath;
            _dialog.BrowseFiles = browseFiles;
            _dialog.BrowseShares = browseShares;
            _dialog.ShowEditBox = showEditBox;
            _dialog.ShowStatusText = showStatusText;
            _dialog.Title = Resources.FolderDialogService_Title;
            _dialog.ValidateResult = true;

            if (_dialog.ShowDialog(owner) == true)
            {
                return new FolderDialogResult(_dialog.SelectedPath);
            }

            return null;
        }
    }
}

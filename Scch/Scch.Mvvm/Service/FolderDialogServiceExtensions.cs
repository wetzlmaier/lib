using System;
using System.Windows;
using Scch.Common.Windows;

namespace Scch.Mvvm.Service
{
    /// <summary>
    /// Provides method overloads for the <see cref="IFolderDialogService"/> to simplify its usage.
    /// </summary>
    public static class FolderDialogServiceExtensions
    {
        /// <summary>
        /// Shows the select folder dialog.
        /// </summary>
        /// <param name="service">The folder dialog service.</param>
        /// <param name="owner">The window that owns this OpenFileDialog.</param>
        /// <param name="selectedPath">The path preselected by the user.</param>
        /// <returns>The path selected by the user.</returns>
        public static FolderDialogResult ShowSelectFolderDialog(this IFolderDialogService service, Window owner, string selectedPath)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            if (selectedPath == null) { throw new ArgumentNullException("selectedPath"); }
            return service.ShowSelectFolderDialog(owner, Environment.SpecialFolder.Desktop, selectedPath, false, true, true, true);
        }

        /// <summary>
        /// Shows the select folder dialog.
        /// </summary>
        /// <param name="service">The folder dialog service.</param>
        /// <param name="selectedPath">The path preselected by the user.</param>
        /// <returns>The path selected by the user.</returns>
        public static FolderDialogResult ShowSelectFolderDialog(this IFolderDialogService service, string selectedPath)
        {
            return ShowSelectFolderDialog(service, WindowHelper.GetActiveWindow(), selectedPath);
        }
    }
}

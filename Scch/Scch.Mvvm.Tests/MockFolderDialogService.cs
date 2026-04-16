using System;
using System.Windows;
using Scch.Mvvm.Service;

namespace Scch.Mvvm.Tests
{
    public class MockFolderDialogService : IFolderDialogService
    {
        public Window Owner { get; set; }
        public Environment.SpecialFolder RootFolder { get; set; }
        public FolderDialogResult DialogResult { get; set; }
        public bool BrowseFiles { get; set; }
        public bool BrowseShares { get; set; }
        public bool ShowEditBox { get; set; }
        public bool ShowStatusText { get; set; }
        public string SelectedPath { get; set; }

        public FolderDialogResult ShowSelectFolderDialog(Window owner, Environment.SpecialFolder rootFolder, string selectedPath, bool browseFiles, bool browseShares, bool showEditBox, bool showStatusText)
        {
            Owner = owner;
            RootFolder = Environment.SpecialFolder.UserProfile;
            SelectedPath = selectedPath;
            BrowseFiles = browseFiles;
            BrowseShares = browseShares;
            ShowEditBox = showEditBox;
            ShowStatusText = showStatusText;

            return DialogResult;
        }
    }
}

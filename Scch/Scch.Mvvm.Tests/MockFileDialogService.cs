using System.Collections.Generic;
using System.Windows;
using Scch.Mvvm.Service;

namespace Scch.Mvvm.Tests
{
    public class MockFileDialogService : IFileDialogService
    {
        public Window Owner { get; set; }
        public IEnumerable<FileType> FileTypes { get; set; }
        public FileType DefaultFileType { get; set; }
        public string DefaultFileName { get; set; }
        public FileDialogResult DialogResult { get; set; }

        public FileDialogResult ShowOpenFileDialog(Window owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            Owner = owner;
            FileTypes = fileTypes;
            DefaultFileType = defaultFileType;
            DefaultFileName = defaultFileName;

            return DialogResult;
        }

        public FileDialogResult ShowSaveFileDialog(Window owner, IEnumerable<FileType> fileTypes, FileType defaultFileType, string defaultFileName)
        {
            Owner = owner;
            FileTypes = fileTypes;
            DefaultFileType = defaultFileType;
            DefaultFileName = defaultFileName;

            return DialogResult;
        }
    }
}

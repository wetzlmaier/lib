namespace Scch.Mvvm.Service
{
    public class FolderDialogResult
    {
        public FolderDialogResult(string selectedPath)
        {
            SelectedPath = selectedPath;
        }

        /// <summary>
        /// Gets a value indicating whether this instance contains valid data. This property returns <c>false</c>
        /// when the user canceled the folder dialog box.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return SelectedPath != null;
            }
        }

        public string SelectedPath { get; private set; }
    }
}

using System.IO;

namespace Scch.Common.IO
{
    public static class FileHelper
    {
        private static readonly string[] _imageFileTypes;
        private static readonly string[] _assemblyFileTypes;
        private static readonly string _dllFileType;
        private static readonly string _exeFileType;
        private static readonly string[] _executableFileTypes;

        static FileHelper()
        {
            _dllFileType = ".dll";
            _exeFileType = ".exe";
            _imageFileTypes = new[] { ".jpg", ".bmp", "jpeg", "png", "gif" };
            _assemblyFileTypes = new[] { _dllFileType, _exeFileType };
            _executableFileTypes = new[] { _exeFileType, ".bat", ".com" };
        }

        public static string[] ImageFileTypes
        {
            get
            {
                return _imageFileTypes;
            }
        }

        public static string[] AssemblyFileTypes
        {
            get
            {
                return _assemblyFileTypes;
            }
        }

        public static string[] ExecutableFileTypes
        {
            get
            {
                return _executableFileTypes;
            }
        }

        public static string DllFileType
        {
            get
            {
                return _dllFileType;
            }
        }

        public static string ExeFileType
        {
            get
            {
                return _exeFileType;
            }
        }

        public static void CreateBackup(string filename)
        {
            if (!File.Exists(filename))
                return;

            var bakfile = Path.ChangeExtension(filename, ".bak");
            if (File.Exists(bakfile))
                File.Delete(bakfile);

            File.Move(filename, bakfile);
        }

        public static void DeleteFile(string directory, string fileName)
        {
            new FileInfo(Path.Combine(directory, fileName)).Delete();
        }

        public static void DeleteFiles(string directory, string wildcard="*.*")
        {
            foreach (FileInfo info in new DirectoryInfo(directory).GetFiles(wildcard))
            {
                info.Delete();
            }
        }

        public static string FileNameWithoutExtension(string fileName)
        {
            return FileNameWithoutExtension(new FileInfo(fileName));
        }

        private static string FileNameWithoutExtension(FileInfo fileInfo)
        {
            return fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
        }
    }
}

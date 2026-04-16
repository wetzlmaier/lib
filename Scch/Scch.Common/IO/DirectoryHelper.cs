using System;
using System.IO;
using System.Linq;

namespace Scch.Common.IO
{
    public static class DirectoryHelper
    {
        public static void Copy(string sourceDir, string destinationDir, bool overwrite = false, bool recursive = false)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.EnumerateFiles(sourceDir))
            {
                var fileName = new FileInfo(file).Name;
                File.Copy(file, Path.Combine(destinationDir, fileName), overwrite);
            }

            if (!recursive)
                return;

            foreach (var subDir in Directory.EnumerateDirectories(sourceDir))
            {
                var subDirName = new DirectoryInfo(subDir).Name;
                Copy(Path.Combine(sourceDir, subDirName), Path.Combine(destinationDir, subDirName), overwrite);
            }
        }

        private static string ReplaceInvalidChars(string str, char[] invalidChars, char replacement = '_')
        {
            if (invalidChars.Contains(replacement))
                throw new ArgumentOutOfRangeException(nameof(replacement), "Replacement char is invalid.");

            foreach (var chr in invalidChars)
            {
                str = str.Replace(chr, replacement);
            }

            return str;
        }

        public static string ReplaceInvalidPathChars(string path, char replacement = '_')
        {
            return ReplaceInvalidChars(path, Path.GetInvalidPathChars(), replacement);
        }

        public static string ReplaceInvalidFileNameChars(string path, char replacement = '_')
        {
            return ReplaceInvalidChars(path, Path.GetInvalidFileNameChars(), replacement);
        }

        public static string CreateTempDir()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="relativeTo">Contains the directory that defines the start of the relative path.</param>
        /// <param name="path">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>path</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string GetRelativePath(string relativeTo, string path)
        {
            if (string.IsNullOrEmpty(relativeTo))
                throw new ArgumentNullException(nameof(relativeTo));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!relativeTo.EndsWith(Path.DirectorySeparatorChar.ToString()))
                relativeTo += Path.DirectorySeparatorChar;

            Uri fromUri = new Uri(relativeTo);
            Uri toUri = new Uri(path);

            if (fromUri.Scheme != toUri.Scheme) // path can't be made relative.
                return path;

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        public static bool IsPathFullyQualified(string path)
        {
            return !IsPartiallyQualified(path);
        }

        /// <summary>
        /// Returns true if the path specified is relative to the current drive or working directory.
        /// Returns false if the path is fixed to a specific drive or UNC path.  This method does no
        /// validation of the path (URIs will be returned as relative as a result).
        /// </summary>
        /// <remarks>
        /// Handles paths that use the alternate directory separator.  It is a frequent mistake to
        /// assume that rooted paths (Path.IsPathRooted) are not relative.  This isn't the case.
        /// "C:a" is drive relative- meaning that it will be resolved against the current directory
        /// for C: (rooted, but relative). "C:\a" is rooted and not relative (the current directory
        /// will not be used to modify the path).
        /// </remarks>
        private static bool IsPartiallyQualified(string path)
        {
            if (path.Length < 2)
            {
                // It isn't fixed, it must be relative.  There is no way to specify a fixed
                // path with one character (or less).
                return true;
            }

            if (IsDirectorySeparator(path[0]))
            {
                // There is no valid way to specify a relative path with two initial slashes or
                // \? as ? isn't valid for drive relative paths and \??\ is equivalent to \\?\
                return !(path[1] == '?' || IsDirectorySeparator(path[1]));
            }

            // The only way to specify a fixed path that doesn't begin with two slashes
            // is the drive, colon, slash format- i.e. C:\
            return !((path.Length >= 3)
                && (path[1] == Path.VolumeSeparatorChar)
                && IsDirectorySeparator(path[2])
                // To match old behavior we'll check the drive character for validity as the path is technically
                // not qualified if you don't have a valid drive. "=:\" is the "=" file's default data stream.
                && IsValidDriveChar(path[0]));
        }

        private static bool IsDirectorySeparator(char c)
        {
            return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
        }
        private static bool IsValidDriveChar(char value)
        {
            return (value >= 'A' && value <= 'Z') || (value >= 'a' && value <= 'z');
        }
    }
}

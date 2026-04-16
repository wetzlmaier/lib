using System.IO;

namespace Scch.ModelBasedTesting
{
    public abstract class FileStepper
    {
        private readonly string _logFile;

        protected FileStepper(string logFile)
        {
            _logFile = logFile;

            if (File.Exists(_logFile))
            {
                File.Delete(_logFile);
            }
        }

        protected void WriteLine(string text = null, params object[] args)
        {
            text = text ?? string.Empty;
            text = args.Length == 0 ? text : string.Format(text, args);
            File.AppendAllLines(_logFile, new[] { text });
        }
    }
}

using System;

namespace Scch.Logging
{
    public static class ConsoleLogger
    {
        public static void WriteDebug(string format, params object[] arg)
        {
            WriteLog(new DebugLogEntry(format, arg));
        }

        public static void WriteError(Exception exception)
        {
            WriteLog(new ExceptionLogEntry(exception));
        }

        public static void WriteError(string format, params object[] arg)
        {
            WriteLog(new ExceptionLogEntry(format, arg));
        }

        private static void WriteLog(LogEntryBase entry)
        {
            Logger.Write(entry);
            Console.WriteLine(entry.Message);
        }
    }
}

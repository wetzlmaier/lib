using System;
using System.Diagnostics;
using System.Threading;
using Scch.Common;
using Scch.Common.Configuration;
using Scch.Common.Diagnostics;
using Scch.Common.Threading;

namespace Scch.TimeoutWatchDog
{
    public class Program
    {
        private static int _timeout;

        static int Main(string[] args)
        {
            try
            {
                if (!ProcessHelper.IsSingleInstance())
                {
                    Console.WriteLine("Only one instance allowed.");
                    return ExitCodes.Error;
                }

                Console.WriteLine("Waiting for processes ...");

                var processesToMonitor = ConfigurationHelper.Current.ReadString("Processes", string.Empty).Split(StringHelper.CommaSemicolon);
                _timeout = ConfigurationHelper.Current.ReadInt("Timeout", 60);

                var watchDog = new ProcessWatchDog();
                watchDog.Monitor += Monitor;
                watchDog.RunConsole(processesToMonitor, ProcessHelper.LocalMachine, 500);

                return ExitCodes.Ok;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return ExitCodes.Error;
            }
            finally
            {
                ProcessHelper.DisposeMutex();
            }
        }

        private static void WriteTimeStamp()
        {
            var timeStampFormat = ConfigurationHelper.Current.ReadString("TimeStampFormat", "yyyy-MM-dd HH:mm:ss");
            Console.Write("[{0}] ", DateTime.Now.ToString(timeStampFormat));
        }

        private static void Monitor(Process process, CancellationToken token)
        {
            var start = DateTime.Now;
            bool killed = false;

            WriteTimeStamp();
            Console.WriteLine("Monitoring process '{0}' with id {1} (Timeout: {2})", process.ProcessName, process.Id, _timeout);

            while (!token.IsCancellationRequested && !process.HasExited)
            {
                if ((DateTime.Now - start).TotalSeconds > _timeout)
                {
                    WriteTimeStamp();
                    Console.WriteLine("Process '{0}' with id {1} reached timeout.", process.ProcessName, process.Id);
                    process.Kill();
                    killed = true;
                    break;
                }

                Thread.Sleep(100);
            }

            if (!killed)
            {
                WriteTimeStamp();
                Console.WriteLine("Process '{0}' with id {1} exited.", process.ProcessName, process.Id);
            }
        }
    }
}

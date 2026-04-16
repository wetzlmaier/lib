using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Scch.Common.Diagnostics;

namespace Scch.Common.Threading
{
    public class ProcessWatchDog
    {
        public event MonitoringDelegate Monitor;

        private ISet<int> _monitoredProcesses;

        public void RunConsole(string[] processesToMonitor, string machineName = ProcessHelper.LocalMachine, int checkWait = 100)
        {
            Run(processesToMonitor, machineName, checkWait, ConsoleApplication.BlockingCondition, ConsoleApplication.Cancellation);
        }

        public void Run(string[] processesToMonitor, int checkWait, BlockingConditionDelegate blockingCondition, CancellationDelegate cancellation)
        {
            Run(processesToMonitor, ProcessHelper.LocalMachine, checkWait, blockingCondition, cancellation);
        }

        public void Run(string[] processesToMonitor, string machineName, int checkWait, BlockingConditionDelegate blockingCondition, CancellationDelegate cancellation)
        {
            if (Monitor == null)
                throw new InvalidOperationException("Monitor event not set.");

            List<Task> tasks = new List<Task>();
            _monitoredProcesses = new HashSet<int>();

            ConsoleApplication.Run(source =>
            {
                foreach (var processName in processesToMonitor)
                {
                    Process[] processes;

                    try
                    {
                        processes = Process.GetProcessesByName(processName, machineName);
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (ex.InnerException?.InnerException != null && ex.InnerException.InnerException.GetType() == typeof(Win32Exception))
                        {
                            var rootException = (Win32Exception)ex.InnerException.InnerException;

                            switch (rootException.NativeErrorCode)
                            {
                                case 5:
                                    throw new SecurityException("Access denied.", ex);
                                case 1717:
                                case 53:
                                    throw new InvalidOperationException("The 'Remote Registry' service is not running on the remote machine.", ex);
                                default:
                                    throw;
                            }
                        }

                        throw;
                    }

                    foreach (var process in processes)
                    {
                        var process1 = process;

                        if (!_monitoredProcesses.Contains(process.Id))
                        {
                            _monitoredProcesses.Add(process1.Id);
                            tasks.Add(Task.Factory.StartNew(() => MonitorInternal(process1, source.Token), source.Token));
                        }
                    }
                }

                Thread.Sleep(checkWait);
            });

            Task.WaitAll(tasks.ToArray());
        }

        private void MonitorInternal(Process process, CancellationToken token)
        {
            try
            {
                Monitor(process, token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                _monitoredProcesses.Remove(process.Id);
            }
        }
    }
}

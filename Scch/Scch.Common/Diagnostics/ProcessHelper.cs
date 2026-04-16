using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scch.Common.Windows;

namespace Scch.Common.Diagnostics
{
    public static class ProcessHelper
    {
        private static Mutex _mutex;
        public const string LocalMachine = ".";

        /// <summary>
        /// Returns true, if the program is started once. Otherwise false.
        /// </summary>
        /// <returns>True, if the program is started once. Otherwise false.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static bool IsSingleInstance()
        {
            // ReSharper disable PossibleNullReferenceException
            var info = new FileInfo(Process.GetCurrentProcess().MainModule.ModuleName);
            // ReSharper restore PossibleNullReferenceException
            string name = info.Name;

            bool firstInstance;
            var mutex = new Mutex(true, name, out firstInstance);

            if (!firstInstance)
                mutex.Close();
            else
                _mutex = mutex;

            return firstInstance;
        }

        public static bool IsRemoteMachine(string machineName)
        {
            if (machineName == null)
            {
                throw new ArgumentNullException("machineName");
            }

            string text;
            if (machineName.StartsWith("\\", StringComparison.Ordinal))
            {
                text = machineName.Substring(2);
            }
            else
            {
                text = machineName;
            }
            if (text.Equals("."))
            {
                return false;
            }
            StringBuilder stringBuilder = new StringBuilder(256);
            Kernel32.GetComputerName(stringBuilder, new[] { stringBuilder.Capacity });
            string strA = stringBuilder.ToString();
            return string.Compare(strA, text, StringComparison.OrdinalIgnoreCase) != 0;
        }

        public static string GetBatchOutput(string fileName, string args = "", bool includeOutput = true, bool includeErrorOutput = true, int milliseconds = int.MaxValue)
        {
            return GetBatchOutput(CreateProcessStartInfo(fileName, args), includeOutput, includeErrorOutput, milliseconds);
        }

        public static void DisposeMutex()
        {
            if (_mutex != null)
            {
                _mutex.Dispose();
                _mutex = null;
            }
        }

        public static Process[] GetProcessesByFileName(string fileName)
        {
            var processName = GetProcessName(fileName);
            return Process.GetProcessesByName(processName);
        }

        public static int ExecuteBatchProcess(string fileName, string args = "")
        {
            return ExecuteBatchProcess(CreateProcessStartInfo(fileName, args), true);
        }

        public static Process StartApplication(ProcessStartInfo info, int timeout)
        {
            var processes = GetProcessesByFileName(info.FileName);

            Process process;
            if (processes.Length == 0)
            {
                var workingDirectory = Path.GetDirectoryName(info.FileName);
                if (workingDirectory != null)
                    info.WorkingDirectory = workingDirectory;

                process = Process.Start(info);

                Task.Factory.StartNew(() =>
                {
                    while (process.MainWindowHandle == IntPtr.Zero)
                        Thread.Sleep(50);
                }).Wait(timeout);
            }
            else
                process = processes[0];

            return process;
        }

        public static string GetProcessName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public static string GetBatchOutput(ProcessStartInfo startInfo, bool includeOutput = true, bool includeErrorOutput = true, int milliseconds = int.MaxValue)
        {
            StringBuilder sb = new StringBuilder();

            DataReceivedEventHandler outputDataReceived = null;

            if (includeOutput)
            {
                outputDataReceived = (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        sb.AppendLine(e.Data);
                    }
                };
            }

            DataReceivedEventHandler errorDataReceived = null;

            if (includeErrorOutput)
            {
                errorDataReceived = (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        sb.AppendLine(e.Data);
                    }
                };
            }

            ExecuteBatchProcess(startInfo, outputDataReceived, errorDataReceived, milliseconds);

            return sb.ToString();
        }

        public static int ExecuteBatchProcess(ProcessStartInfo startInfo, bool writeOutputToConsole = true, int milliseconds = int.MaxValue)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("{0} {1}", startInfo.FileName, startInfo.Arguments);

            DataReceivedEventHandler errorDataReceived = null;
            DataReceivedEventHandler outputDataReceived = null;

            if (writeOutputToConsole)
            {
                errorDataReceived = (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Data);
                    }
                };

                outputDataReceived = (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(e.Data);
                    }
                };
            }

            return ExecuteBatchProcess(startInfo, outputDataReceived, errorDataReceived, milliseconds);
        }

        public static ProcessStartInfo CreateProcessStartInfo(string fileName, string args = "")
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args
            };

            var workingDirectory = Path.GetDirectoryName(startInfo.FileName);
            if (workingDirectory != null)
                startInfo.WorkingDirectory = workingDirectory;
            startInfo.UseShellExecute = false;

            return startInfo;
        }

        public static int ExecuteBatchProcess(ProcessStartInfo startInfo, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null, int milliseconds = int.MaxValue)
        {
            using (var process = new Process())
            {
                try
                {
                    process.StartInfo = startInfo;

                    if (errorDataReceived != null)
                    {
                        startInfo.RedirectStandardError = true;
                        process.ErrorDataReceived += errorDataReceived;
                    }

                    if (outputDataReceived != null)
                    {
                        startInfo.RedirectStandardOutput = true;
                        process.OutputDataReceived += outputDataReceived;
                    }

                    process.Start();

                    if (errorDataReceived != null)
                    {
                        process.BeginErrorReadLine();
                    }

                    if (outputDataReceived != null)
                    {
                        process.BeginOutputReadLine();
                    }

                    try
                    {
                        if (!process.WaitForExit(milliseconds))
                            throw new TimeoutException("Process timed out.");

                        Console.WriteLine();
                        return process.ExitCode;
                    }
                    finally
                    {
                        if (errorDataReceived != null)
                        {
                            process.CancelErrorRead();
                        }

                        if (outputDataReceived != null)
                        {
                            process.CancelOutputRead();
                        }
                    }
                }
                finally
                {
                    if (errorDataReceived != null)
                    {
                        process.ErrorDataReceived -= errorDataReceived;
                    }

                    if (outputDataReceived != null)
                    {
                        process.OutputDataReceived -= outputDataReceived;
                    }
                }
            }
        }

        public static void Shutdown(Process process)
        {
            if (process.HasExited)
                return;

            process.CloseMainWindow();
        }

        public static bool Kill(Process[] processes)
        {
            bool result = false;

            foreach (var process in processes)
                result |= Kill(process);

            return result;
        }

        public static bool Kill(Process process)
        {
            if (process.HasExited)
                return false;

            process.Kill();

            var werFault = Process.GetProcessesByName("WerFault").SingleOrDefault();
            var fault = werFault != null;

            if (fault)
                werFault.Kill();

            return fault;
        }

        public static bool ShutdownOrKill(Process[] processes, int timeout)
        {
            var shutdownTasks = new List<Task>();

            foreach (var process in processes)
            {
                if (process.HasExited)
                    continue;

                var shutdownTask = Task.Factory.StartNew(() =>
                {
                    if (!process.HasExited)
                        Shutdown(process);

                    while (!process.HasExited)
                    {
                        Thread.Sleep(10);
                    }
                });

                shutdownTasks.Add(shutdownTask);
            }

            Task.WaitAll(shutdownTasks.ToArray(), timeout);
            var result = true;

            foreach (var process in processes)
            {
                if (process.HasExited)
                    continue;

                Kill(process);
                result = false;
            }

            return result;
        }

        public static bool ShutdownOrKill(Process process, int timeout)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.HasExited)
                return true;

            var shutdownTask = Task.Factory.StartNew(() =>
            {
                if (!process.HasExited)
                    Shutdown(process);

                while (!process.HasExited)
                {
                    Thread.Sleep(10);
                }
            });

            shutdownTask.Wait(timeout);
            if (process.HasExited)
                return true;

            Kill(process);
            return false;
        }

        private static ManagementObjectCollection ExecuteWmiQuery(string wmiQuery)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
            return searcher.Get();
        }

        private static IEnumerable<object> WmiProcessInfo(string processName, string property)
        {
            string wmiQuery = string.Format("select CommandLine from Win32_Process where Name like '%{0}%'", processName);
            var result = ExecuteWmiQuery(wmiQuery);

            return (result.Cast<ManagementObject>().Select<ManagementObject, object>(r => r[property]));
        }

        public static string[] WmiCommandLine(string processName)
        {
            return WmiProcessInfo(processName, "CommandLine").Cast<string>().ToArray();
        }

        public static void SetPriority(ProcessPriorityClass priority)
        {
            SetPriority(Process.GetCurrentProcess(), priority);
        }

        public static void SetPriority(Process process, ProcessPriorityClass priority)
        {
            process.PriorityClass = priority;
        }

        public static bool HasExited(Process process, string machineName)
        {
            try
            {
                using (var p = Process.GetProcessById(process.Id, machineName))
                {
                    return p == null;
                }
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        public static void RestartAsAdministrator()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdministrator = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdministrator)
            {
                var process = Process.GetCurrentProcess();

                ProcessStartInfo startInfo = new ProcessStartInfo(process.MainModule.FileName, process.StartInfo.Arguments)
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                /*
                if (user != null)
                {
                    var splitUserAndDomain = user.Split('\\');
                    if (splitUserAndDomain.Length > 1)
                    {
                        startInfo.Domain = splitUserAndDomain[0];
                        startInfo.UserName = splitUserAndDomain[1];
                    }
                    else
                    {
                        startInfo.Domain = Environment.UserDomainName;
                        startInfo.UserName = user;
                    }

                    if (password != null)
                    {
                        var securePassword = new SecureString();

                        foreach (char c in password)
                        {
                            securePassword.AppendChar(c);
                        }

                        startInfo.Password = securePassword;
                    }
                }*/

                Process.Start(startInfo);
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        /// <summary>
        /// Splits the command line arguments and returns the arguments as array of string.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>The arguments as array of string.</returns>
        public static string[] CommandLineToArgs(string commandLine)
        {
            if (commandLine == null)
                throw new ArgumentNullException(nameof(commandLine));

            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);

            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();

            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        /// <summary>
        /// Executes an array of commands in parallel.
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="maxDegreeOfParallelism"></param>
        /// <param name="writeToConsole">Writes the output to the console.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string ExecuteBatchProcessesParallel(string[] commands, int maxDegreeOfParallelism, bool writeToConsole)
        {
            var syncRoot = new object();
            var consoleOutput = new StringBuilder();
            Parallel.ForEach(commands, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, command =>
            {
                if (string.IsNullOrWhiteSpace(command))
                    return;

                var commandArgs = CommandLineToArgs(command);

                if (commandArgs.Length == 0)
                    throw new FileNotFoundException("Command '{0}' not found.", command);

                var commandIndex = commandArgs[0].ToLower() == "call" ? 1 : 0;
                List<string> expandedArgs = new List<string>();
                for (int index = commandIndex + 1; index < commandArgs.Length; index++)
                {
                    var expandedArg = Environment.ExpandEnvironmentVariables(commandArgs[index]);
                    if (expandedArg.Contains(" "))
                        expandedArg = "\"" + expandedArg + "\"";
                    expandedArgs.Add(expandedArg);
                }

                var output = GetBatchOutput(commandArgs[commandIndex], string.Join(" ", expandedArgs));

                lock (syncRoot)
                {
                    if (writeToConsole)
                        Console.Write(output);

                    consoleOutput.Append(output);
                }
            });

            return consoleOutput.ToString();
        }
    }
}

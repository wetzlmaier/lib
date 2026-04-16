using System;
using System.ComponentModel;
using System.Diagnostics;
using Scch.Common.Reflecton;

namespace Scch.Common.Diagnostics
{
    public class ProcessInfo : IProcessInfo
    {
        private readonly Process _process;
        private readonly IAssemblyInfo _assemblyInfo;

        public ProcessInfo(Process process)
        {
            _process = process;

            try
            {
                _assemblyInfo = new AssemblyInfo(_process.MainModule.FileName);
            }
            catch (Win32Exception)
            {
            }
        }

        public IAssemblyInfo AssemblyInfo
        {
            get { return _assemblyInfo; }
        }

        public string Arguments
        {
            get { return _process.StartInfo.Arguments; }
        }

        public int BasePriority
        {
            get { return _process.BasePriority; }
        }

        public int ExitCode
        {
            get { return _process.ExitCode; }
        }

        public DateTime ExitTime
        {
            get { return _process.ExitTime; }
        }

        public IntPtr Handle
        {
            get { return _process.Handle; }
        }

        public bool HasExited
        {
            get { return _process.HasExited; }
        }

        public int Id
        {
            get { return _process.Id; }
        }

        public string MachineName
        {
            get { return _process.MachineName; }
        }

        public IntPtr MainWindowHandle
        {
            get { return _process.MainWindowHandle; }
        }

        public string MainWindowTitle
        {
            get { return _process.MainWindowTitle; }
        }

        public ProcessPriorityClass PriorityClass
        {
            get { return _process.PriorityClass; }
        }

        public string ProcessName
        {
            get { return _process.ProcessName; }
        }

        public DateTime StartTime
        {
            get { return _process.StartTime; }
        }

        public string WorkingDirectory
        {
            get { return _process.StartInfo.WorkingDirectory; }
        }
    }
}

using System;
using System.Diagnostics;
using Scch.Common.Reflecton;

namespace Scch.Common.Diagnostics
{
    public interface IProcessInfo
    {
        IAssemblyInfo AssemblyInfo { get; }
        string Arguments { get; }
        int BasePriority { get; }
        int ExitCode { get; }
        DateTime ExitTime { get; }
        IntPtr Handle { get; }
        bool HasExited { get; }
        int Id { get; }
        string MachineName { get; }
        IntPtr MainWindowHandle { get; }
        string MainWindowTitle { get; }
        string ProcessName { get; }
        ProcessPriorityClass PriorityClass { get; }
        DateTime StartTime { get; }
        string WorkingDirectory { get; }
    }
}
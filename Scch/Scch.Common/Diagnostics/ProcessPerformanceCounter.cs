using System;
using System.Diagnostics;

namespace Scch.Common.Diagnostics
{
    /// <summary>
    /// Read <see cref="PerformanceCounter"/> values for processes.
    /// </summary>
    public class ProcessPerformanceCounter : PerformanceCounterBase, IProcessPerformanceCounter
    {
        private const string Process = "Process";
        private readonly PerformanceCounter _percentPrivilegedTime;
        private readonly PerformanceCounter _percentProcessorTime;
        private readonly PerformanceCounter _percentUserTime;
        private readonly PerformanceCounter _pageFaultsPerSecond;
        private readonly PerformanceCounter _pageFileBytes;
        private readonly PerformanceCounter _pageFileBytesPeak;
        private readonly PerformanceCounter _priorityBase;
        private readonly PerformanceCounter _privateBytes;
        private readonly PerformanceCounter _workingSet;
        private readonly PerformanceCounter _creatingProcessId;
        private readonly PerformanceCounter _elapsedTime;
        private readonly PerformanceCounter _handleCount;
        private readonly PerformanceCounter _processId;
        private readonly PerformanceCounter _dataBytesPerSecond;
        private readonly PerformanceCounter _dataOperationsPerSecond;
        private readonly PerformanceCounter _otherBytesPerSecond;
        private readonly PerformanceCounter _otherOperationsPerSecond;
        private readonly PerformanceCounter _readBytesPerSecond;
        private readonly PerformanceCounter _readOperationsPerSecond;
        private readonly PerformanceCounter _writeBytesPerSecond;
        private readonly PerformanceCounter _writeOperationsPerSecond;
        private readonly PerformanceCounter _poolNonpagedBytes;
        private readonly PerformanceCounter _poolPagedBytes;
        private readonly PerformanceCounter _threadCount;
        private readonly PerformanceCounter _virtualBytes;
        private readonly PerformanceCounter _virtualBytesPeak;
        private readonly PerformanceCounter _workingSetPeak;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessPerformanceCounter"/> class.
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="machineName"></param>
        public ProcessPerformanceCounter(string processName, string machineName = ProcessHelper.LocalMachine) : base(processName, machineName)
        {
            if (processName == null)
                throw new ArgumentNullException(nameof(processName));
            if (machineName == null)
                throw new ArgumentNullException(nameof(machineName));

            _percentPrivilegedTime = new PerformanceCounter(Process, "% Privileged Time", processName, machineName);
            _percentProcessorTime = new PerformanceCounter(Process, "% Processor Time", processName, machineName);
            _percentUserTime = new PerformanceCounter(Process, "% User Time", processName, machineName);
            _pageFaultsPerSecond = new PerformanceCounter(Process, "Page Faults/sec", processName, machineName);
            _pageFileBytes = new PerformanceCounter(Process, "Page File Bytes", processName, machineName);
            _pageFileBytesPeak = new PerformanceCounter(Process, "Page File Bytes Peak", processName, machineName);
            _priorityBase = new PerformanceCounter(Process, "Priority Base", processName, machineName);
            _privateBytes = new PerformanceCounter(Process, "Private Bytes", processName, machineName);
            _workingSet = new PerformanceCounter(Process, "Working Set", processName, machineName);
            _creatingProcessId = new PerformanceCounter(Process, "Creating Process ID", processName, machineName);
            _elapsedTime = new PerformanceCounter(Process, "Elapsed Time", processName, machineName);
            _handleCount = new PerformanceCounter(Process, "Handle Count", processName, machineName);
            _processId = new PerformanceCounter(Process, "ID Process", processName, machineName);
            _dataBytesPerSecond = new PerformanceCounter(Process, "IO Data Bytes/sec", processName, machineName);
            _dataOperationsPerSecond = new PerformanceCounter(Process, "IO Data Operations/sec", processName, machineName);
            _otherBytesPerSecond = new PerformanceCounter(Process, "IO Other Bytes/sec", processName, machineName);
            _otherOperationsPerSecond = new PerformanceCounter(Process, "IO Other Operations/sec", processName, machineName);
            _readBytesPerSecond = new PerformanceCounter(Process, "IO Read Bytes/sec", processName, machineName);
            _readOperationsPerSecond = new PerformanceCounter(Process, "IO Read Operations/sec", processName, machineName);
            _writeBytesPerSecond = new PerformanceCounter(Process, "IO Write Bytes/sec", processName, machineName);
            _writeOperationsPerSecond = new PerformanceCounter(Process, "IO Write Operations/sec", processName, machineName);
            _poolNonpagedBytes = new PerformanceCounter(Process, "Pool Nonpaged Bytes", processName, machineName);
            _poolPagedBytes = new PerformanceCounter(Process, "Pool Paged Bytes", processName, machineName);
            _threadCount = new PerformanceCounter(Process, "Thread Count", processName, machineName);
            _virtualBytes = new PerformanceCounter(Process, "Virtual Bytes", processName, machineName);
            _virtualBytesPeak = new PerformanceCounter(Process, "Virtual Bytes Peak", processName, machineName);
            _workingSetPeak = new PerformanceCounter(Process, "Working Set Peak", processName, machineName);
        }

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PercentPrivilegedTime"/>
        /// </summary>
        public double PercentPrivilegedTime => _percentPrivilegedTime.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PercentProcessorTime"/>
        /// </summary>
        public double PercentProcessorTime => _percentProcessorTime.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PercentUserTime"/>
        /// </summary>
        public double PercentUserTime => _percentUserTime.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PageFaultsPerSecond"/>
        /// </summary>
        public double PageFaultsPerSecond => _pageFaultsPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PageFileBytes"/>
        /// </summary>
        public long PageFileBytes => (long)_pageFileBytes.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PageFileBytesPeak"/>
        /// </summary>
        public long PageFileBytesPeak => (long)_pageFileBytesPeak.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PriorityBase"/>
        /// </summary>
        public int PriorityBase => (int)_priorityBase.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PrivateBytes"/>
        /// </summary>
        public long PrivateBytes => (long)_privateBytes.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.WorkingSet"/>
        /// </summary>
        public long WorkingSet => (long)_workingSet.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.CreatingProcessId"/>
        /// </summary>
        public int CreatingProcessId => (int)_creatingProcessId.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.ElapsedTime"/>
        /// </summary>
        public double ElapsedTime => _elapsedTime.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.HandleCount"/>
        /// </summary>
        public int HandleCount => (int)_handleCount.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.ProcessId"/>
        /// </summary>
        public int ProcessId => (int)_processId.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.DataBytesPerSecond"/>
        /// </summary>
        public double DataBytesPerSecond => _dataBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.DataOperationsPerSecond"/>
        /// </summary>
        public double DataOperationsPerSecond => _dataOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.OtherBytesPerSecond"/>
        /// </summary>
        public double OtherBytesPerSecond => _otherBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.OtherOperationsPerSecond"/>
        /// </summary>
        public double OtherOperationsPerSecond => _otherOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.ReadBytesPerSecond"/>
        /// </summary>
        public double ReadBytesPerSecond => _readBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.ReadOperationsPerSecond"/>
        /// </summary>
        public double ReadOperationsPerSecond => _readOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.WriteBytesPerSecond"/>
        /// </summary>
        public double WriteBytesPerSecond => _writeBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.WriteOperationsPerSecond"/>
        /// </summary>
        public double WriteOperationsPerSecond => _writeOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PoolNonpagedBytes"/>
        /// </summary>
        public long PoolNonpagedBytes => (long)_poolNonpagedBytes.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.PoolPagedBytes"/>
        /// </summary>
        public long PoolPagedBytes => (long)_poolPagedBytes.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.ThreadCount"/>
        /// </summary>
        public int ThreadCount => (int)_threadCount.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.VirtualBytes"/>
        /// </summary>
        public long VirtualBytes => (long)_virtualBytes.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.VirtualBytesPeak"/>
        /// </summary>
        public long VirtualBytesPeak => (long)_virtualBytesPeak.NextValue();

        /// <summary>
        /// <see cref="IProcessPerformanceCounter.WorkingSetPeak"/>
        /// </summary>
        public long WorkingSetPeak => (long)_workingSetPeak.NextValue();

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _percentPrivilegedTime.Dispose();
                _percentProcessorTime.Dispose();
                _percentUserTime.Dispose();
                _pageFaultsPerSecond.Dispose();
                _pageFileBytes.Dispose();
                _pageFileBytesPeak.Dispose();
                _priorityBase.Dispose();
                _privateBytes.Dispose();
                _workingSet.Dispose();
                _creatingProcessId.Dispose();
                _elapsedTime.Dispose();
                _handleCount.Dispose();
                _processId.Dispose();
                _dataBytesPerSecond.Dispose();
                _dataOperationsPerSecond.Dispose();
                _otherBytesPerSecond.Dispose();
                _otherOperationsPerSecond.Dispose();
                _readBytesPerSecond.Dispose();
                _readOperationsPerSecond.Dispose();
                _writeBytesPerSecond.Dispose();
                _writeOperationsPerSecond.Dispose();
                _poolNonpagedBytes.Dispose();
                _poolPagedBytes.Dispose();
                _threadCount.Dispose();
                _virtualBytes.Dispose();
                _virtualBytesPeak.Dispose();
                _workingSetPeak.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

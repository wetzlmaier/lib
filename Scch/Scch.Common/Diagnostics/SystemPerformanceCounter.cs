using System;
using System.Diagnostics;

namespace Scch.Common.Diagnostics
{
    /// <summary>
    /// Read <see cref="PerformanceCounter"/> values for the system.
    /// </summary>
    public class SystemPerformanceCounter : PerformanceCounterBase, ISystemPerformanceCounter
    {
        private const string System = "System";
        private readonly PerformanceCounter _percentRegistryQuotaInUse;
        private readonly PerformanceCounter _alignmentFixupsPerSecond;
        private readonly PerformanceCounter _contextSwitchesPerSecond;
        private readonly PerformanceCounter _exceptionDispatchesPerSecond;
        private readonly PerformanceCounter _fileControlBytesPerSecond;
        private readonly PerformanceCounter _fileControlOperationsPerSecond;
        private readonly PerformanceCounter _fileDataOperationsPerSecond;
        private readonly PerformanceCounter _fileReadBytesPerSecond;
        private readonly PerformanceCounter _fileReadOperationsPerSecond;
        private readonly PerformanceCounter _fileWriteBytesPerSecond;
        private readonly PerformanceCounter _fileWriteOperationsPerSecond;
        private readonly PerformanceCounter _floatingEmulationPerSecond;
        private readonly PerformanceCounter _processes;
        private readonly PerformanceCounter _processorQueueLength;
        private readonly PerformanceCounter _systemCallsPerSecond;
        private readonly PerformanceCounter _systemUpTime;
        private readonly PerformanceCounter _threads;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemPerformanceCounter"/> class.
        /// </summary>
        /// <param name="machineName"></param>
        public SystemPerformanceCounter(string machineName = ProcessHelper.LocalMachine) : base(null, machineName)
        {
            if (machineName == null)
                throw new ArgumentNullException(nameof(machineName));

            _percentRegistryQuotaInUse = new PerformanceCounter(System, "% Registry Quota In Use", null, machineName);
            _alignmentFixupsPerSecond = new PerformanceCounter(System, "Alignment Fixups/sec", null, machineName);
            _contextSwitchesPerSecond = new PerformanceCounter(System, "Context Switches/sec", null, machineName);
            _exceptionDispatchesPerSecond = new PerformanceCounter(System, "Exception Dispatches/sec", null, machineName);
            _fileControlBytesPerSecond = new PerformanceCounter(System, "File Control Bytes/sec", null, machineName);
            _fileControlOperationsPerSecond = new PerformanceCounter(System, "File Control Operations/sec", null, machineName);
            _fileDataOperationsPerSecond = new PerformanceCounter(System, "File Data Operations/sec", null, machineName);
            _fileReadBytesPerSecond = new PerformanceCounter(System, "File Read Bytes/sec", null, machineName);
            _fileReadOperationsPerSecond = new PerformanceCounter(System, "File Read Operations/sec", null, machineName);
            _fileWriteBytesPerSecond = new PerformanceCounter(System, "File Write Bytes/sec", null, machineName);
            _fileWriteOperationsPerSecond = new PerformanceCounter(System, "File Write Operations/sec", null, machineName);
            _floatingEmulationPerSecond = new PerformanceCounter(System, "Floating Emulations/sec", null, machineName);
            _processes = new PerformanceCounter(System, "Processes", null, machineName);
            _processorQueueLength = new PerformanceCounter(System, "Processor Queue Length", null, machineName);
            _systemCallsPerSecond = new PerformanceCounter(System, "System Calls/sec", null, machineName);
            _systemUpTime = new PerformanceCounter(System, "System Up Time", null, machineName);
            _threads = new PerformanceCounter(System, "Threads", null, machineName);
        }

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.PercentRegistryQuotaInUse"/>
        /// </summary>
        public double PercentRegistryQuotaInUse => _percentRegistryQuotaInUse.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.AlignmentFixupsPerSecond"/>
        /// </summary>
        public double AlignmentFixupsPerSecond => _alignmentFixupsPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.ContextSwitchesPerSecond"/>
        /// </summary>
        public double ContextSwitchesPerSecond => _contextSwitchesPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.ExceptionDispatchesPerSecond"/>
        /// </summary>
        public double ExceptionDispatchesPerSecond => _exceptionDispatchesPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileControlBytesPerSecond"/>
        /// </summary>
        public double FileControlBytesPerSecond => _fileControlBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileControlOperationsPerSecond"/>
        /// </summary>
        public double FileControlOperationsPerSecond => _fileControlOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileDataOperationsPerSecond"/>
        /// </summary>
        public double FileDataOperationsPerSecond => _fileDataOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileReadBytesPerSecond"/>
        /// </summary>
        public double FileReadBytesPerSecond => _fileReadBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileReadOperationsPerSecond"/>
        /// </summary>
        public double FileReadOperationsPerSecond => _fileReadOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileWriteBytesPerSecond"/>
        /// </summary>
        public double FileWriteBytesPerSecond => _fileWriteBytesPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FileWriteOperationsPerSecond"/>
        /// </summary>
        public double FileWriteOperationsPerSecond => _fileWriteOperationsPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.FloatingEmulationPerSecond"/>
        /// </summary>
        public double FloatingEmulationPerSecond => _floatingEmulationPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.Processes"/>
        /// </summary>
        public int Processes => (int)_processes.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.ProcessorQueueLength"/>
        /// </summary>
        public int ProcessorQueueLength => (int)_processorQueueLength.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.SystemCallsPerSecond"/>
        /// </summary>
        public double SystemCallsPerSecond => _systemCallsPerSecond.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.SystemUpTime"/>
        /// </summary>
        public double SystemUpTime => _systemUpTime.NextValue();

        /// <summary>
        /// <see cref="ISystemPerformanceCounter.Threads"/>
        /// </summary>
        public int Threads => (int)_threads.NextValue();

        /// <summary>
        /// <see cref="IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _percentRegistryQuotaInUse.Dispose();
                _alignmentFixupsPerSecond.Dispose();
                _contextSwitchesPerSecond.Dispose();
                _exceptionDispatchesPerSecond.Dispose();
                _fileControlBytesPerSecond.Dispose();
                _fileControlOperationsPerSecond.Dispose();
                _fileDataOperationsPerSecond.Dispose();
                _fileReadBytesPerSecond.Dispose();
                _fileReadOperationsPerSecond.Dispose();
                _fileWriteBytesPerSecond.Dispose();
                _fileWriteOperationsPerSecond.Dispose();
                _floatingEmulationPerSecond.Dispose();
                _processes.Dispose();
                _processorQueueLength.Dispose();
                _systemCallsPerSecond.Dispose();
                _systemUpTime.Dispose();
                _threads.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

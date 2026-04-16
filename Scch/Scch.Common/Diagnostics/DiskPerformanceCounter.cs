using System.Diagnostics;

namespace Scch.Common.Diagnostics
{
    public abstract class DiskPerformanceCounter : PerformanceCounterBase, IDiskPerformanceCounter
    {
        private readonly PerformanceCounter _percentDiskReadTime;
        private readonly PerformanceCounter _percentDiskTime;
        private readonly PerformanceCounter _percentDiskWriteTime;
        private readonly PerformanceCounter _avgDiskBytesPerRead;
        private readonly PerformanceCounter _avgDiskBytesPerTransfer;
        private readonly PerformanceCounter _avgDiskBytesPerWrite;
        private readonly PerformanceCounter _avgDiskQueueLength;
        private readonly PerformanceCounter _avgDiskReadQueueLength;
        private readonly PerformanceCounter _avgDiskSecPerRead;
        private readonly PerformanceCounter _avgDiskSecPerTransfer;
        private readonly PerformanceCounter _avgDiskSecPerWrite;
        private readonly PerformanceCounter _avgDiskWriteQueueLength;
        private readonly PerformanceCounter _currentDiskQueueLength;
        private readonly PerformanceCounter _diskBytesPerSec;
        private readonly PerformanceCounter _diskReadBytesPerSec;
        private readonly PerformanceCounter _diskReadsPerSec;
        private readonly PerformanceCounter _diskTransfersPerSec;
        private readonly PerformanceCounter _diskWriteBytesPerSec;
        private readonly PerformanceCounter _diskWritesPerSec;

        protected DiskPerformanceCounter(string categoryName, string instanceName, string machineName) : base(instanceName, machineName)
        {
            _percentDiskReadTime = new PerformanceCounter(categoryName, "% Disk Read Time", instanceName, machineName);
            _percentDiskTime = new PerformanceCounter(categoryName, "% Disk Time", instanceName, machineName);
            _percentDiskWriteTime = new PerformanceCounter(categoryName, "% Disk Write Time", instanceName, machineName);
            _avgDiskBytesPerRead = new PerformanceCounter(categoryName, "Avg. Disk Bytes/Read", instanceName, machineName);
            _avgDiskBytesPerTransfer = new PerformanceCounter(categoryName, "Avg. Disk Bytes/Transfer", instanceName, machineName);
            _avgDiskBytesPerWrite = new PerformanceCounter(categoryName, "Avg. Disk Bytes/Write", instanceName, machineName);
            _avgDiskQueueLength = new PerformanceCounter(categoryName, "Avg. Disk Queue Length", instanceName, machineName);
            _avgDiskReadQueueLength = new PerformanceCounter(categoryName, "Avg. Disk Read Queue Length", instanceName, machineName);
            _avgDiskSecPerRead = new PerformanceCounter(categoryName, "Avg. Disk sec/Read", instanceName, machineName);
            _avgDiskSecPerTransfer = new PerformanceCounter(categoryName, "Avg. Disk sec/Transfer", instanceName, machineName);
            _avgDiskSecPerWrite = new PerformanceCounter(categoryName, "Avg. Disk sec/Write", instanceName, machineName);
            _avgDiskWriteQueueLength = new PerformanceCounter(categoryName, "Avg. Disk Write Queue Length", instanceName, machineName);
            _currentDiskQueueLength = new PerformanceCounter(categoryName, "Current Disk Queue Length", instanceName, machineName);
            _diskBytesPerSec = new PerformanceCounter(categoryName, "Disk Bytes/sec", instanceName, machineName);
            _diskReadBytesPerSec = new PerformanceCounter(categoryName, "Disk Read Bytes/sec", instanceName, machineName);
            _diskReadsPerSec = new PerformanceCounter(categoryName, "Disk Reads/sec", instanceName, machineName);
            _diskTransfersPerSec = new PerformanceCounter(categoryName, "Disk Transfers/sec", instanceName, machineName);
            _diskWriteBytesPerSec = new PerformanceCounter(categoryName, "Disk Write Bytes/sec", instanceName, machineName);
            _diskWritesPerSec = new PerformanceCounter(categoryName, "Disk Writes/sec", instanceName, machineName);
        }

        public double PercentDiskReadTime => _percentDiskReadTime.NextValue();

        public double PercentDiskTime => _percentDiskTime.NextValue();

        public double PercentDiskWriteTime => _percentDiskWriteTime.NextValue();

        public double AvgDiskBytesPerRead => _avgDiskBytesPerRead.NextValue();

        public double AvgDiskBytesPerTransfer => _avgDiskBytesPerTransfer.NextValue();

        public double AvgDiskBytesPerWrite => _avgDiskBytesPerWrite.NextValue();

        public double AvgDiskQueueLength => _avgDiskQueueLength.NextValue();

        public double AvgDiskReadQueueLength => _avgDiskReadQueueLength.NextValue();

        public double AvgDiskSecPerRead => _avgDiskSecPerRead.NextValue();

        public double AvgDiskSecPerTransfer => _avgDiskSecPerTransfer.NextValue();

        public double AvgDiskSecPerWrite => _avgDiskSecPerWrite.NextValue();

        public double AvgDiskWriteQueueLength => _avgDiskWriteQueueLength.NextValue();

        public int CurrentDiskQueueLength => (int)_currentDiskQueueLength.NextValue();

        public double DiskBytesPerSec => _diskBytesPerSec.NextValue();

        public double DiskReadBytesPerSec => _diskReadBytesPerSec.NextValue();

        public double DiskReadsPerSec => _diskReadsPerSec.NextValue();

        public double DiskTransfersPerSec => _diskTransfersPerSec.NextValue();

        public double DiskWriteBytesPerSec => _diskWriteBytesPerSec.NextValue();

        public double DiskWritesPerSec => _diskWritesPerSec.NextValue();

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _percentDiskReadTime.Dispose();
                _percentDiskTime.Dispose();
                _percentDiskWriteTime.Dispose();
                _avgDiskBytesPerRead.Dispose();
                _avgDiskBytesPerTransfer.Dispose();
                _avgDiskBytesPerWrite.Dispose();
                _avgDiskQueueLength.Dispose();
                _avgDiskReadQueueLength.Dispose();
                _avgDiskSecPerRead.Dispose();
                _avgDiskSecPerTransfer.Dispose();
                _avgDiskSecPerWrite.Dispose();
                _avgDiskWriteQueueLength.Dispose();
                _currentDiskQueueLength.Dispose();
                _diskBytesPerSec.Dispose();
                _diskReadBytesPerSec.Dispose();
                _diskReadsPerSec.Dispose();
                _diskTransfersPerSec.Dispose();
                _diskWriteBytesPerSec.Dispose();
                _diskWritesPerSec.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

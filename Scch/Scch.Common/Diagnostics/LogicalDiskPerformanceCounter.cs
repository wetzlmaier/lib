using System;
using System.Diagnostics;

namespace Scch.Common.Diagnostics
{
    public class LogicalDiskPerformanceCounter : DiskPerformanceCounter, ILogicalDiskPerformanceCounter
    {
        private const string LogicalDisk = "LogicalDisk";

        private readonly PerformanceCounter _freeMegabytes;
        private readonly PerformanceCounter _percentFreeSpace;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalDiskPerformanceCounter"/> class.
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="machineName"></param>
        public LogicalDiskPerformanceCounter(string instanceName = null, string machineName = ProcessHelper.LocalMachine) : base(LogicalDisk, instanceName, machineName)
        {
            if (machineName == null)
                throw new ArgumentNullException(nameof(machineName));

            _freeMegabytes = new PerformanceCounter(LogicalDisk, "Free Megabytes", instanceName, machineName);
            _percentFreeSpace = new PerformanceCounter(LogicalDisk, "% Free Space", instanceName, machineName);
        }

        public double FreeMegabytes => _freeMegabytes.NextValue();

        public double PercentFreeSpace => _percentFreeSpace.NextValue();

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _freeMegabytes.Dispose();
                _percentFreeSpace.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

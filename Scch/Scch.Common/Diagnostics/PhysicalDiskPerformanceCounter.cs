using System;
using System.Diagnostics;

namespace Scch.Common.Diagnostics
{
    public class PhysicalDiskPerformanceCounter : DiskPerformanceCounter, IPhysicalDiskPerformanceCounter
    {
        private const string PhysicalDisk = "PhysicalDisk";

        private readonly PerformanceCounter _percentIdleTime;
        private readonly PerformanceCounter _splitIOperSecond;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalDiskPerformanceCounter"/> class.
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="machineName"></param>
        public PhysicalDiskPerformanceCounter(string instanceName = null, string machineName = ProcessHelper.LocalMachine) : base(PhysicalDisk, instanceName, machineName)
        {
            if (machineName == null)
                throw new ArgumentNullException(nameof(machineName));

            _percentIdleTime = new PerformanceCounter(PhysicalDisk, "% Idle Time", instanceName, machineName);
            _splitIOperSecond = new PerformanceCounter(PhysicalDisk, "Split IO/Sec", instanceName, machineName);
        }

        public double PercentIdleTime => _percentIdleTime.NextValue();

        public int SplitIOperSecond => (int)_splitIOperSecond.NextValue();

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _percentIdleTime.Dispose();
                _splitIOperSecond.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

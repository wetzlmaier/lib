using System.ComponentModel;

namespace Scch.Common.Diagnostics
{
    public interface IPhysicalDiskPerformanceCounter : IDiskPerformanceCounter
    {
        /// <summary>
        /// Shows the percentage of time that the disk was idle during the sample interval.
        /// </summary>
        [Description("Shows the percentage of time that the disk was idle during the sample interval.")]
        [DisplayName("% Idle Time")]
        double PercentIdleTime { get; }

        /// <summary>
        /// Shows the rate at which that I/O requests to the disk were split into multiple requests. A split I/O may result from requesting data in a size that is too large to fit into a single I/O or that the disk is fragmented on single-disk systems.
        /// </summary>
        [Description("Shows the rate at which that I/O requests to the disk were split into multiple requests. A split I/O may result from requesting data in a size that is too large to fit into a single I/O or that the disk is fragmented on single-disk systems.")]
        [DisplayName("Split IO/Sec")]
        int SplitIOperSecond { get; }
    }
}

using System.ComponentModel;

namespace Scch.Common.Diagnostics
{
    public interface ILogicalDiskPerformanceCounter : IDiskPerformanceCounter
    {
        [DisplayName("Free Megabytes")]
        [Description("Shows the unallocated space, in megabytes, on the disk drive.One megabyte is equal to 1,048,576 bytes.")]
        double FreeMegabytes { get; }

        [DisplayName("% Free Space")]
        [Description("Shows the percentage of the total usable space on the selected logical disk drive that is free.")]
        double PercentFreeSpace { get; }
    }
}

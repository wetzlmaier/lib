using System.ComponentModel;

namespace Scch.Common.Diagnostics
{
    public interface IDiskPerformanceCounter : IPerformanceCounter
    {
        [DisplayName("% Disk Read Time")]
        [Description("Shows the percentage of time that the selected disk drive is busy servicing read requests.")]
        double PercentDiskReadTime { get; }

        [DisplayName("% Disk Time")]
        [Description("Shows the percentage of elapsed time that the selected disk drive is busy servicing read or write requests.")]
        double PercentDiskTime { get; }

        [DisplayName("% Disk Write Time")]
        [Description("Shows the percentage of elapsed time that the selected disk drive is busy servicing write requests.")]
        double PercentDiskWriteTime { get; }

        [DisplayName("Avg.Disk Bytes/Read")]
        [Description("Shows the average number of bytes transferred from the disk during read operations.")]
        double AvgDiskBytesPerRead { get; }

        [DisplayName("Avg.Disk Bytes/Transfer")]
        [Description("Shows the average number of bytes transferred to or from the disk during write or read operations.")]
        double AvgDiskBytesPerTransfer { get; }

        [DisplayName("Avg.Disk Bytes/Write")]
        [Description("Shows the average number of bytes transferred to the disk during write operations.")]
        double AvgDiskBytesPerWrite { get; }

        [DisplayName("Avg.Disk Queue Length")]
        [Description("Shows the average number of both read and write requests that were queued for the selected disk during the sample interval.")]
        double AvgDiskQueueLength { get; }

        [DisplayName("Avg.Disk Read Queue Length")]
        [Description("Shows the average number of read requests that were queued for the selected disk during the sample interval.")]
        double AvgDiskReadQueueLength { get; }

        [DisplayName("Avg.Disk sec/Read")]
        [Description("Shows the average time, in seconds, of a read of data from the disk.")]
        double AvgDiskSecPerRead { get; }

        [DisplayName("Avg.Disk sec/Transfer")]
        [Description("Shows the average time, in seconds, of a disk transfer.")]
        double AvgDiskSecPerTransfer { get; }

        [DisplayName("Avg.Disk sec/Write")]
        [Description("Shows the average time, in seconds, of a write of data to the disk.")]
        double AvgDiskSecPerWrite { get; }

        [DisplayName("Avg.Disk Write Queue Length")]
        [Description("Shows the average number of write requests that were queued for the selected disk during the sample interval.")]
        double AvgDiskWriteQueueLength { get; }

        [DisplayName("Current Disk Queue Length")]
        [Description("Shows the number of requests that are outstanding on the disk at the time that the performance data is collected.It includes requests in service at the time of the collection.This is a snapshot, not an average over the time interval.It includes requests in service at the time of the collection.Multispindle disk devices can have multiple requests active at one time, but other concurrent requests are awaiting service.This counter might reflect a transitory high or low queue length, but if there is a sustained load on the disk drive, it is likely that this is consistently high. Requests experience delays proportional to the length of this queue minus the number of spindles on the disks.This difference should average less than two.")]
        int CurrentDiskQueueLength { get; }

        [DisplayName("Disk Bytes/sec")]
        [Description("Shows the rate at which bytes are transferred to or from the disk during write or read operations.")]
        double DiskBytesPerSec { get; }

        [DisplayName("Disk Read Bytes/sec")]
        [Description("Shows the rate at which bytes are transferred from the disk during read operations.")]
        double DiskReadBytesPerSec { get; }

        [DisplayName("Disk Reads/sec")]
        [Description("Shows the rate of read operations on the disk.")]
        double DiskReadsPerSec { get; }

        [DisplayName("Disk Transfers/sec")]
        [Description("Shows the rate of read and write operations on the disk.")]
        double DiskTransfersPerSec { get; }

        [DisplayName("Disk Write Bytes/sec")]
        [Description("Shows the rate at which bytes are transferred to the disk during write operations.")]
        double DiskWriteBytesPerSec { get; }

        [DisplayName("Disk Writes/sec")]
        [Description("Shows the rate of write operations on the disk.")]
        double DiskWritesPerSec { get; }
    }
}

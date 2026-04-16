using System.ComponentModel;

namespace Scch.Common.Diagnostics
{
    public interface ISystemPerformanceCounter : IPerformanceCounter
    {
        /// <summary>
        /// Shows the percentage of the Total Registry Quota Allowed that is currently being used by the system.
        /// </summary>
        [Description("Shows the percentage of the Total Registry Quota Allowed that is currently being used by the system.")]
        [DisplayName("% Registry Quota In Use")]
        double PercentRegistryQuotaInUse { get; }

        /// <summary>
        /// Shows the rate of alignment faults fixed by the system.
        /// </summary>
        [Description("Shows the rate of alignment faults fixed by the system.")]
        [DisplayName("Alignment Fixups/sec")]
        double AlignmentFixupsPerSecond { get; }

        /// <summary>
        /// Shows the combined rate at which all processors on the computer are switched from one thread to another. Context switches occur when a running thread voluntarily relinquishes the processor, is preempted by a higher priority, ready thread, or switches between user-mode and privileged (kernel) mode to use an Executive or subsystem service. It is the sum of the values of Thread\Thread: Context Switches/sec for each thread running on all processors on the computer and is measured in numbers of switches. There are context switch counters on the System and Thread objects.
        /// </summary>
        [Description("Shows the combined rate at which all processors on the computer are switched from one thread to another. Context switches occur when a running thread voluntarily relinquishes the processor, is preempted by a higher priority, ready thread, or switches between user-mode and privileged (kernel) mode to use an Executive or subsystem service. It is the sum of the values of Thread\\Thread: Context Switches/sec for each thread running on all processors on the computer and is measured in numbers of switches. There are context switch counters on the System and Thread objects.")]
        [DisplayName("Context Switches/sec")]
        double ContextSwitchesPerSecond { get; }

        /// <summary>
        /// Shows the rate at which exceptions are dispatched by the system.
        /// </summary>
        [Description("Shows the rate at which exceptions are dispatched by the system.")]
        [DisplayName("Exception Dispatches/sec")]
        double ExceptionDispatchesPerSecond { get; }

        /// <summary>
        /// Shows the overall rate at which bytes are transferred for all file system operations that are neither read nor write operations, including file system control requests and requests for information about device characteristics or status. It is measured in numbers of bytes per second.
        /// </summary>
        [Description("Shows the overall rate at which bytes are transferred for all file system operations that are neither read nor write operations, including file system control requests and requests for information about device characteristics or status. It is measured in numbers of bytes per second.")]
        [DisplayName("File Control Bytes/sec")]
        double FileControlBytesPerSecond { get; }

        /// <summary>
        /// Shows the combined rate of file system operations that are neither read nor write operations, such as file system control requests and requests for information about device characteristics or status. This is the inverse of System\System: File Data Operations/sec and is measured in numbers of operations.
        /// </summary>
        [Description("Shows the combined rate of file system operations that are neither read nor write operations, such as file system control requests and requests for information about device characteristics or status. This is the inverse of System\\System: File Data Operations/sec and is measured in numbers of operations.")]
        [DisplayName("File Control Operations/sec")]
        double FileControlOperationsPerSecond { get; }

        /// <summary>
        /// Shows the combined rate of read and write operations on all logical disks on the computer. This is the inverse of System\System: File Control Operations/sec.
        /// </summary>
        [Description("Shows the combined rate of read and write operations on all logical disks on the computer. This is the inverse of System\\System: File Control Operations/sec.")]
        [DisplayName("File Data Operations/sec")]
        double FileDataOperationsPerSecond { get; }

        /// <summary>
        /// Shows the overall rate at which bytes are read to satisfy file system read requests to all devices on the computer, including read operations from the file system cache. It is measured in numbers of bytes per second.
        /// </summary>
        [Description("Shows the overall rate at which bytes are read to satisfy file system read requests to all devices on the computer, including read operations from the file system cache. It is measured in numbers of bytes per second.")]
        [DisplayName("File Read Bytes/sec")]
        double FileReadBytesPerSecond { get; }

        /// <summary>
        /// Shows the combined rate of file system read requests to all devices on the computer, including requests to read from the file system cache. It is measured in numbers of read operations per second.
        /// </summary>
        [Description("Shows the combined rate of file system read requests to all devices on the computer, including requests to read from the file system cache. It is measured in numbers of read operations per second.")]
        [DisplayName("File Read Operations/sec")]
        double FileReadOperationsPerSecond { get; }

        /// <summary>
        /// Shows the overall rate at which bytes are written to satisfy file system write requests to all devices on the computer, including write operations to the file system cache. It is measured in numbers of bytes per second.
        /// </summary>
        [Description("Shows the overall rate at which bytes are written to satisfy file system write requests to all devices on the computer, including write operations to the file system cache. It is measured in numbers of bytes per second.")]
        [DisplayName("File Write Bytes/sec")]
        double FileWriteBytesPerSecond { get; }

        /// <summary>
        /// Shows the combined rate of file system write requests to all devices on the computer, including requests to write to data in the file system cache. It is measured in numbers of write operations per second.
        /// </summary>
        [Description("Shows the combined rate of file system write requests to all devices on the computer, including requests to write to data in the file system cache. It is measured in numbers of write operations per second.")]
        [DisplayName("File Write Operations/sec")]
        double FileWriteOperationsPerSecond { get; }

        /// <summary>
        /// Shows the rate of floating emulations performed by the system.
        /// </summary>
        [Description("Shows the rate of floating emulations performed by the system.")]
        [DisplayName("Floating Emulations/sec")]
        double FloatingEmulationPerSecond { get; }

        /// <summary>
        /// Shows the number of proceses running on the system.
        /// </summary>
        [Description("Shows the number of proceses running on the system.")]
        [DisplayName("Processes")]
        int Processes { get; }

        /// <summary>
        /// Shows the number of threads in the processor queue. There is a single queue for processor time even on computers with multiple processors. Therefore, you may need to divide this value by the number of processors servicing the workload. Unlike the disk counters, this counter shows ready threads only, not threads that are running. A sustained processor queue of greater than two threads generally indicates processor congestion.
        /// </summary>
        [Description("Shows the number of threads in the processor queue. There is a single queue for processor time even on computers with multiple processors. Therefore, you may need to divide this value by the number of processors servicing the workload. Unlike the disk counters, this counter shows ready threads only, not threads that are running. A sustained processor queue of greater than two threads generally indicates processor congestion.")]
        [DisplayName("Processor Queue Length")]
        int ProcessorQueueLength { get; }

        /// <summary>
        /// Shows the combined rate of calls to Windows 2000 system service routines by all processes running on the computer. These routines perform all of the basic scheduling and synchronization of activities on the computer, and provide access to nongraphic devices, memory management, and name space management.
        /// </summary>
        [Description("Shows the combined rate of calls to Windows 2000 system service routines by all processes running on the computer. These routines perform all of the basic scheduling and synchronization of activities on the computer, and provide access to nongraphic devices, memory management, and name space management.")]
        [DisplayName("System Calls/sec")]
        double SystemCallsPerSecond { get; }

        /// <summary>
        /// Shows the total time, in seconds, that the computer has been operational since it was last started.
        /// </summary>
        [Description("Shows the total time, in seconds, that the computer has been operational since it was last started.")]
        [DisplayName("System Up Time")]
        double SystemUpTime { get; }

        /// <summary>
        /// Shows the number of threads running on the system.
        /// </summary>
        [Description("Shows the number of threads running on the system.")]
        [DisplayName("Threads")]
        int Threads { get; }
    }
}
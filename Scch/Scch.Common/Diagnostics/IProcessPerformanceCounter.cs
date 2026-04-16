using System.ComponentModel;

namespace Scch.Common.Diagnostics
{
    public interface IProcessPerformanceCounter : IPerformanceCounter
    {
        /// <summary>
        /// Shows the percentage of elapsed time that the threads of the process have spent executing code in privileged mode. When a Windows 2000 system service is called, the service often runs in Privileged Mode to gain access to system-private data. Such data is protected from access by threads executing in user mode. Calls to the system can be explicit or implicit, such as page faults or interrupts. Unlike some early operating systems, Windows 2000 uses process boundaries for subsystem protection in addition to the traditional protection of user and privileged modes. These subsystem processes provide additional protection. Therefore, some work done by Windows 2000 on behalf of your application might appear in other subsystem processes in addition to the privileged time in your process.
        /// </summary>
        [Description("Shows the percentage of elapsed time that the threads of the process have spent executing code in privileged mode. When a Windows 2000 system service is called, the service often runs in Privileged Mode to gain access to system-private data. Such data is protected from access by threads executing in user mode. Calls to the system can be explicit or implicit, such as page faults or interrupts. Unlike some early operating systems, Windows 2000 uses process boundaries for subsystem protection in addition to the traditional protection of user and privileged modes. These subsystem processes provide additional protection. Therefore, some work done by Windows 2000 on behalf of your application might appear in other subsystem processes in addition to the privileged time in your process.")]
        [DisplayName("% Privileged Time")]
        double PercentPrivilegedTime { get; }

        /// <summary>
        /// Shows the percentage of elapsed time that all of the threads of this process used the processor to execute instructions. An instruction is the basic unit of execution in a computer; a thread is the object that executes instructions; and a process is the object created when a program is run. Code executed to handle some hardware interrupts and trap conditions are included in this count.
        /// </summary>
        [Description("Shows the percentage of elapsed time that all of the threads of this process used the processor to execute instructions. An instruction is the basic unit of execution in a computer; a thread is the object that executes instructions; and a process is the object created when a program is run. Code executed to handle some hardware interrupts and trap conditions are included in this count.")]
        [DisplayName("% Processor Time")]
        double PercentProcessorTime { get; }

        /// <summary>
        /// Shows the percentage of elapsed time that this process's threads have spent executing code in user mode. Applications, environment subsystems, and integral subsystems execute in user mode. Code executing in user mode cannot damage the integrity of the Windows NT Executive, kernel, or and device drivers. Unlike some early operating systems, Windows 2000 uses process boundaries for subsystem protection in addition to the traditional protection of user and privileged modes. These subsystem processes provide additional protection. Therefore, some work done by Windows 2000 on behalf of your application might appear in other subsystem processes in addition to the privileged time in your process.
        /// </summary>
        [Description("Shows the percentage of elapsed time that this process's threads have spent executing code in user mode. Applications, environment subsystems, and integral subsystems execute in user mode. Code executing in user mode cannot damage the integrity of the Windows NT Executive, kernel, or and device drivers. Unlike some early operating systems, Windows 2000 uses process boundaries for subsystem protection in addition to the traditional protection of user and privileged modes. These subsystem processes provide additional protection. Therefore, some work done by Windows 2000 on behalf of your application might appear in other subsystem processes in addition to the privileged time in your process.")]
        [DisplayName("% User Time")]
        double PercentUserTime { get; }

        /// <summary>
        /// Shows the rate at which page faults by the threads executing in this process are occurring. A page fault occurs when a thread refers to a virtual memory page that is not in its working set in main memory. This does not cause the page to be fetched from disk if it is on the standby list and hence already in main memory, or if it is in use by another process with whom the page is shared.
        /// </summary>
        [Description("Shows the rate at which page faults by the threads executing in this process are occurring. A page fault occurs when a thread refers to a virtual memory page that is not in its working set in main memory. This does not cause the page to be fetched from disk if it is on the standby list and hence already in main memory, or if it is in use by another process with whom the page is shared.")]
        [DisplayName("Page Faults/sec")]
        double PageFaultsPerSecond { get; }

        /// <summary>
        /// Shows the current number of bytes that this process has used in the paging file(s). Paging files are used to store pages of memory used by the process that are not contained in other files. Paging files are shared by all processes, and lack of space in paging files can prevent other processes from allocating memory.
        /// </summary>
        [Description("Shows the current number of bytes that this process has used in the paging file(s). Paging files are used to store pages of memory used by the process that are not contained in other files. Paging files are shared by all processes, and lack of space in paging files can prevent other processes from allocating memory.")]
        [DisplayName("Page File Bytes")]
        long PageFileBytes { get; }

        /// <summary>
        /// Shows the maximum number of bytes that this process has used in the paging file(s). Paging files are used to store pages of memory used by the process that are not contained in other files. Paging files are shared by all processes, and lack of space in paging files can prevent other processes from allocating memory.
        /// </summary>
        [Description("Shows the maximum number of bytes that this process has used in the paging file(s). Paging files are used to store pages of memory used by the process that are not contained in other files. Paging files are shared by all processes, and lack of space in paging files can prevent other processes from allocating memory.")]
        [DisplayName("Page File Bytes Peak")]
        long PageFileBytesPeak { get; }

        /// <summary>
        /// Shows the current base priority of this process. Threads within a process can raise and lower their own base priority relative to the process's base priority.
        /// </summary>
        [Description("Shows the current base priority of this process. Threads within a process can raise and lower their own base priority relative to the process's base priority.")]
        [DisplayName("Priority Base")]
        int PriorityBase { get; }

        /// <summary>
        /// Shows the current number of bytes that this process has allocated that cannot be shared with other processes.
        /// </summary>
        [Description("Shows the current number of bytes that this process has allocated that cannot be shared with other processes.")]
        [DisplayName("Private Bytes")]
        long PrivateBytes { get; }

        /// <summary>
        /// Shows the current number of bytes in the working set of this process. The working set is the set of memory pages touched recently by the threads in the process. If free memory in the computer is above a certain threshold, pages are left in the working set of a process even if they are not in use. When free memory falls below a certain threshold, pages are trimmed from working sets. If they are needed, they are then soft-faulted back into the working set before they leave main memory.
        /// </summary>
        [Description("Shows the current number of bytes in the working set of this process. The working set is the set of memory pages touched recently by the threads in the process. If free memory in the computer is above a certain threshold, pages are left in the working set of a process even if they are not in use. When free memory falls below a certain threshold, pages are trimmed from working sets. If they are needed, they are then soft-faulted back into the working set before they leave main memory.")]
        [DisplayName("Working Set")]
        long WorkingSet { get; }

        /// <summary>
        /// Shows the identifier of the process that created the current process. Note that the creating process may have terminated since this process was created and so this value may no longer identify a running process.
        /// </summary>
        [Description("Shows the identifier of the process that created the current process. Note that the creating process may have terminated since this process was created and so this value may no longer identify a running process.")]
        [DisplayName("Creating Process ID")]
        int CreatingProcessId { get; }

        /// <summary>
        /// Shows the total elapsed time, in seconds, that this process has been running.
        /// </summary>
        [Description("Shows the total elapsed time, in seconds, that this process has been running.")]
        [DisplayName("Elapsed Time")]
        double ElapsedTime { get; }

        /// <summary>
        /// Shows the total number of handles currently open by this process. This number is the equal to the sum of the handles currently open by each thread in this process.
        /// </summary>
        [Description("Shows the total number of handles currently open by this process. This number is the equal to the sum of the handles currently open by each thread in this process.")]
        [DisplayName("Handle Count")]
        int HandleCount { get; }

        /// <summary>
        /// Shows the unique identifier of this process. ID Process numbers are reused, so they only identify a process for the lifetime of that process.
        /// </summary>
        [Description("Shows the unique identifier of this process. ID Process numbers are reused, so they only identify a process for the lifetime of that process.")]
        [DisplayName("ID Process")]
        int ProcessId { get; }

        /// <summary>
        /// Shows the rate at which the process is reading and writing bytes in I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is reading and writing bytes in I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Data Bytes/sec")]
        double DataBytesPerSecond { get; }

        /// <summary>
        /// Shows the rate at which the process is issuing read and write I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is issuing read and write I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Data Operations/sec")]
        double DataOperationsPerSecond { get; }

        /// <summary>
        /// Shows the rate at which the process is issuing bytes to I/O operations that don't involve data such as control operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is issuing bytes to I/O operations that don't involve data such as control operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Other Bytes/sec")]
        double OtherBytesPerSecond { get; }

        /// <summary>
        /// Shows the rate at which the process is issuing I/O operations that are neither a read or a write operation. An example of this type of operation would be a control function. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is issuing I/O operations that are neither a read or a write operation. An example of this type of operation would be a control function. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Other Operations/sec")]
        double OtherOperationsPerSecond { get; }

        /// <summary>
        /// Shows the rate at which the process is reading bytes from I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is reading bytes from I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Read Bytes/sec")]
        double ReadBytesPerSecond { get; }

        /// <summary>
        /// Shows the rate at which the process is issuing read I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is issuing read I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Read Operations/sec")]
        double ReadOperationsPerSecond { get; }

        /// <summary>
        /// Shows the rate the process is writing bytes to I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate the process is writing bytes to I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Write Bytes/sec")]
        double WriteBytesPerSecond { get; }

        /// <summary>
        /// Shows the rate at which the process is issuing write I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.
        /// </summary>
        [Description("Shows the rate at which the process is issuing write I/O operations. This counter counts all I/O activity generated by the process to include file, network and device I/O's.")]
        [DisplayName("IO Write Operations/sec")]
        double WriteOperationsPerSecond { get; }

        /// <summary>
        /// Shows the number of bytes in the nonpaged pool, a system memory area where space is acquired by operating system components as they accomplish their appointed tasks. Nonpaged pool pages cannot be paged out to the paging file, but instead remain in main memory as long as they are allocated.
        /// </summary>
        [Description("Shows the number of bytes in the nonpaged pool, a system memory area where space is acquired by operating system components as they accomplish their appointed tasks. Nonpaged pool pages cannot be paged out to the paging file, but instead remain in main memory as long as they are allocated.")]
        [DisplayName("Pool Nonpaged Bytes")]
        long PoolNonpagedBytes { get; }

        /// <summary>
        /// Shows the number of bytes in the Paged Pool, a system memory area where space is acquired by operating system components as they accomplish their appointed tasks. Paged Pool pages can be paged out to the paging file when not accessed by the system for sustained periods of time.
        /// </summary>
        [Description("Shows the number of bytes in the Paged Pool, a system memory area where space is acquired by operating system components as they accomplish their appointed tasks. Paged Pool pages can be paged out to the paging file when not accessed by the system for sustained periods of time.")]
        [DisplayName("Pool Paged Bytes")]
        long PoolPagedBytes { get; }

        /// <summary>
        /// Shows the number of threads currently active in this process. An instruction is the basic unit of execution in a processor, and a thread is the object that executes instructions. Every running process has at least one thread.
        /// </summary>
        [Description("Shows the number of threads currently active in this process. An instruction is the basic unit of execution in a processor, and a thread is the object that executes instructions. Every running process has at least one thread.")]
        [DisplayName("Thread Count")]
        int ThreadCount { get; }

        /// <summary>
        /// Shows the current size, in bytes, of the virtual address space that the process is using. Use of virtual address space does not necessarily imply corresponding use of either disk or main memory pages. Virtual space is finite, and by using too much, the process can limit its ability to load libraries.
        /// </summary>
        [Description("Shows the current size, in bytes, of the virtual address space that the process is using. Use of virtual address space does not necessarily imply corresponding use of either disk or main memory pages. Virtual space is finite, and by using too much, the process can limit its ability to load libraries.")]
        [DisplayName("Virtual Bytes")]
        long VirtualBytes { get; }

        /// <summary>
        /// Shows the maximum size, in bytes, of virtual address space the process has used at any one time. Use of virtual address space does not necessarily imply corresponding use of either disk or main memory pages. However, virtual space is finite, and the process might limit its ability to load libraries by using too much.
        /// </summary>
        [Description("Shows the maximum size, in bytes, of virtual address space the process has used at any one time. Use of virtual address space does not necessarily imply corresponding use of either disk or main memory pages. However, virtual space is finite, and the process might limit its ability to load libraries by using too much.")]
        [DisplayName("Virtual Bytes Peak")]
        long VirtualBytesPeak { get; }

        /// <summary>
        /// Shows the maximum size, in bytes, in the working set of this process at any point in time. The working set is the set of memory pages touched recently by the threads in the process. If free memory in the computer is above a certain threshold, pages are left in the working set of a process even if they are not in use. When free memory falls below a certain threshold, pages are trimmed from working sets. If they are needed, they are then soft-faulted back into the working set before they leave main memory.
        /// </summary>
        [Description("Shows the maximum size, in bytes, in the working set of this process at any point in time. The working set is the set of memory pages touched recently by the threads in the process. If free memory in the computer is above a certain threshold, pages are left in the working set of a process even if they are not in use. When free memory falls below a certain threshold, pages are trimmed from working sets. If they are needed, they are then soft-faulted back into the working set before they leave main memory.")]
        [DisplayName("Working Set Peak")]
        long WorkingSetPeak { get; }
    }
}
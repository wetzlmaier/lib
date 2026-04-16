namespace Scch.Common.Diagnostics
{
    public interface IPerformanceCounter
    {
        string InstanceName { get; }

        string MachineName { get; }
    }
}

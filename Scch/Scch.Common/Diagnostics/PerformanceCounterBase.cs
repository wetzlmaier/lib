using System;

namespace Scch.Common.Diagnostics
{
    public abstract class PerformanceCounterBase : Disposable, IPerformanceCounter
    {
        private readonly string _instanceName;
        private readonly string _machineName;

        protected PerformanceCounterBase(string instanceName = null, string machineName = ProcessHelper.LocalMachine)
        {
            _instanceName = instanceName;

            if (machineName == ProcessHelper.LocalMachine || machineName.ToLower() == "localhost" || machineName == "127.0.0.1")
                machineName = Environment.MachineName;

            _machineName = machineName;
        }

        public string InstanceName => _instanceName;

        public string MachineName => _machineName;
    }
}

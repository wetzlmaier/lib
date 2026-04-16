using System.Diagnostics;
using System.Threading;

namespace Scch.Common.Threading
{
    public delegate void MonitoringDelegate(Process process, CancellationToken token);
}

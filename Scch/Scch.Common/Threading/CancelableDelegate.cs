using System.Threading;

namespace Scch.Common.Threading
{
    public delegate void CancelableDelegate(CancellationTokenSource cts);
}

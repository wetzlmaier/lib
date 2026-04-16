using System;
using System.Threading;

namespace Scch.Common.Threading
{
    public static class BatchOperation
    {
        public static void Run(CancelableDelegate worker, BlockingConditionDelegate blockingCondition = null, CancellationDelegate cancellation = null, int interval = 1000)
        {
            if (worker == null)
                throw new ArgumentNullException(nameof(worker));

            if (blockingCondition == null)
                blockingCondition = () => false;

            if (cancellation == null)
                cancellation = () => false;

            CancellationTokenSource source = new CancellationTokenSource();

            do
            {
                while (!blockingCondition())
                {
                    var now = DateTime.Now;

                    while ((DateTime.Now - now).TotalMilliseconds < interval)
                    {
                        Thread.Sleep(interval / 100);
                    }

                    worker(source);
                }
            } while (!cancellation() && !source.IsCancellationRequested);

            source.Cancel();
        }
    }
}

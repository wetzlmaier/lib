using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scch.Common.Threading
{
    public class Timer : Disposable
    {
        public event EventHandler Notify;

        private DateTime _lastHeartbeat;
        private TimeSpan _timeout;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Timer(TimeSpan timeout)
        {
            _timeout = timeout;

            _cancellationTokenSource = new CancellationTokenSource();
            Reset();
            Task.Factory.StartNew(() =>
            {
                int waitTime = (int)_timeout.TotalMilliseconds / 100;
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if (DateTime.Now - _lastHeartbeat > timeout)
                    {
                        _cancellationTokenSource.Cancel();
                        OnNotify();
                    }
                    Thread.Sleep(waitTime);
                }
            }, _cancellationTokenSource.Token);
        }

        protected void OnNotify()
        {
            EventHandler notify = Notify;
            notify?.Invoke(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Reset()
        {
            _lastHeartbeat = DateTime.Now;
        }
    }
}

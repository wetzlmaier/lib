using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Scch.Common.Net.Sockets
{
    public static class TcpHelper
    {
        private const int ChatDelay = 100;

        public static void WaitForConnect(string hostname, int port, int timeout = int.MaxValue)
        {
            CancellationTimeoutWrapper(token =>
                {
                    WaitForConnect(hostname, port, token);
                }, timeout);
        }

        private static void CancellationTimeoutWrapper(Action<CancellationToken> action, int timeout = int.MaxValue)
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                var task = Task.Factory.StartNew(() => action(cts.Token), cts.Token);

                cts.CancelAfter(timeout);
                while (!task.IsCompleted && !task.IsCanceled)
                {
                    Thread.Sleep(ChatDelay);
                }
            }
        }

        public static void WaitForConnect(string hostname, int port, CancellationToken token)
        {
            bool connected = false;
            do
            {
                try
                {
                    using (new TcpClient(hostname, port))
                    {
                        connected = true;
                    }
                }
                catch (SocketException)
                {
                }
                finally
                {
                    if (!connected)
                    {
                        Thread.Sleep(ChatDelay);
                    }
                }
            } while (!connected && !token.IsCancellationRequested);
        }

        public static void WaitForDisconnect(string hostname, int port, int timeout = int.MaxValue)
        {
            CancellationTimeoutWrapper(token =>
                {
                    WaitForDisconnect(hostname, port, token);
                }, timeout);
        }

        public static void WaitForDisconnect(string hostname, int port, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    using (new TcpClient(hostname, port))
                    {
                        Thread.Sleep(ChatDelay);
                    }
                }
            }
            catch (SocketException)
            {
            }
        }
    }
}

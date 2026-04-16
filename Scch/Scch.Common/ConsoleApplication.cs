using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scch.Common.Threading;

namespace Scch.Common
{
    public static class ConsoleApplication
    {
        public delegate Task<int> MainAsyncMethod(string[] args, CancellationToken token);

        public static BlockingConditionDelegate BlockingCondition { get; }

        public static CancellationDelegate Cancellation { get; }

        static ConsoleApplication()
        {
            BlockingCondition = () => Console.KeyAvailable;
            Cancellation = () =>
            {
                var key = Console.ReadKey(true).Key; // read next key, but discard
                return key == ConsoleKey.Escape;
            };
        }
        public static int MainAsync(MainAsyncMethod mainAsync, string[] args, CancellationTokenSource cts)
        {
            try
            {
                var task = mainAsync(args, cts.Token);

                task.Wait(cts.Token);
                return task.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public static int MainAsync(MainAsyncMethod mainAsync, string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            return MainAsync(mainAsync, args, cts);
        }

        public static void Run(CancelableDelegate worker, int interval = 0)
        {
            BatchOperation.Run(worker, BlockingCondition, Cancellation, interval);
        }

        public static string[] ReadStdIn()
        {
            string s;
            var lines = new List<string>();

            while ((s = Console.ReadLine()) != null)
            {
                lines.Add(s);
            }

            return lines.ToArray();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Threading;

namespace Scch.Common.Windows.Threading
{
    public static class DispatcherHelper
    {
        private static readonly Dispatcher Dispatcher;

        /// <summary>
        /// Initializes the framework using the current dispatcher.
        /// </summary>
        static DispatcherHelper()
        {
            Dispatcher = Application.Current.Dispatcher;
        }

        /// <summary>
        /// Executes the action on the UI thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="priority"></param>
        /// <param name="wait"></param>
        /// <param name="milliSeconds"></param>
        /// <param name="args"></param>
        public static DispatcherOperationStatus ExecuteOnUIThread(this MulticastDelegate action, bool wait = true, DispatcherPriority priority = DispatcherPriority.Background, int milliSeconds = -1, params object[] args)
        {
            if (Dispatcher.CheckAccess())
            {
                action.DynamicInvoke(args);
                return DispatcherOperationStatus.Completed;
            }

            var operation = Dispatcher.BeginInvoke(action, priority, args);
            if (wait)
                operation.Wait(TimeSpan.FromMilliseconds(milliSeconds));

            return operation.Status;
        }

        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }
    }
}

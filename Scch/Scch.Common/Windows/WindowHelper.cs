using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Scch.Common.Windows
{
    public static class WindowHelper
    {
        public static Window GetActiveWindow()
        {
            return Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.IsActive);
        }

        public static void ActivateWindow(Window window)
        {
            var interopHelper = new WindowInteropHelper(window);
            var currentForegroundWindow = User32.GetForegroundWindow();
            var thisWindowThreadId = User32.GetWindowThreadProcessId(interopHelper.Handle, IntPtr.Zero);
            var currentForegroundWindowThreadId = User32.GetWindowThreadProcessId(currentForegroundWindow, IntPtr.Zero);
            User32.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, true);
            User32.SetWindowPos(interopHelper.Handle, User32.HWNDConst.HWND_TOP, 0, 0, 0, 0, User32.SWPConst.SWP_NOSIZE | User32.SWPConst.SWP_NOMOVE | User32.SWPConst.SWP_SHOWWINDOW);
            User32.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, false);
            window.Show();
            window.Activate();
        }
        
        public static void ShowWaitCursorWhile(Action task)
        {
            ShowWaitCursorWhile(task, null);
        }

        public static void ShowWaitCursorWhile(Action task, Action continuation)
        {
            Window activeWindow = GetActiveWindow();

            Cursor previousCursor = activeWindow.Cursor;
            activeWindow.Cursor = Cursors.Wait;

            Task.Factory.StartNew(task).ContinueWith(
                delegate
                {
                    try
                    {
                        if (continuation != null)
                        {
                            activeWindow.Dispatcher.BeginInvoke(new Action(continuation.Invoke));
                        }
                    }
                    finally
                    {
                        activeWindow.Dispatcher.BeginInvoke(new Action(() => activeWindow.Cursor = previousCursor));
                    }
                });
        }

        /// <summary>
        /// Sets the window with the specified handle to HWND_TOPMOST depending on the condition.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static bool SetTopMostWindow(IntPtr handle, bool condition)
        {
            return User32.SetWindowPos(handle, condition ? User32.HWNDConst.HWND_TOPMOST : User32.HWNDConst.HWND_NOTOPMOST, 0, 0, 0, 0, User32.SWPConst.SWP_NOMOVE | User32.SWPConst.SWP_NOSIZE | User32.SWPConst.SWP_SHOWWINDOW);
        }

        /// <summary>
        /// Flashes a window
        /// </summary>
        /// <param name="hWnd">The handle to the window to flash</param>
        /// <param name="flags"></param>
        /// <param name="count"></param>
        /// <returns>whether or not the window needed flashing</returns>
        public static void FlashWindow(IntPtr hWnd, uint flags, ushort count)
        {
            var fInfo = new User32.FLASHWINFO();

            fInfo.cbSize = (ushort)Marshal.SizeOf(fInfo);
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = flags;
            fInfo.uCount = count;
            fInfo.dwTimeout = 0;

            User32.FlashWindowEx(ref fInfo);
        }

        /// <summary>
        /// Flashes a window
        /// </summary>
        /// <param name="hWnd">The handle to the window to flash</param>
        /// <param name="count"></param>
        /// <returns>whether or not the window needed flashing</returns>
        public static void FlashWindow(IntPtr hWnd, ushort count)
        {
            FlashWindow(hWnd, User32.FLASHWConst.FLASHW_ALL, 1);
        }
    }
}

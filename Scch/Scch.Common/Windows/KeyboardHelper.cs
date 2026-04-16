using System;
using System.Windows.Forms;

namespace Scch.Common.Windows
{
    public static class KeyboardHelper
    {
        public static void SendKey(IntPtr handle, Keys key)
        {
            var keyCode = Keys.C;

            uint scanCode = User32.MapVirtualKey((uint)keyCode, 0);
            var lParam = (0x00000001 | (scanCode << 16));

            User32.PostMessage(handle, WindowsMessages.WM_KEYDOWN, (IntPtr)keyCode, (IntPtr)lParam);
            User32.PostMessage(handle, WindowsMessages.WM_CHAR, (IntPtr)keyCode, (IntPtr)lParam); 
        }
    }
}

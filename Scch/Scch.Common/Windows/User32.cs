using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Functions in user32.dll
    /// </summary>
    public static class User32
    {
        /// <summary>
        /// Const for <see cref="GetWindow(IntPtr, uint)"/>.
        /// </summary>
        public enum GWConst : uint
        {
            /// <summary>
            /// The retrieved handle identifies the window of the same type that is highest in the Z order. If the specified window is a topmost window, the handle identifies the topmost window that is highest in the Z order. If the specified window is a top-level window, the handle identifies the top-level window that is highest in the Z order. If the specified window is a child window, the handle identifies the sibling window that is highest in the Z order.
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// The retrieved handle identifies the window of the same type that is lowest in the Z order. If the specified window is a topmost window, the handle identifies the topmost window that is lowest in the Z order. If the specified window is a top-level window, the handle identifies the top-level window that is lowest in the Z order. If the specified window is a child window, the handle identifies the sibling window that is lowest in the Z order.
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// The retrieved handle identifies the window below the specified window in the Z order. If the specified window is a topmost window, the handle identifies the topmost window below the specified window. If the specified window is a top-level window, the handle identifies the top-level window below the specified window. If the specified window is a child window, the handle identifies the sibling window below the specified window.
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// The retrieved handle identifies the window above the specified window in the Z order. If the specified window is a topmost window, the handle identifies the topmost window above the specified window. If the specified window is a top-level window, the handle identifies the top-level window above the specified window. If the specified window is a child window, the handle identifies the sibling window above the specified window.
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// The retrieved handle identifies the specified window's owner window, if any. 
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// The retrieved handle identifies the child window at the top of the Z order, if the specified window is a parent window; otherwise, the retrieved handle is NULL. The function examines only child windows of the specified window. It does not examine descendant windows.
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// Windows 2000/XP: The retrieved handle identifies the enabled popup window owned by the specified window (the search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled popup windows, the retrieved handle is that of the specified window. 
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /// <summary>
        /// Const for the <see cref="User32.GetWindowLong"/> method.
        /// </summary>
        public static class GWLConst
        {
            /// <summary>
            /// Retrieves the extended window styles. For more information, see CreateWindowEx. 
            /// </summary>
            public const int GWL_EXSTYLE = (-20);
            /// <summary>
            /// Retrieves the window styles.
            /// </summary>
            public const int GWL_STYLE = (-16);
            /// <summary>
            /// Retrieves the address of the window procedure, or a handle representing the address of the window procedure. You must use the CallWindowProc function to call the window procedure.
            /// </summary>
            public const int GWL_WNDPROC = (-4);
            /// <summary>
            /// Retrieves a handle to the application instance.
            /// </summary>
            public const int GWL_HINSTANCE = (-6);
            /// <summary>
            /// Retrieves a handle to the parent window, if any.
            /// </summary>
            public const int GWL_HWNDPARENT = (-8);
            /// <summary>
            /// Retrieves the identifier of the window.
            /// </summary>
            public const int GWL_ID = (-12);
            /// <summary>
            /// Retrieves the user data associated with the window. This data is intended for use by the application that created the window. Its value is initially zero.
            /// The following values are also available when the hWnd parameter identifies a dialog box.
            /// </summary>
            public const int GWL_USERDATA = (-21);
        }


        /// <summary>
        /// Const for window styles.
        /// </summary>
        public static class WindowStyles
        {
            /// <summary>
            /// Creates a window that has a thin-line border.
            /// </summary>
            public const uint WS_BORDER = 0x00800000;
            /// <summary>
            /// Creates a window that has a title bar (includes the WS_BORDER style).
            /// </summary>
            public const uint WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
            /// <summary>
            /// Creates a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
            /// </summary>
            public const uint WS_CHILD = 0x40000000;
            /// <summary>
            /// Same as the WS_CHILD style.
            /// </summary>
            public const uint WS_CHILDWINDOW = WS_CHILD;
            /// <summary>
            /// Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the 
            /// parent window.
            /// </summary>
            public const uint WS_CLIPCHILDREN = 0x02000000;
            /// <summary>
            /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, 
            /// the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. 
            /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, 
            /// to draw within the client area of a neighboring child window.
            /// </summary>
            public const uint WS_CLIPSIBLINGS = 0x04000000;
            /// <summary>
            /// Creates a window that is initially disabled. A disabled window cannot receive input from the user. To change this after a window has 
            /// been created, use EnableWindow. 
            /// </summary>
            public const uint WS_DISABLED = 0x08000000;
            /// <summary>
            /// Creates a window that has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
            /// </summary>
            public const uint WS_DLGFRAME = 0x00400000;
            /// <summary>
            /// Specifies the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys. 
            /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use SetWindowLong.
            /// </summary>
            public const uint WS_GROUP = 0x00020000;
            /// <summary>
            /// Creates a window that has a horizontal scroll bar.
            /// </summary>
            public const uint WS_HSCROLL = 0x00100000;
            /// <summary>
            /// Creates a window that is initially minimized. Same as the WS_MINIMIZE style.
            /// </summary>
            public const uint WS_ICONIC = WS_MINIMIZE;
            /// <summary>
            /// Creates a window that is initially maximized.
            /// </summary>
            public const uint WS_MAXIMIZE = 0x01000000;
            /// <summary>
            /// Creates a window that has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
            /// </summary>
            public const uint WS_MAXIMIZEBOX = 0x00010000;
            /// <summary>
            /// Creates a window that is initially minimized. Same as the WS_ICONIC style.
            /// </summary>
            public const uint WS_MINIMIZE = 0x20000000;
            /// <summary>
            /// Creates a window that has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified. 
            /// </summary>
            public const uint WS_MINIMIZEBOX = 0x00020000;
            /// <summary>
            /// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
            /// </summary>
            public const uint WS_OVERLAPPED = 0x00000000;
            /// <summary>
            /// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_TILEDWINDOW style. 
            /// </summary>
            public const uint WS_OVERLAPPEDWINDOW =
                (WS_OVERLAPPED |
                  WS_CAPTION |
                  WS_SYSMENU |
                  WS_THICKFRAME |
                  WS_MINIMIZEBOX |
                  WS_MAXIMIZEBOX);
            /// <summary>
            /// Creates a pop-up window. This style cannot be used with the WS_CHILD style.
            /// </summary>
            public const uint WS_POPUP = 0x80000000;
            /// <summary>
            /// Creates a pop-up window with WS_BORDER, WS_POPUP, and WS_SYSMENU styles. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.
            /// </summary>
            public const uint WS_POPUPWINDOW =
                (WS_POPUP |
                  WS_BORDER |
                  WS_SYSMENU);
            /// <summary>
            /// Creates a window that has a sizing border. Same as the WS_THICKFRAME style.
            /// </summary>
            public const uint WS_SIZEBOX = WS_THICKFRAME;
            /// <summary>
            /// Creates a window that has a window menu on its title bar. The WS_CAPTION style must also be specified.
            /// </summary>
            public const uint WS_SYSMENU = 0x00080000;
            /// <summary>
            /// Specifies a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
            /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use SetWindowLong.
            /// </summary>
            public const uint WS_TABSTOP = 0x00010000;
            /// <summary>
            /// Creates a window that has a sizing border. Same as the WS_SIZEBOX style.
            /// </summary>
            public const uint WS_THICKFRAME = 0x00040000;
            /// <summary>
            /// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style. 
            /// </summary>
            public const uint WS_TILED = WS_OVERLAPPED;
            /// <summary>
            /// Creates an overlapped window with the WS_OVERLAPPED, WS_CAPTION, WS_SYSMENU, WS_THICKFRAME, WS_MINIMIZEBOX, and WS_MAXIMIZEBOX styles. Same as the WS_OVERLAPPEDWINDOW style. 
            /// </summary>
            public const uint WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;
            /// <summary>
            /// Creates a window that is initially visible.
            /// This style can be turned on and off by using ShowWindow or SetWindowPos.
            /// </summary>
            public const uint WS_VISIBLE = 0x10000000;
            /// <summary>
            /// Creates a window that has a vertical scroll bar.
            /// </summary>
            public const uint WS_VSCROLL = 0x00200000;
        }


        /// <summary>
        /// Const for extended window styles.
        /// </summary>
        public static class WindowStylesEx
        {
            /// <summary>
            /// Specifies that a window created with this style accepts drag-drop files.
            /// </summary>
            public const uint WS_EX_ACCEPTFILES = 0x00000010;
            /// <summary>
            /// Forces a top-level window onto the taskbar when the window is visible. 
            /// </summary>
            public const uint WS_EX_APPWINDOW = 0x00040000;
            /// <summary>
            /// Specifies that a window has a border with a sunken edge.
            /// </summary>
            public const uint WS_EX_CLIENTEDGE = 0x00000200;
            /// <summary>
            /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using double-buffering. 
            /// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. 
            /// </summary>
            public const uint WS_EX_COMPOSITED = 0x02000000;
            /// <summary>
            /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
            /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
            /// </summary>
            public const uint WS_EX_CONTEXTHELP = 0x00000400;
            /// <summary>
            /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
            /// </summary>
            public const uint WS_EX_CONTROLPARENT = 0x00010000;
            /// <summary>
            /// Creates a window that has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
            /// </summary>
            public const uint WS_EX_DLGMODALFRAME = 0x00000001;
            /// <summary>
            /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows. Also, this cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. 
            /// </summary>
            public const uint WS_EX_LAYERED = 0x00080000;
            /// <summary>
            /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left. 
            /// </summary>
            public const uint WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
            /// <summary>
            /// Creates a window that has generic left-aligned properties. This is the default.
            /// </summary>
            public const uint WS_EX_LEFT = 0x00000000;
            /// <summary>
            /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
            /// </summary>
            public const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
            /// <summary>
            /// The window text is displayed using left-to-right reading-order properties. This is the default.
            /// </summary>
            public const uint WS_EX_LTRREADING = 0x00000000;
            /// <summary>
            /// Creates a multiple-document interface (MDI) child window.
            /// </summary>
            public const uint WS_EX_MDICHILD = 0x00000040;
            /// <summary>
            /// Windows 2000/XP: A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
            /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
            /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
            /// </summary>
            public const uint WS_EX_NOACTIVATE = 0x08000000;
            /// <summary>
            /// Windows 2000/XP: A window created with this style does not pass its window layout to its child windows.
            /// </summary>
            public const uint WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
            /// <summary>
            /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
            /// </summary>
            public const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
            /// <summary>
            /// Combines the WS_EX_CLIENTEDGE and WS_EX_WINDOWEDGE styles.
            /// </summary>
            public const uint WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
            /// <summary>
            /// Combines the WS_EX_WINDOWEDGE, WS_EX_TOOLWINDOW, and WS_EX_TOPMOST styles.
            /// </summary>
            public const uint WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
            /// <summary>
            /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
            /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles. 
            /// </summary>
            public const uint WS_EX_RIGHT = 0x00001000;
            /// <summary>
            /// Vertical scroll bar (if present) is to the right of the client area. This is the default.
            /// </summary>
            public const uint WS_EX_RIGHTSCROLLBAR = 0x00000000;
            /// <summary>
            /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
            /// </summary>
            public const uint WS_EX_RTLREADING = 0x00002000;
            /// <summary>
            /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.
            /// </summary>
            public const uint WS_EX_STATICEDGE = 0x00020000;
            /// <summary>
            /// Creates a tool window; that is, a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE. 
            /// </summary>
            public const uint WS_EX_TOOLWINDOW = 0x00000080;
            /// <summary>
            /// Specifies that a window created with this style should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
            /// </summary>
            public const uint WS_EX_TOPMOST = 0x00000008;
            /// <summary>
            /// Specifies that a window created with this style should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted.
            /// To achieve transparency without these restrictions, use the SetWindowRgn function.
            /// </summary>
            public const uint WS_EX_TRANSPARENT = 0x00000020;
            /// <summary>
            /// Specifies that a window has a border with a raised edge.
            /// </summary>
            public const uint WS_EX_WINDOWEDGE = 0x00000100;
        }

        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, int wParam, string lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, int wParam, int lParam);


        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, StringBuilder lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, String lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, ref Structures.POINT lParam);

        /// <summary>
        /// Posts the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(HandleRef hWnd, uint msg, int wParam, string lParam);

        /// <summary>
        /// Posts the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(HandleRef hWnd, uint msg, int wParam, int lParam);


        /// <summary>
        /// Posts the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Posts the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, StringBuilder lParam);

        /// <summary>
        /// Posts the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, String lParam);

        /// <summary>
        /// Posts the specified message to a window or windows. It calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The windows handle.</param>
        /// <param name="msg">The message constant. <see cref="WindowsMessages"/></param>
        /// <param name="wParam">The wParam of the message.</param>
        /// <param name="lParam">The lParam of the message.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, ref Structures.POINT lParam);

        /// <summary>
        /// Returns the currect active window handle.
        /// </summary>
        /// <returns>The currect active window handle.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();

        /// <summary>
        /// Returns the handle of the focues control.
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// Sets the focus to the control specified by its handle.
        /// </summary>
        /// <param name="handleRef">The handle.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SetFocus(HandleRef handleRef);

        /// <summary>
        /// The GetWindow function retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window. 
        /// </summary>
        /// <param name="hWnd">Handle to a window. The window handle retrieved is relative to this window, based on the value of the uCmd parameter.</param>
        /// <param name="uCmd">Specifies the relationship between the specified window and the window whose handle is to be retrieved. <see cref="GWConst"/>.</param>
        /// <returns>If the function succeeds, the return value is a window handle. If no window exists with the specified relationship to the specified window, the return value is NULL. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        /// <summary>
        /// The GetWindow function retrieves a handle to a window that has the specified relationship (Z-Order or owner) to the specified window. 
        /// </summary>
        /// <param name="handleRef">Handle to a window. The window handle retrieved is relative to this window, based on the value of the uCmd parameter.</param>
        /// <param name="uCmd">Specifies the relationship between the specified window and the window whose handle is to be retrieved. <see cref="GWConst"/>.</param>
        /// <returns>If the function succeeds, the return value is a window handle. If no window exists with the specified relationship to the specified window, the return value is NULL. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(HandleRef handleRef, uint uCmd);

        /// <summary>
        /// The GetWindowRect function retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen. 
        /// </summary>
        /// <param name="hWnd">Handle to the window. </param>
        /// <param name="lpRect">Pointer to a structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(HandleRef hWnd, out Structures.RECT lpRect);

        /// <summary>
        /// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area. 
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="X">Specifies the new position of the left side of the window.</param>
        /// <param name="Y">Specifies the new position of the top of the window.</param>
        /// <param name="nWidth">Specifies the new width of the window.</param>
        /// <param name="nHeight">Specifies the new height of the window.</param>
        /// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public static bool MoveWindow(IntPtr hWnd, Rectangle rectangle, bool bRepaint)
        {
            return MoveWindow(hWnd, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, bRepaint);
        }

        /// <summary>
        /// The SetWindowLong function changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
        /// <param name="dwNewLong">Specifies the replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
        /// If the previous value of the specified 32-bit integer is zero, and the function succeeds, the return value is zero, but the function does not clear the last error information. This makes it difficult to determine success or failure. To deal with this, you should clear the last error information by calling SetLastError(0) before calling SetWindowLong. Then, function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        /// <summary>
        /// The SetWindowLong function changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
        /// <param name="dwNewLong">Specifies the replacement value.</param>
        /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
        /// If the previous value of the specified 32-bit integer is zero, and the function succeeds, the return value is zero, but the function does not clear the last error information. This makes it difficult to determine success or failure. To deal with this, you should clear the last error information by calling SetLastError(0) before calling SetWindowLong. Then, function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        /// <summary>
        /// The GetWindowLong function retrieves information about the specified window. The function also retrieves the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="nIndex">Specifies the zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer.</param>
        /// <returns>If the function succeeds, the return value is the requested 32-bit value. If the function fails, the return value is zero. To get extended error information, call GetLastError. 
        /// If SetWindowLong has not been called previously, GetWindowLong returns zero for values in the extra window or class memory.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// The ShowScrollBar function shows or hides the specified scroll bar. 
        /// </summary>
        /// <param name="hWnd">Handle to a scroll bar control or a window with a standard scroll bar, depending on the value of the wBar parameter.</param>
        /// <param name="wBar">Specifies the scroll bar(s) to be shown or hidden.</param>
        /// <param name="bShow">Specifies whether the scroll bar is shown or hidden. If this parameter is TRUE, the scroll bar is shown; otherwise, it is hidden.</param>
        /// <returns>If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        /// <summary>
        /// Const for <see cref="User32.SetWindowPos"/>
        /// </summary>
        public static class HWNDConst
        {
            public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
            public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
            public static readonly IntPtr HWND_TOP = new IntPtr(0);
        }

        /// <summary>
        /// Const for <see cref="User32.SetWindowPos"/>
        /// </summary>
        public static class SWPConst
        {
            public const UInt32 SWP_NOSIZE = 0x0001;
            public const UInt32 SWP_NOMOVE = 0x0002;
            public const UInt32 SWP_SHOWWINDOW = 0x0040;
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        public static class SWConst
        {
            public const int SW_HIDE = 0;
            public const int SW_SHOWNORMAL = 1;
            public const int SW_SHOWMINIMIZED = 2;
            public const int SW_SHOWMAXIMIZED = 3;
            public const int SW_MAXIMIZE = 3;
            public const int SW_SHOWNOACTIVATE = 4;
            public const int SW_SHOW = 5;
            public const int SW_MINIMIZE = 6;
            public const int SW_SHOWMINNOACTIVE = 7;
            public const int SW_SHOWNA = 8;
            public const int SW_RESTORE = 9;
            public const int SW_SHOWDEFAULT = 10;
            public const int SW_FORCEMINIMIZE = 11;
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern Int16 FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt16 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt16 uCount;
            public UInt32 dwTimeout;
        }

        /// <summary>
        /// Const for <see cref="FlashWindowEx"/>.
        /// </summary>
        public static class FLASHWConst
        {
            public const UInt32 FLASHW_STOP = 0;
            public const UInt32 FLASHW_CAPTION = 0x00000001;
            public const UInt32 FLASHW_TRAY = 0x00000002;
            public const UInt32 FLASHW_ALL = (FLASHW_CAPTION | FLASHW_TRAY);
            public const UInt32 FLASHW_TIMER = 0x00000004;
            public const UInt32 FLASHW_TIMERNOFG = 0x0000000C;
        }

        /// <summary>
        /// Const for <see cref="User32.SetWindowsHookEx"/>
        /// </summary>
        public static class HookConst
        {
            public const int WH_CALLWNDPROC = 4;
            public const int WH_CALLWNDPROCRET = 12;
            public const int WH_CBT = 5;
            public const int WH_DEBUG = 9;
            public const int WH_FOREGROUNDIDLE = 11;
            public const int WH_GETMESSAGE = 3;
            public const int WH_HARDWARE = 8;
            public const int WH_JOURNALPLAYBACK = 1;
            public const int WH_JOURNALRECORD = 0;
            public const int WH_KEYBOARD = 2;
            public const int WH_KEYBOARD_LL = 13;
            public const int WH_MAX = 15;
            public const int WH_MIN = -1;
            public const int WH_MOUSE = 7;
            public const int WH_MOUSE_LL = 14;
            public const int WH_MSGFILTER = -1;
            public const int WH_SHELL = 10;
            public const int WH_SYSMSGFILTER = 6;
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
                                                    IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public static class BFFMConst
        {
            static BFFMConst()
            {
                BFFM_SETSELECTION = Marshal.SystemDefaultCharSize == 1 ? BFFM_SETSELECTIONA : BFFM_SETSELECTIONW;
            }

            public const uint BFFM_ENABLEOK = 0x465;
            public const uint BFFM_INITIALIZED = 1;
            public const uint BFFM_SELCHANGED = 2;
            public static readonly uint BFFM_SETSELECTION;
            public const uint BFFM_SETSELECTIONA = 0x466;
            public const uint BFFM_SETSELECTIONW = 0x467;
        }

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public static class MKConst
        {
            /// <summary>
            /// The CTRL key is down.
            /// </summary>
            public const uint MK_CONTROL = 0x0008;

            /// <summary>
            /// The left mouse button is down.
            /// </summary>
            public const uint MK_LBUTTON = 0x0001;

            /// <summary>
            /// The middle mouse button is down.
            /// </summary>
            public const uint MK_MBUTTON = 0x0010;

            /// <summary>
            /// The right mouse button is down.
            /// </summary>
            public const uint MK_RBUTTON = 0x0002;

            /// <summary>
            /// The SHIFT key is down.
            /// </summary>
            public const uint MK_SHIFT = 0x0004;

            /// <summary>
            /// The first X button is down.
            /// </summary>
            public const uint MK_XBUTTON1 = 0x0020;

            /// <summary>
            /// The second X button is down.
            /// </summary>
            public const uint MK_XBUTTON2 = 0x0040;

        }

        public enum GuiResourcesFlags : uint
        {
            GdiObjects = 0,
            UserObjects = 1,
            GdiObjectsPeak = 2,
            UserObjectsPeak = 4,
        }

        [DllImport("User32")]
        public static extern uint GetGuiResources(IntPtr hProcess, GuiResourcesFlags uiFlags);
    }
}

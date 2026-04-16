using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Prompts the user to select a folder.
    /// </summary>
    public sealed class FolderBrowserDialog : CommonDialog
    {
        #region Fields

        [SecurityCritical, SecuritySafeCritical]
        Shell32.FolderBrowserOptions _dialogOptions;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderBrowserDialog"/> class.
        /// </summary>
        [SecurityCritical]
        public FolderBrowserDialog()
        {
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the properties of a common dialog to their default values.
        /// </summary>
        [SecurityCritical]
        public override void Reset()
        {
            new FileIOPermission(PermissionState.Unrestricted).Demand();

            Initialize();
        }

        /// <summary>
        /// Displays the folder browser dialog.
        /// </summary>
        /// <param name="hwndOwner">Handle to the window that owns the dialog box.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned; otherwise, false.
        /// </returns>
        [SecurityCritical]
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            bool result = false;

            IntPtr pidlRoot = IntPtr.Zero,
                   pszPath = IntPtr.Zero,
                   pidlSelected = IntPtr.Zero;

            try
            {
                Shell32.SHGetFolderLocation(hwndOwner, (int)RootFolder, IntPtr.Zero, 0, out pidlRoot);

                var browseInfo = new Shell32.BROWSEINFO
                {
                    HwndOwner = hwndOwner,
                    Root = pidlRoot,
                    DisplayName = new string(' ', 256),
                    Title = Title,
                    Flags = (uint)_dialogOptions,
                    LParam = 0,
                    Callback = HookProc
                };

                // Show dialog
                pidlSelected = Shell32.SHBrowseForFolder(ref browseInfo);

                if (pidlSelected != IntPtr.Zero)
                {
                    result = true;

                    pszPath = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
                    Shell32.SHGetPathFromIDList(pidlSelected, pszPath);

                    SelectedPath = Marshal.PtrToStringAuto(pszPath);
                }
            }
            finally // release all unmanaged resources
            {
                Shell32.IMalloc malloc = Shell32.GetSHMalloc();

                if (pidlRoot != IntPtr.Zero)
                {
                    malloc.Free(pidlRoot);
                }

                if (pidlSelected != IntPtr.Zero)
                {
                    malloc.Free(pidlSelected);
                }

                if (pszPath != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pszPath);
                }

                Marshal.ReleaseComObject(malloc);
            }

            return result;
        }

        protected override IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            switch ((uint)msg)
            {
                case User32.BFFMConst.BFFM_INITIALIZED:
                    if (!string.IsNullOrEmpty(SelectedPath))
                    {
                        User32.SendMessage(new HandleRef(null, hwnd), User32.BFFMConst.BFFM_SETSELECTION, 1, SelectedPath);
                    }
                    break;

                case User32.BFFMConst.BFFM_SELCHANGED:
                    {
                        IntPtr pidl = lParam;
                        if (pidl != IntPtr.Zero)
                        {
                            IntPtr pszPath = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
                            bool flag = Shell32.SHGetPathFromIDList(pidl, pszPath);
                            Marshal.FreeHGlobal(pszPath);
                            User32.SendMessage(new HandleRef(null, hwnd), User32.BFFMConst.BFFM_ENABLEOK, 0, flag ? 1 : 0);
                        }
                        break;
                    }
                default:
                    return base.HookProc(hwnd, msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }

        [SecurityCritical]
        private void Initialize()
        {
            RootFolder = Environment.SpecialFolder.Desktop;
            Title = string.Empty;

            // default options
            _dialogOptions = Shell32.FolderBrowserOptions.BrowseFiles
                | Shell32.FolderBrowserOptions.ShowEditBox
                | Shell32.FolderBrowserOptions.UseNewStyle
                | Shell32.FolderBrowserOptions.BrowseShares
                | Shell32.FolderBrowserOptions.ShowStatusText
                | Shell32.FolderBrowserOptions.ValidateResult;
        }

        private bool GetOption(Shell32.FolderBrowserOptions option)
        {
            return ((_dialogOptions & option) != Shell32.FolderBrowserOptions.None);
        }

        [SecurityCritical]
        private void SetOption(Shell32.FolderBrowserOptions option, bool value)
        {
            if (value)
            {
                _dialogOptions |= option;
            }
            else
            {
                _dialogOptions &= ~option;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the root folder where the browsing starts from.
        /// </summary>
        /// <value>The root special folder.</value>
        public Environment.SpecialFolder RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        /// <value>The display name.</value>
        public string SelectedPath { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether browsing files is allowed.
        /// </summary>
        /// <value></value>
        public bool BrowseFiles
        {
            get { return GetOption(Shell32.FolderBrowserOptions.BrowseFiles); }
            [SecurityCritical]
            set { SetOption(Shell32.FolderBrowserOptions.BrowseFiles, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show an edit box.
        /// </summary>
        /// <value></value>
        public bool ShowEditBox
        {
            get { return GetOption(Shell32.FolderBrowserOptions.ShowEditBox); }
            [SecurityCritical]
            set { SetOption(Shell32.FolderBrowserOptions.ShowEditBox, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether browsing shares is allowed.
        /// </summary>
        /// <value></value>
        public bool BrowseShares
        {
            get { return GetOption(Shell32.FolderBrowserOptions.BrowseShares); }
            [SecurityCritical]
            set { SetOption(Shell32.FolderBrowserOptions.BrowseShares, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show status text.
        /// </summary>
        /// <value></value>
        public bool ShowStatusText
        {
            get { return GetOption(Shell32.FolderBrowserOptions.ShowStatusText); }
            [SecurityCritical]
            set { SetOption(Shell32.FolderBrowserOptions.ShowStatusText, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to validate the result.
        /// </summary>
        /// <value></value>
        public bool ValidateResult
        {
            get { return GetOption(Shell32.FolderBrowserOptions.ValidateResult); }
            [SecurityCritical]
            set { SetOption(Shell32.FolderBrowserOptions.ValidateResult, value); }
        }
        #endregion
    }
}

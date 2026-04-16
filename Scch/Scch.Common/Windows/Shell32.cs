using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Functions in shell32.dll
    /// </summary>
    public static class Shell32
    {
        /// <summary>
        /// Flags for <see cref="Shell32.SHGetFileInfo"/>
        /// </summary>
        public static class SHGFIConst
        {
            /// <summary>
            /// Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the hIcon member of the structure specified by psfi, and the index is copied to the iIcon member.
            /// </summary>
            public const int SHGFI_ICON = 0x000000100;
            /// <summary>
            /// Retrieve the display name for the file. The name is copied to the szDisplayName member of the structure specified in psfi. The returned display name uses the long file name, if there is one, rather than the 8.3 form of the file name.
            /// </summary>
            public const int SHGFI_DISPLAYNAME = 0x000000200;
            /// <summary>
            /// Retrieve the string that describes the file's type. The string is copied to the szTypeName member of the structure specified in psfi.
            /// </summary>
            public const int SHGFI_TYPENAME = 0x000000400;
            /// <summary>
            /// Retrieve the item attributes. The attributes are copied to the dwAttributes member of the structure specified in the psfi parameter. These are the same attributes that are obtained from IShellFolder::GetAttributesOf.
            /// </summary>
            public const int SHGFI_ATTRIBUTES = 0x000000800;
            /// <summary>
            /// Retrieve the name of the file that contains the icon representing the file specified by pszPath, as returned by the IExtractIcon::GetIconLocation method of the file's icon handler. Also retrieve the icon index within that file. The name of the file containing the icon is copied to the szDisplayName member of the structure specified by psfi. The icon's index is copied to that structure's iIcon member.
            /// </summary>
            public const int SHGFI_ICONLOCATION = 0x000001000;
            /// <summary>
            /// Retrieve the type of the executable file if pszPath identifies an executable file. The information is packed into the return value. This flag cannot be specified with any other flags.
            /// </summary>
            public const int SHGFI_EXETYPE = 0x000002000;
            /// <summary>
            /// Retrieve the index of a system image list icon. If successful, the index is copied to the iIcon member of psfi. The return value is a handle to the system image list. Only those images whose indices are successfully copied to iIcon are valid. Attempting to access other images in the system image list will result in undefined behavior.
            /// </summary>
            public const int SHGFI_SYSICONINDEX = 0x000004000;
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to add the link overlay to the file's icon. The SHGFI_ICON flag must also be set.
            /// </summary>
            public const int SHGFI_LINKOVERLAY = 0x000008000;
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to blend the file's icon with the system highlight color. The SHGFI_ICON flag must also be set.
            /// </summary>
            public const int SHGFI_SELECTED = 0x000010000;
            /// <summary>
            /// Modify SHGFI_ATTRIBUTES to indicate that the dwAttributes member of the SHFILEINFO structure at psfi contains the specific attributes that are desired. These attributes are passed to IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified with the SHGFI_ICON flag.
            /// </summary>
            public const int SHGFI_ATTR_SPECIFIED = 0x000020000;
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON flag must also be set.
            /// </summary>
            public const int SHGFI_LARGEICON = 0x000000000;
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
            /// </summary>
            public const int SHGFI_SMALLICON = 0x000000001;
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve the file's open icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains the file's small open icon. A container object displays an open icon to indicate that the container is open. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
            /// </summary>
            public const int SHGFI_OPENICON = 0x000000002;
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve a Shell-sized icon. If this flag is not specified the function sizes the icon according to the system metric values. The SHGFI_ICON flag must also be set.
            /// </summary>
            public const int SHGFI_SHELLICONSIZE = 0x000000004;
            /// <summary>
            /// Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path name.
            /// </summary>
            public const int SHGFI_PIDL = 0x000000008;
            /// <summary>
            /// Indicates that the function should not attempt to access the file specified by pszPath. Rather, it should act as if the file specified by pszPath exists with the file attributes passed in dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or SHGFI_PIDL flags.
            /// </summary>
            public const int SHGFI_USEFILEATTRIBUTES = 0x000000010;

            /// <summary>
            /// Version 5.0. Apply the appropriate overlays to the file's icon. The SHGFI_ICON flag must also be set.
            /// </summary>
            public const int SHGFI_ADDOVERLAYS = 0x000000020;
            /// <summary>
            /// Version 5.0. Return the index of the overlay icon. The value of the overlay index is returned in the upper eight bits of the iIcon member of the structure specified by psfi. This flag requires that the SHGFI_ICON be set as well.
            /// </summary>
            public const int SHGFI_OVERLAYINDEX = 0x000000040;
        }

        /// <summary>
        /// Contains information about a file object. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            /// <summary>
            /// A handle to the icon that represents the file. You are responsible for destroying this handle with DestroyIcon when you no longer need it. 
            /// </summary>
            public IntPtr hIcon;
            /// <summary>
            /// The index of the icon image within the system image list. 
            /// </summary>
            public IntPtr iIcon;
            /// <summary>
            /// An array of values that indicates the attributes of the file object. For information about these values, see the IShellFolder::GetAttributesOf method. 
            /// </summary>
            public uint dwAttributes;
            /// <summary>
            /// A string that contains the name of the file as it appears in the Microsoft Windows Shell, or the path and file name of the file that contains the icon representing the file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            /// <summary>
            /// A string that describes the type of file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [Flags]
        public enum FolderBrowserOptions
        {
            /// <summary>
            /// None.
            /// </summary>
            None = 0,
            /// <summary>
            /// For finding a folder to start document searching
            /// </summary>
            FolderOnly = 0x0001,
            /// <summary>
            /// For starting the Find Computer
            /// </summary>
            FindComputer = 0x0002,
            /// <summary>
            /// Top of the dialog has 2 lines of text for BROWSEINFO.lpszTitle and 
            /// one line if this flag is set.  Passing the message 
            /// BFFM_SETSTATUSTEXTA to the hwnd can set the rest of the text.  
            /// This is not used with BIF_USENEWUI and BROWSEINFO.lpszTitle gets
            /// all three lines of text.
            /// </summary>
            ShowStatusText = 0x0004,
            ReturnAncestors = 0x0008,
            /// <summary>
            /// Add an editbox to the dialog
            /// </summary>
            ShowEditBox = 0x0010,
            /// <summary>
            /// insist on valid result (or CANCEL)
            /// </summary>
            ValidateResult = 0x0020,
            /// <summary>
            /// Use the new dialog layout with the ability to resize
            /// Caller needs to call OleInitialize() before using this API
            /// </summary>
            UseNewStyle = 0x0040,
            UseNewStyleWithEditBox = (UseNewStyle | ShowEditBox),
            /// <summary>
            /// Allow URLs to be displayed or entered. (Requires BIF_USENEWUI)
            /// </summary>
            AllowUrls = 0x0080,
            /// <summary>
            /// Add a UA hint to the dialog, in place of the edit box. May not be
            /// combined with BIF_EDITBOX.
            /// </summary>
            ShowUsageHint = 0x0100,
            /// <summary>
            /// Do not add the "New Folder" button to the dialog.  Only applicable 
            /// with BIF_NEWDIALOGSTYLE.
            /// </summary>
            HideNewFolderButton = 0x0200,
            /// <summary>
            /// don't traverse target as shortcut
            /// </summary>
            GetShortcuts = 0x0400,
            /// <summary>
            /// Browsing for Computers.
            /// </summary>
            BrowseComputers = 0x1000,
            /// <summary>
            /// Browsing for Printers.
            /// </summary>
            BrowsePrinters = 0x2000,
            /// <summary>
            /// Browsing for Everything
            /// </summary>
            BrowseFiles = 0x4000,
            /// <summary>
            /// sharable resources displayed (remote shares, requires BIF_USENEWUI)
            /// </summary>
            BrowseShares = 0x8000
        }

        /// <summary>
        /// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
        /// </summary>
        /// <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid. 
        /// If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely identifies the file within the Shell's namespace. The pointer to an item identifier list (PIDL) must be a fully qualified PIDL. Relative PIDLs are not allowed.
        /// If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have to be a valid file name. The function will proceed as if the file exists with the specified name and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain information about a file type by passing just the extension for pszPath and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.
        /// This string can use either short (the 8.3 form) or long file names.</param>
        /// <param name="dwFileAttributes">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="psfi">The address of a <see cref="SHFILEINFO"/> structure to receive the file information.</param>
        /// <param name="cbSizeFileInfo">The size, in bytes, of the <see cref="SHFILEINFO"/> structure pointed to by the psfi parameter.</param>
        /// <param name="uFlags"></param>
        /// <returns>The flags that specify the file information to retrieve.</returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath,
                                    uint dwFileAttributes,
                                    ref SHFILEINFO psfi,
                                    uint cbSizeFileInfo,
                                    uint uFlags);

        [SecurityCritical, DllImport("shell32")]
        public static extern int SHGetFolderLocation(IntPtr hwndOwner, Int32 nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

        [SecurityCritical, DllImport("shell32")]
        public static extern int SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)]string pszName, IntPtr pbc, out IntPtr ppidl, uint sfgaoIn, out uint psfgaoOut);

        [SecurityCritical, DllImport("shell32")]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lbpi);

        [SecurityCritical, DllImport("shell32", CharSet = CharSet.Auto)]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            /// <summary>
            /// Handle to the owner window for the dialog box.
            /// </summary>
            public IntPtr HwndOwner;

            /// <summary>
            /// Pointer to an item identifier list (PIDL) specifying the 
            /// location of the root folder from which to start browsing.
            /// </summary>
            public IntPtr Root;

            /// <summary>
            /// Address of a buffer to receive the display name of the
            /// folder selected by the user.
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string DisplayName;

            /// <summary>
            /// Address of a null-terminated string that is displayed 
            /// above the tree view control in the dialog box.
            /// </summary>
            [MarshalAs(UnmanagedType.LPStr)]
            public string Title;

            /// <summary>
            /// Flags specifying the options for the dialog box.
            /// </summary>
            public uint Flags;

            /// <summary>
            /// Address of an application-defined function that the
            /// dialog box calls when an event occurs.
            /// </summary>
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc Callback;

            /// <summary>
            /// Application-defined value that the dialog box passes to 
            /// the callback function
            /// </summary>
            public int LParam;

            /// <summary>
            /// Variable to receive the image associated with the selected folder.
            /// </summary>
            public int Image;
        }

        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [ComImport, SuppressUnmanagedCodeSecurity, Guid("00000002-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc(int cb);
            [PreserveSig]
            IntPtr Realloc(IntPtr pv, int cb);
            [PreserveSig]
            void Free(IntPtr pv);
            [PreserveSig]
            int GetSize(IntPtr pv);
            [PreserveSig]
            int DidAlloc(IntPtr pv);
            [PreserveSig]
            void HeapMinimize();
        }

        [SecurityCritical]
        public static IMalloc GetSHMalloc()
        {
            IMalloc[] ppMalloc = new IMalloc[1];
            SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        [SecurityCritical, DllImport("shell32")]
        private static extern int SHGetMalloc([Out, MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);

        [DllImport("shell32.dll", EntryPoint = "FindExecutable")]
        private static extern long FindExecutableA(string lpFile, string lpDirectory, StringBuilder lpResult);

        public static string FindExecutable(string filename)
        {
            StringBuilder objResultBuffer = new StringBuilder(1024);

            long result = FindExecutableA(filename, string.Empty, objResultBuffer);

            if (result >= 32)
            {
                return objResultBuffer.ToString();
            }

            throw new InvalidOperationException(string.Format("Error: ({0})", result));
        }
    }
}

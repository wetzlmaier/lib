using System.Runtime.InteropServices;
using System.Text;

namespace Scch.Common.Windows
{
    public static class Kernel32
    {
        /// <summary>
        /// Functions in kernel32.dll
        /// </summary>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
        public static extern bool GetComputerName(StringBuilder lpBuffer, int[] nSize);
    }
}

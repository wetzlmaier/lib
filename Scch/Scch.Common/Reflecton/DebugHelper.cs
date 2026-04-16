using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Scch.Common.Reflecton
{
    public static class DebugHelper
    {
        /// <summary>
        /// Returns the full method name of the stack frame specified by the index.
        /// </summary>
        /// <param name="index">The index of the stack frame.</param>
        /// <returns>The full method name of the stack frame specified by the index.</returns>
        public static string GetFullMethodName(int index)
        {
            StackFrame frame = new StackTrace(true).GetFrame(index);
            MethodBase method = frame.GetMethod();
            return method.DeclaringType + "." + method.Name;
        }

        public static string FormatArgs(object[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var arg in args)
            {
                sb.Append(", " + arg == null ? "(null)" : arg.ToString());
            }

            sb.Remove(0, 2);
            return sb.ToString();
        }
    }
}

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Windows structures.
    /// </summary>
    public static class Structures
    {
        #region POINT
        /// <summary>
        /// The POINT structure defines the x- and y-coordinates of a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// Creates a new instance of <see cref="POINT"/>.
            /// </summary>
            /// <param name="x">x-coordinate.</param>
            /// <param name="y">y-coordinate</param>
            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            /// <summary>
            /// x-coordinate.
            /// </summary>
            public int X;

            /// <summary>
            /// y-coordinate
            /// </summary>
            public int Y;

            /// <summary>
            /// Converts a <see cref="POINT"/> to a <see cref="Point"/>.
            /// </summary>
            /// <param name="p">A <see cref="POINT"/>.</param>
            /// <returns>A <see cref="Point"/>.</returns>
            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            /// <summary>
            /// Converts a <see cref="Point"/> to a <see cref="POINT"/>.
            /// </summary>
            /// <param name="p">A <see cref="Point"/>.</param>
            /// <returns>A <see cref="POINT"/>.</returns>
            public static implicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }
        #endregion POINT

        #region RECT
        /// <summary>
        /// RECT structure
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// Left position.
            /// </summary>
            public int Left;
            /// <summary>
            /// Top position
            /// </summary>
            public int Top;
            /// <summary>
            /// Right position
            /// </summary>
            public int Right;
            /// <summary>
            /// Bottom position.
            /// </summary>
            public int Bottom;

            /// <summary>
            /// Creates a new instance of <see cref="RECT"/>.
            /// </summary>
            /// <param name="left"></param>
            /// <param name="top"></param>
            /// <param name="right"></param>
            /// <param name="bottom"></param>
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            /// <summary>
            /// Converts a <see cref="RECT"/> to a <see cref="Rectangle"/>.
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public static implicit operator Rectangle(RECT r)
            {
                return new Rectangle(r.Left, r.Top, r.Right-r.Left, r.Bottom-r.Top);
            }

            /// <summary>
            /// Converts a <see cref="Rectangle"/> to a <see cref="RECT"/>.
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public static implicit operator RECT(Rectangle r)
            {
                return new RECT(r.X, r.Y, r.Right, r.Bottom);
            }
        }
        #endregion RECT
    }
}

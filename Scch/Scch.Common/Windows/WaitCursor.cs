using System.Windows.Input;

namespace Scch.Common.Windows
{
    /// <summary>
    /// Provides a <see cref="WaitCursor"/> using the disposable pattern.
    /// </summary>
    public class WaitCursor : Disposable
    {
        private readonly Cursor _previousCursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitCursor"/> class.
        /// </summary>
        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        /// <summary>
        /// <see cref="Disposable.Dispose(bool)"/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
                Mouse.OverrideCursor = _previousCursor;

            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Scch.Common
{
    /// <summary>
    /// Base implementation for disposable classes.
    /// </summary>
    public class Disposable : IDisposable
    {
        #region Events
        /// <summary>
        /// Occurs when this instance has been disposed. 
        /// </summary>
        public event EventHandler Disposed;
        #endregion Events

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Disposable"/>.
        /// </summary>
        public Disposable()
        {
            Components = new Container();
            Disposables = new Collection<IDisposable>();
        }
        #endregion Constructors

        #region IDisposable Member
        /// <summary>
        /// Free module ressources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Releases the unmanaged resources used by the object and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // dispose managed ressources
                    if (Components != null)
                    {
                        Components.Dispose();
                    }

                    foreach (var disposable in Disposables)
                    {
                        disposable.Dispose();
                    }
                }

                // dispose unmanaged ressources
                IsDisposed = true;

                if (Disposed != null)
                {
                    Disposed(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="ObjectDisposedException"/> if the object is disposed.
        /// </summary>
        protected void CheckIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Use C# destructor syntax for finalization code.
        /// This destructor will run only if the Dispose method
        /// does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructors in types derived from this class.
        /// </summary>
        ~Disposable()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion Methods

        #region Properties

        /// <summary>
        /// Contains the disposable objects.
        /// </summary>
        protected IContainer Components { get; private set; }

        protected ICollection<IDisposable> Disposables { get; private set; }

        /// <summary>
        /// Returns true, if the object has beed disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed { get; private set; }
        #endregion Properties
    }
}


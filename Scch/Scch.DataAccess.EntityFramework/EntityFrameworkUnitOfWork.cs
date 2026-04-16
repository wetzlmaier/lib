using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Threading;
using Scch.Common;
using Scch.Logging;

namespace Scch.DataAccess.EntityFramework
{
    public class EntityFrameworkUnitOfWork<TKey> : Disposable, IEntityFrameworkUnitOfWork 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private DbTransaction _transaction;
        private readonly ExtendedDbContext<TKey> _context;

        public EntityFrameworkUnitOfWork(ExtendedDbContext<TKey> context)
        {
            _context = context;
        }

        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (IsInTransaction)
            {
                throw new InvalidOperationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                "Please commit or rollback the existing transaction before starting a new one.");
            }

            OpenConnection();
            _transaction = _context.ObjectContext.Connection.BeginTransaction(isolationLevel);
            Logger.Write(new DebugLogEntry("EntityFrameworkUnitOfWork.BeginTransaction for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        public void RollBackTransaction()
        {
            if (!IsInTransaction)
            {
                throw new InvalidOperationException("Cannot roll back a transaction while there is no transaction running.");
            }

            _transaction.Rollback();
            ReleaseCurrentTransaction();
            Logger.Write(new DebugLogEntry("EntityFrameworkUnitOfWork.RollBackTransaction for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        public void CommitTransaction()
        {
            if (!IsInTransaction)
            {
                throw new InvalidOperationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                EntityLogger.LogEntities(_context);
                _context.SaveChanges();
                _transaction.Commit();
                ReleaseCurrentTransaction();
                Logger.Write(new DebugLogEntry("EntityFrameworkUnitOfWork.CommitTransaction for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
            }
            catch(Exception ex)
            {
                Logger.Write(new ExceptionLogEntry("Rolling back after exception: '{0}'.", ex.Message));
                RollBackTransaction();
                throw;
            }
        }

        public void SaveChanges()
        {
            if (IsInTransaction)
            {
                throw new InvalidOperationException("A transaction is running. Call CommitTransaction instead.");
            }
            EntityLogger.LogEntities(_context);
            _context.SaveChanges();
            Logger.Write(new DebugLogEntry("EntityFrameworkUnitOfWork.SaveChanges for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        public void SaveChanges(SaveOptions saveOptions)
        {
            if (IsInTransaction)
            {
                throw new InvalidOperationException("A transaction is running. Call CommitTransaction instead.");
            }

            EntityLogger.LogEntities(_context);
            (_context).SaveChanges(saveOptions);
            Logger.Write(new DebugLogEntry("EntityFrameworkUnitOfWork.SaveChanges for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                if (_transaction != null)
                {
                    Logger.Write(new DebugLogEntry("EntityFrameworkUnitOfWork.Dispose with running transaction."));
                    _transaction.Dispose();
                }

                if (_context.ObjectContext.Connection.State != ConnectionState.Closed)
                {
                    _context.ObjectContext.Connection.Close();
                }
            }

            base.Dispose(disposing);
        }

        private void OpenConnection()
        {
            if (_context.ObjectContext.Connection.State != ConnectionState.Open)
            {
                _context.ObjectContext.Connection.Open();
            }
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}

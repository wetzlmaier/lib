using Scch.Common;
using Scch.Logging;
using System;
using System.Data;
using System.Threading;
using NHibernate;

namespace Scch.DataAccess.Hibernate
{
    public class HibernateUnitOfWork<TKey> : Disposable, IHibernateUnitOfWork
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private ITransaction _transaction;
        private readonly IStatelessSession _session;

        public HibernateUnitOfWork(IStatelessSession session)
        {
            _session = session;
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

            _transaction=_session.BeginTransaction(isolationLevel);
            Logger.Write(new DebugLogEntry("HibernateUnitOfWork.BeginTransaction for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        public void RollBackTransaction()
        {
            if (!IsInTransaction)
            {
                throw new InvalidOperationException("Cannot roll back a transaction while there is no transaction running.");
            }

            _transaction.Rollback();
            ReleaseCurrentTransaction();
            Logger.Write(new DebugLogEntry("HibernateUnitOfWork.RollBackTransaction for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        public void CommitTransaction()
        {
            if (!IsInTransaction)
            {
                throw new InvalidOperationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _transaction.Commit();
                ReleaseCurrentTransaction();
                Logger.Write(new DebugLogEntry("HibernateUnitOfWork.CommitTransaction for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry("Rolling back after exception: '{0}'.", ex.Message));
                RollBackTransaction();
                throw;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                if (_transaction != null)
                {
                    Logger.Write(new DebugLogEntry("HibernateUnitOfWork.Dispose with running transaction."));
                    _transaction.Dispose();
                }
            }

            base.Dispose(disposing);
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

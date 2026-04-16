using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using Scch.Common;
using Scch.DataAccess.EntityFramework;

namespace Scch.DataAccess.Tests
{
    public class MockUnitOfWork<TKey> : Disposable, IEntityFrameworkUnitOfWork
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly MockRepository<TKey> _repository;

        public MockUnitOfWork(MockRepository<TKey> repository)
        {
            _repository = repository;
        }

        private void CheckIfChangesApplied()
        {
            if (!_repository.ChangesApplied)
                throw new InvalidOperationException("Changes were not applied.");
        }

        public bool IsInTransaction { get; private set; }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            CheckIfDisposed();

            if (IsInTransaction)
            {
                throw new InvalidOperationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                "Please commit or rollback the existing transaction before starting a new one.");
            }

            IsInTransaction = true;
        }

        public void RollBackTransaction()
        {
            CheckIfDisposed();
            CheckIfChangesApplied();

            if (!IsInTransaction)
            {
                throw new InvalidOperationException("Cannot roll back a transaction while there is no transaction running.");
            }

            IsInTransaction = false;
        }

        public void CommitTransaction()
        {
            CheckIfDisposed();
            CheckIfChangesApplied();

            if (!IsInTransaction)
            {
                throw new InvalidOperationException("Cannot roll back a transaction while there is no transaction running.");
            }

            IsInTransaction = false;
            _repository.ChangesApplied = false;
        }

        public void SaveChanges()
        {
            SaveChanges(SaveOptions.None);
        }

        public void SaveChanges(SaveOptions saveOptions)
        {
            CheckIfDisposed();
            CheckIfChangesApplied();

            if (IsInTransaction)
            {
                throw new InvalidOperationException("A transaction is running. Call CommitTransaction instead.");
            }

            _repository.ChangesApplied = false;
        }
    }
}

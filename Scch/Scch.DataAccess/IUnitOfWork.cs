using System;

namespace Scch.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsInTransaction { get; }

        void BeginTransaction();

        void RollBackTransaction();

        void CommitTransaction();
    }
}

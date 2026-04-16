using Scch.Common;

namespace Scch.DataAccess.NoSql
{
    public abstract class NoSqlUnitOfWork : Disposable, INoSqlUnitOfWork
    {
        public abstract bool IsInTransaction { get; }

        public abstract void BeginTransaction();

        public abstract void RollBackTransaction();

        public abstract void CommitTransaction();
    }
}

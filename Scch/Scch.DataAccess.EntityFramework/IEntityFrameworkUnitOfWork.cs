using System.Data;
using System.Data.Entity.Core.Objects;

namespace Scch.DataAccess.EntityFramework
{
    public interface IEntityFrameworkUnitOfWork : IUnitOfWork
    {
        void BeginTransaction(IsolationLevel isolationLevel);

        void SaveChanges();

        void SaveChanges(SaveOptions saveOptions);
    }
}

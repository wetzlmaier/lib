using System.Data;

namespace Scch.DataAccess.Hibernate
{
    public interface IHibernateUnitOfWork : IUnitOfWork
    {
        void BeginTransaction(IsolationLevel isolationLevel);
    }
}

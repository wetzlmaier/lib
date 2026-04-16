using Scch.DomainModel.Hibernate;
using System;

namespace Scch.DataAccess.Hibernate
{
    public interface IHibernateRepositoryFactory<TKey> : IRepositoryFactory<IHibernateRepository<TKey>, TKey, IHibernateEntity<TKey>>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        new IHibernateRepository<TKey> Create();
    }
}

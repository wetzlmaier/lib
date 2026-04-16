using System;
using Scch.DomainModel;

namespace Scch.DataAccess
{
    public interface IRepositoryFactory<out TRepository, TKey, TEntityBase>
        where TRepository : IRepository<TKey, TEntityBase>
        where TKey : IComparable<TKey>, IEquatable<TKey>
        where TEntityBase : class, IEntity<TKey>
    {
        TRepository Create();

        void Drop();
    }
}

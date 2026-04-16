using System;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.EntityFramework
{
    public interface IEntityFrameworkRepositoryFactory<TKey> : IRepositoryFactory<IEntityFrameworkRepository<TKey>, TKey, IEntityFrameworkEntity<TKey>>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        new IEntityFrameworkRepository<TKey> Create();
    }
}

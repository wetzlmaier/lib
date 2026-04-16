using System;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql
{
    public interface INoSqlRepositoryFactory<TKey> : IRepositoryFactory<INoSqlRepository<TKey>, TKey, INoSqlEntity<TKey>>
        where TKey:IComparable<TKey>,IEquatable<TKey> 
    {
    }
}

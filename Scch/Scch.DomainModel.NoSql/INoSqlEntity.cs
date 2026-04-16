using System;

namespace Scch.DomainModel.NoSql
{
    public interface INoSqlEntity<TKey> : IEntity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
    }
}

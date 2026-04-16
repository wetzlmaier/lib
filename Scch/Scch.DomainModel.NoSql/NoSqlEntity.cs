using System;
using System.Xml.Serialization;

namespace Scch.DomainModel.NoSql
{
    [Serializable]
    [XmlRoot]
    public abstract class NoSqlEntity<TKey> : Entity<TKey>, INoSqlEntity<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
    }
}

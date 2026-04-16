using System;

namespace Scch.DomainModel.Hibernate
{
    public interface IHibernateEntity<TKey> : IEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Row version for optimistic locking.
        /// </summary>
        byte[] RowVersion { get; }

        /// <summary>
        /// Increments the <see cref="RowVersion"/> by one.
        /// </summary>
        void IncrementVersion();
    }
}

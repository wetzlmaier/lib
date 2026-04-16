using System;
using System.ComponentModel.DataAnnotations;

namespace Scch.DomainModel.EntityFramework
{
    public interface IEntityFrameworkEntity<TKey> : IObjectWithChangeTracker, IEntity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Row version for optimistic locking.
        /// </summary>
        [ConcurrencyCheck]
        long RowVersion { get; set; }

        /// <summary>
        /// Increments the <see cref="RowVersion"/> by one.
        /// </summary>
        void IncrementVersion();

        /// <summary>
        /// Clears the navigation properties.
        /// </summary>
        void ClearNavigationProperties();
    }
}

using System;
using Scch.DomainModel.Hibernate;

namespace Scch.DataAccess.Hibernate
{
    public interface IHibernateRepository<TKey> : IDisposable, IRepository<TKey, IHibernateEntity<TKey>>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        /// <summary>
        /// Updates changes of the existing entity. 
        /// The caller must later call SaveChanges() on the repository explicitly to save the entity to database
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void ApplyChanges<TEntity>(TEntity entity) where TEntity : class, IHibernateEntity<TKey>;

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        IHibernateUnitOfWork UnitOfWork { get; }
    }
}

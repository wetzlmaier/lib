using System;
using System.Data.Entity;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.EntityFramework
{
    public interface IEntityFrameworkRepository<TKey> : IDisposable, IRepository<TKey, IEntityFrameworkEntity<TKey>>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IObjectQuery<TEntity> GetQuery<TEntity>() where TEntity : IEntityFrameworkEntity<TKey>;
        /*
                /// <summary>
                /// Gets the query.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                /// <returns></returns>
                IObjectQuery<TEntity> GetQuery<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        */

        /// <summary>
        /// Gets entity by key.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="keyValue">The key value.</param>
        /// <returns></returns>
        TEntity GetByKey<TEntity>(object keyValue) where TEntity : class, IEntityFrameworkEntity<TKey>;

        /// <summary>
        /// Returns the <see cref="EntityState"/> of the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>The <see cref="EntityState"/> of the specified entity.</returns>
        EntityState State<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>;

        /// <summary>
        /// Updates the <see cref="EntityState"/> of the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="state">The <see cref="EntityState"/>.</param>
        void UpdateState<TEntity>(TEntity entity, EntityState state) where TEntity : class, IEntityFrameworkEntity<TKey>;

        /// <summary>
        /// Detects the changes.
        /// </summary>
        void DetectChanges();
        /*
                /// <summary>
                /// Attaches the specified entity.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="entity">The entity.</param>
                /// <param name="paths">The property paths.</param>
                //void Attach<TEntity>(TEntity entity) where TEntity : Entity;

                /// <summary>
                /// Attaches the specified entities.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entities.</typeparam>
                /// <param name="entities">The entities.</param>
                /// <param name="paths">The property paths.</param>
                //void AttachAll<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;*/

        /// <summary>
        /// Updates changes of the existing entity. 
        /// The caller must later call SaveChanges() on the repository explicitly to save the entity to database
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void ApplyChanges<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>;

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>The unit of work.</value>
        IEntityFrameworkUnitOfWork UnitOfWork { get; }
    }
}

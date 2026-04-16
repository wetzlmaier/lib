using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scch.DomainModel;

namespace Scch.DataAccess
{
    public interface IRepository<TKey, in TEntityBase>
        where TKey : IComparable<TKey>, IEquatable<TKey>
        where TEntityBase : class, IEntity<TKey>
    {
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TEntityBase;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, TEntityBase;

        /// <summary>
        /// Gets one entity based on matching criteria
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, TEntityBase;

        /// <summary>
        /// Gets one entity based on matching criteria
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, TEntityBase;
        /*
                /// <summary>
                /// Gets single entity using specification
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                /// <returns></returns>
                TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, TEntityBase;
        */
        /// <summary>
        /// Firsts the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, TEntityBase;
        /*
                /// <summary>
                /// Gets first entity with specification.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                /// <returns></returns>
                TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, TEntityBase;
        */

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class, TEntityBase;

        /// <summary>
        /// Deletes one or many entities matching the specified criteria
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, TEntityBase;
        /*
                /// <summary>
                /// Deletes entities which satify specificatiion
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        */

        /*
                /// <summary>
                /// Finds entities based on provided criteria.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                /// <returns></returns>
                IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;
        */
        /// <summary>
        /// Finds entities based on the provided criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, TEntityBase;
        /*
                /// <summary>
                /// Finds one entity based on provided criteria.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                /// <returns></returns>
                TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, TEntityBase;
        */
        /// <summary>
        /// Finds one entity based on provided criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, TEntityBase;

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, TEntityBase;

        /// <summary>
        /// Gets the specified order by.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, TEntityBase;

        /// <summary>
        /// Gets the specified criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, TEntityBase;
        /*
                /// <summary>
                /// Gets entities which satifies a specification.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
                /// <param name="specification">The specification.</param>
                /// <param name="orderBy">The order by.</param>
                /// <param name="pageIndex">Index of the page.</param>
                /// <param name="pageSize">Size of the page.</param>
                /// <param name="sortOrder">The sort order.</param>
                /// <returns></returns>
                IEnumerable<TEntity> Get<TEntity, TOrderBy>(ISpecification<TEntity> specification, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : Entity;
        */
        /// <summary>
        /// Counts the specified entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        long Count<TEntity>() where TEntity : class, TEntityBase;

        /// <summary>
        /// Counts entities with the specified criteria.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, TEntityBase;
        /*
                /// <summary>
                /// Counts entities satifying specification.
                /// </summary>
                /// <typeparam name="TEntity">The type of the entity.</typeparam>
                /// <param name="criteria">The criteria.</param>
                /// <returns></returns>
                int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, TEntityBase;
        */

        /// <summary>
        /// Filters out entities that implements the <see cref="IDeletable"/> interface.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities);
    }
}

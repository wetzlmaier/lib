using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scch.DataAccess.NoSql;
using Scch.DomainModel.NoSql;

namespace Scch.BusinessLogic.NoSql
{
    /// <summary>
    /// Business service interface for create, read, update and delete operations performed on the <see cref="INoSqlEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface INoSqlCrudService<TKey, TEntity>
        where TEntity : INoSqlEntity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new <see cref="INoSqlEntity{TKey}"/>
        /// </summary>
        /// <returns></returns>
        TEntity Create();

        /// <summary>
        /// Loads and returns the entity specified by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The entity specified by its id.</returns>
        TEntity LoadById(TKey id);

        TEntity LoadById(INoSqlRepository<TKey> repository, TKey id);

        /// <summary>
        /// Loads and returns the entities specified by their ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>The entities specified by their ids.</returns>
        IList<TEntity> LoadByIds(IEnumerable<TKey> ids);

        IEnumerable<TEntity> LoadByIds(INoSqlRepository<TKey> repository, ICollection<TKey> ids);

        /// <summary>
        /// Loads and returns all <see cref="INoSqlEntity{TKey}"/> of the specified type.
        /// </summary>
        /// <returns>All <see cref="INoSqlEntity{TKey}"/> of the specified type.</returns>
        IList<TEntity> LoadAll();

        IList<TEntity> LoadAll(INoSqlRepository<TKey> repository);

        /// <summary>
        /// Returns true, if an <see cref="INoSqlEntity{TKey}"/> with the specified id already exists.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>True, if an <see cref="INoSqlEntity{TKey}"/> with the specified id already exists.</returns>
        bool Exists(TKey id);

        bool Exists(INoSqlRepository<TKey> repository, TKey id);

        /// <summary>
        /// Saves the specified <see cref="INoSqlEntity{TKey}"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        TEntity Save(TEntity entity);

        TEntity Save(INoSqlRepository<TKey> repository, TEntity entity);

        /// <summary>
        /// Deletes the specified <see cref="INoSqlEntity{TKey}"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        void Delete(INoSqlRepository<TKey> repository, TEntity entity);

        /// <summary>
        /// Deletes all <see cref="INoSqlEntity{TKey}"/>.
        /// </summary>
        void DeleteAll();

        void DeleteAll(INoSqlRepository<TKey> repository);

        /// <summary>
        /// Finds entities based on the provided criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria);

        IEnumerable<TEntity> Find(INoSqlRepository<TKey> repository, Expression<Func<TEntity, bool>> criteria);
    }
}

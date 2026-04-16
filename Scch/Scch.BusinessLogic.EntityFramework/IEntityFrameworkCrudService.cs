using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scch.DataAccess.EntityFramework;
using Scch.DomainModel.EntityFramework;

namespace Scch.BusinessLogic.EntityFramework
{
    public interface IEntityFrameworkCrudService<TKey, TEntity>
        where TEntity : IEntityFrameworkEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new <see cref="IEntityFrameworkEntity{TKey}"/>
        /// </summary>
        /// <returns></returns>
        TEntity Create();

        /// <summary>
        /// Loads and returns the entity specified by its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="view">The <see cref="EntityView{TEntity}"/>.</param>
        /// <returns>The entity specified by its id.</returns>
        TEntity LoadById(TKey id, EntityView<TEntity> view = null);

        TEntity LoadById(IEntityFrameworkRepository<TKey> repository, TKey id, EntityView<TEntity> view = null);

        /// <summary>
        /// Loads and returns the entities specified by their ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="view">The <see cref="EntityView{TEntity}"/>.</param>
        /// <returns>The entities specified by their ids.</returns>
        IList<TEntity> LoadByIds(IEnumerable<TKey> ids, EntityView<TEntity> view = null);

        IEnumerable<TEntity> LoadByIds(IEntityFrameworkRepository<TKey> repository, IEnumerable<TKey> ids, EntityView<TEntity> view = null);

        /// <summary>
        /// Loads and returns all <see cref="IEntityFrameworkEntity{TKey}"/> of the specified type.
        /// </summary>
        /// <returns>All <see cref="IEntityFrameworkEntity{TKey}"/> of the specified type.</returns>
        IList<TEntity> LoadAll(EntityView<TEntity> view = null);

        IEnumerable<TEntity> LoadAll(IEntityFrameworkRepository<TKey> repository, EntityView<TEntity> view = null);

        /// <summary>
        /// Returns true, if an <see cref="IEntityFrameworkEntity{TKey}"/> with the specified id already exists.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>True, if an <see cref="IEntityFrameworkEntity{TKey}"/> with the specified id already exists.</returns>
        bool Exists(TKey id);

        bool Exists(IEntityFrameworkRepository<TKey> repository, TKey id);

        /// <summary>
        /// Saves the specified <see cref="IEntityFrameworkEntity{TKey}"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save(TEntity entity);

        void Save(IEntityFrameworkRepository<TKey> repository, TEntity entity);

        /// <summary>
        /// Saves a collection of entities in one transaction.
        /// </summary>
        /// <param name="entities"></param>
        void SaveAll(IEnumerable<TEntity> entities);

        void SaveAll(IEntityFrameworkRepository<TKey> repository, IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes the specified <see cref="IEntityFrameworkEntity{TKey}"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        void Delete(IEntityFrameworkRepository<TKey> repository, TEntity entity);

        /// <summary>
        /// Finds entities based on the provided criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="view">The <see cref="EntityView{TEntity}"/>.</param>
        /// <returns></returns>
        IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria, EntityView<TEntity> view = null);

        IEnumerable<TEntity> Find(IEntityFrameworkRepository<TKey> repository, Expression<Func<TEntity, bool>> criteria, EntityView<TEntity> view = null);
    }
}

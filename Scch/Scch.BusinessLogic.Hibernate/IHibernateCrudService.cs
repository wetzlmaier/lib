using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scch.DataAccess.Hibernate;
using Scch.DomainModel.Hibernate;

namespace Scch.BusinessLogic.Hibernate
{
    public interface IHibernateCrudService<TKey, TEntity>
        where TEntity : IHibernateEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new <see cref="IHibernateEntity{TKey}"/>
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

        TEntity LoadById(IHibernateRepository<TKey> repository, TKey id, EntityView<TEntity> view = null);

        /// <summary>
        /// Loads and returns the entities specified by their ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="view">The <see cref="EntityView{TEntity}"/>.</param>
        /// <returns>The entities specified by their ids.</returns>
        IList<TEntity> LoadByIds(IEnumerable<TKey> ids, EntityView<TEntity> view = null);

        IEnumerable<TEntity> LoadByIds(IHibernateRepository<TKey> repository, IEnumerable<TKey> ids, EntityView<TEntity> view = null);

        /// <summary>
        /// Loads and returns all <see cref="IHibernateEntity{TKey}"/> of the specified type.
        /// </summary>
        /// <returns>All <see cref="IHibernateEntity{TKey}"/> of the specified type.</returns>
        IList<TEntity> LoadAll(EntityView<TEntity> view = null);

        IEnumerable<TEntity> LoadAll(IHibernateRepository<TKey> repository, EntityView<TEntity> view = null);

        /// <summary>
        /// Returns true, if an <see cref="IHibernateEntity{TKey}"/> with the specified id already exists.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>True, if an <see cref="IHibernateEntity{TKey}"/> with the specified id already exists.</returns>
        bool Exists(TKey id);

        bool Exists(IHibernateRepository<TKey> repository, TKey id);

        /// <summary>
        /// Saves the specified <see cref="IHibernateEntity{TKey}"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save(TEntity entity);

        void Save(IHibernateRepository<TKey> repository, TEntity entity);

        /// <summary>
        /// Saves a collection of entities in one transaction.
        /// </summary>
        /// <param name="entities"></param>
        void SaveAll(IEnumerable<TEntity> entities);

        void SaveAll(IHibernateRepository<TKey> repository, IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes the specified <see cref="IHibernateEntity{TKey}"/>.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        void Delete(IHibernateRepository<TKey> repository, TEntity entity);

        /// <summary>
        /// Finds entities based on the provided criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="view">The <see cref="EntityView{TEntity}"/>.</param>
        /// <returns></returns>
        IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria, EntityView<TEntity> view = null);

        IEnumerable<TEntity> Find(IHibernateRepository<TKey> repository, Expression<Func<TEntity, bool>> criteria, EntityView<TEntity> view = null);
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql
{
    public interface INoSqlRepository<TKey> : IDisposable, IRepository<TKey, INoSqlEntity<TKey>>
        where TKey: IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        void DeleteAll<TEntity>() where TEntity : class, INoSqlEntity<TKey>;

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<TKey>;

        /// <summary>
        /// Inserts a collection of entities.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void InsertBatch<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, INoSqlEntity<TKey>;

        /// <summary>
        /// Saves an entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Save<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<TKey>;

        void UpdatePush<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<TKey>;

        void UpdatePull<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<TKey>;

        void AddToSet<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, TValue value) where TEntity : class, INoSqlEntity<TKey>;

        void DropDatabase();
    }
}

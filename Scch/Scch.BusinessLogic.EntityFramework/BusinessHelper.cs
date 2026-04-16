using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Scch.DataAccess.EntityFramework;
using Scch.DomainModel.EntityFramework;

namespace Scch.BusinessLogic.EntityFramework
{
    /// <summary>
    /// Helper class for business services.
    /// </summary>
    public static class BusinessHelper
    {
        /// <summary>
        /// Returns the specified entities as <see cref="IList{T}"/>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IList<TEntity> CreateResult<TKey, TEntity>(IEnumerable<TEntity> entities)
            where TEntity : IEntityFrameworkEntity<TKey>
            where TKey : IComparable<TKey>, IEquatable<TKey>

        {
            var result = new List<TEntity>(entities);
            result.ForEach(e => e.StartTracking());
            return result;
        }

        public static TEntity CreateResult<TKey, TEntity>(TEntity entity)
            where TEntity : IEntityFrameworkEntity<TKey>
            where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            if (entity != null)
                entity.StartTracking();
            return entity;
        }


        /// <summary>
        /// Applies an <see cref="EntityView{TEntity}"/> to the specified <see cref="IObjectQuery{T}"/> and returns the resulting <see cref="IObjectQuery{T}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="query">The specified <see cref="IObjectQuery{T}"/>.</param>
        /// <param name="view">The <see cref="EntityView{TEntity}"/>.</param>
        /// <returns>The resulting <see cref="IObjectQuery{T}"/>.</returns>
        public static IObjectQuery<TEntity> ApplyView<TEntity>(IObjectQuery<TEntity> query, EntityView<TEntity> view)
        {
            query.MergeOption = MergeOption.NoTracking;

            if (view == null)
                return query;

            return view.Aggregate(query, (current, path) => current.Include(path));
        }
    }
}

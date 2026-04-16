using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Scch.Common.Linq.Expressions;

namespace Scch.BusinessLogic.EntityFramework
{
    /// <summary>
    /// Holds the property paths, that should be returned as object graph.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class EntityView<TEntity> : IEnumerable<string>
    {
        private readonly ISet<string> _members;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView{TEntity}"/> class.
        /// </summary>
        public EntityView()
        {
            _members = new HashSet<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView{TEntity}"/> class.
        /// </summary>
        /// <param name="members">The property paths.</param>
        public EntityView(params Expression<Func<TEntity, object>>[] members)
            : this()
        {
            foreach (var path in members)
                Include(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityView{TEntity}"/> class.
        /// </summary>
        /// <param name="paths">The property paths.</param>
        public EntityView(params string[] paths)
            : this()
        {
            foreach (var path in paths)
                Include(path);
        }

        /// <summary>
        /// Includes the specified property path to the view.
        /// </summary>
        /// <param name="path">The property path.</param>
        /// <returns>The view.</returns>
        public EntityView<TEntity> Include(Expression<Func<TEntity, object>> path)
        {
            return Include(ExpressionHelper.GetPropertyPath(path));
        }

        /// <summary>
        /// Includes the specified property path to the view.
        /// </summary>
        /// <param name="path">The property path.</param>
        /// <returns>The view.</returns>
        public EntityView<TEntity> Include(string path)
        {
            if (_members.Contains(path))
                throw new ArgumentOutOfRangeException("path");

            _members.Add(path);
            return this;
        }

        /// <summary>
        /// <see cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return _members.GetEnumerator();
        }

        /// <summary>
        /// <see cref="IEnumerable.GetEnumerator"/>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// <see cref="object.ToString"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var path in this)
            {
                sb.Append("; " + path);
            }

            if (sb.Length > 2)
                sb.Remove(0, 2);

            return sb.ToString();
        }
    }
}

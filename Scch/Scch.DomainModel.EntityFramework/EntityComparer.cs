using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Scch.Common.Linq.Expressions;

namespace Scch.DomainModel.EntityFramework
{
    /// <summary>
    /// <see cref="IComparer{T}"/> implementation for <see cref="Entity{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityComparer<T> : IComparer<T>, IEqualityComparer<T> 
    {
        private readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityComparer{T}"/> class.
        /// </summary>
        public EntityComparer()
            : this("Id")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityComparer{T}"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public EntityComparer(Expression<Func<T, object>> property)
            : this(ExpressionHelper.GetPropertyPath(property))
        {
        }

        private EntityComparer(string propertyName)
        {
            _property = typeof(T).GetProperty(propertyName);
            if (!typeof(IComparable).IsAssignableFrom(_property.PropertyType))
                throw new ArgumentException("propertyName");
        }

        /// <summary>
        /// <see cref="IComparer{T}.Compare"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            if (object.Equals(x, default(T)))
                return -1;

            if (object.Equals(y, default(T)))
                return 1;

            var xvalue = _property.GetValue(x, null) as IComparable;
            var yvalue = _property.GetValue(y, null) as IComparable;

            if (xvalue == null || yvalue == null)
                throw new ArgumentException(string.Format("Type of property '{0}' does not implement IComparable", _property.Name));

            return (xvalue.CompareTo(yvalue));
        }

        /// <summary>
        /// <see cref="IEqualityComparer{T}.Equals(T,T)"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return Compare(x, y) == 0;
        }

        /// <summary>
        /// <see cref="IEqualityComparer{T}.GetHashCode(T)"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            var value = (IComparable)_property.GetValue(obj, null);
            return value == null ? 0 : value.GetHashCode();
        }
    }
}

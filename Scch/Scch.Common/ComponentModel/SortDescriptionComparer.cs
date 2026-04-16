using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Scch.Common.ComponentModel
{
    /// <summary>
    /// <see cref="IComparer"/> implementation that uses <see cref="SortDescription"/> for the comparison.
    /// </summary>
    public class SortDescriptionComparer : IComparer
    {
        private readonly PropertyInfo[] _properties;
        private readonly ListSortDirection[] _directions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescriptionComparer"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sortDescriptions"></param>
        public SortDescriptionComparer(Type type, IEnumerable<SortDescription> sortDescriptions)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var properties = new List<PropertyInfo>();
            var directions = new List<ListSortDirection>();

            foreach (var sortDescription in sortDescriptions)
            {
                properties.Add(type.GetProperty(sortDescription.PropertyName));
                directions.Add(sortDescription.Direction);
            }

            _properties = properties.ToArray();
            _directions = directions.ToArray();
        }

        /// <summary>
        /// <see cref="IComparer.Compare"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (Equals(x, null))
                return -1;

            if (Equals(y, null))
                return 1;

            if (x.GetType() != y.GetType())
                throw new ArgumentException("x.GetType()!=y.GetType()");

            for (int i = 0; i < _properties.Length; i++)
            {
                var property = _properties[i];
                var direction = _directions[i];

                var xvalue = property.GetValue(x, null) as IComparable;
                var yvalue = property.GetValue(y, null) as IComparable;

                if (xvalue == null || yvalue == null)
                    throw new ArgumentException(string.Format("Type of property '{0}' does not implement IComparable", property.Name));

                var result = (xvalue.CompareTo(yvalue) * (direction == ListSortDirection.Ascending ? 1 : -1));
                if (result != 0)
                    return result;
            }

            return 0;
        }
    }

    /// <summary>
    /// <see cref="IComparer{T}"/> implementation that uses <see cref="SortDescription"/> for the comparison.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortDescriptionComparer<T> : IComparer<T>
    {
        private readonly SortDescriptionComparer _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescriptionComparer{T}"/> class.
        /// </summary>
        /// <param name="sortDescriptions"></param>
        public SortDescriptionComparer(IEnumerable<SortDescription> sortDescriptions)
        {
            _comparer = new SortDescriptionComparer(typeof(T), sortDescriptions);
        }

        /// <summary>
        /// <see cref="IComparer{T}.Compare"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            return _comparer.Compare(x, y);
        }
    }
}

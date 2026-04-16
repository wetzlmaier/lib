using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using Scch.Common;

namespace Scch.DomainModel.EntityFramework
{
    /// <summary>
    /// An <see cref="ObservableCollection{T}"/> that raises individual item removal notifications on clear and prevents adding duplicates.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    [CollectionDataContract]
    public class TrackableCollection<TKey, TEntity> : ObservableCollection<TEntity> 
        where TEntity : IEntityFrameworkEntity<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private static readonly EntityComparer<TEntity> Comparer = new EntityComparer<TEntity>();

        static TrackableCollection()
        {
            Comparer = new EntityComparer<TEntity>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableCollection{TEntity,TKey}"/> class.
        /// </summary>
        public TrackableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackableCollection{TEntity,TKey}"/> class.
        /// </summary>
        /// <param name="collection"></param>
        public TrackableCollection(IEnumerable<TEntity> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// <see cref="ObservableCollection{T}.ClearItems"/>
        /// </summary>
        protected override void ClearItems()
        {
            new List<TEntity>(this).ForEach(t => Remove(t));
        }

        /// <summary>
        /// <see cref="ObservableCollection{T}.InsertItem"/>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, TEntity item)
        {
            if (!Contains(item))
            {
                base.InsertItem(index, item);
            }
        }

        /// <summary>
        /// <see cref="Collection{T}.Contains"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Contains(TEntity item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// <see cref="Collection{T}.IndexOf"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new int IndexOf(TEntity item)
        {
            for (int i = 0; i < Count; i++)
            {
                if ((Comparer.Equals(this[i], item) && !item.IsTransient) || (Equals(this[i], item) && item.IsTransient))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Sorts the <see cref="TrackableCollection{TEntity,TKey}"/>.
        /// </summary>
        public void Sort()
        {
            Sort((IComparer<TEntity>)null);
        }

        /// <summary>
        /// Sorts the <see cref="TrackableCollection{TEntity,TKey}"/>.
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer<TEntity> comparer)
        {
            if (typeof(TEntity).GetInterface("IComparable") == null && typeof(TEntity).GetInterface("IComparable`1") == null)
                throw new NotSupportedException("At least one object must implement IComparable");

            if (Count < 2)
                return;

            List<TEntity> sorted = this.OrderBy(x => x, comparer).ToList();

            Clear();
            foreach (var item in sorted)
                Add(item);
        }

        /// <summary>
        /// Sorts the <see cref="TrackableCollection{TEntity,TKey}"/>.
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<TEntity> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");

            Sort(new FunctorComparer<TEntity>(comparison));
        }
    }
}

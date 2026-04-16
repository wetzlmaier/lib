using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Scch.Common.Collections.ObjectModel
{
    public class ThreadSafeObservableCollection<T> : ObservableCollection<T>
    {
        private const int Version = 1;
        private readonly object _syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeObservableCollection{T}"/> class.
        /// </summary>
        public ThreadSafeObservableCollection()
        {
            _syncRoot = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="collection"></param>
        public ThreadSafeObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
            _syncRoot = new object();
        }

        /// <summary> 
        /// Clears all items 
        /// </summary> 
        protected override void ClearItems()
        {
            lock (_syncRoot)
            {
                BaseClearItems();
            }
        }

        private void BaseClearItems()
        {
            base.ClearItems();
        }

        /// <summary> 
        /// Inserts a item at the specified index 
        /// </summary> 
        ///<param name="index">The index where the item should be inserted</param> 
        ///<param name="item">The item which should be inserted</param> 
        protected override void InsertItem(int index, T item)
        {
            lock (_syncRoot)
            {
                BaseInsertItem(index, item);
            }
        }

        private void BaseInsertItem(int index, T item)
        {
            base.InsertItem(index, item);
        }

        /// <summary> 
        /// Moves an item from one index to another 
        /// </summary> 
        ///<param name="oldIndex">The index of the item which should be moved</param> 
        ///<param name="newIndex">The index where the item should be moved</param> 
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            lock (_syncRoot)
            {
                BaseMoveItem(oldIndex, newIndex);
            }
        }

        private void BaseMoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        /// <summary> 
        /// Removes the item at the specified index 
        /// </summary> 
        ///<param name="index">The index of the item which should be removed</param> 
        protected override void RemoveItem(int index)
        {
            lock (_syncRoot)
            {
                BaseRemoveItem(index);
            }
        }

        private void BaseRemoveItem(int index)
        {
            base.RemoveItem(index);
        }

        /// <summary> 
        /// Sets the item at the specified index 
        /// </summary> 
        ///<param name="index">The index which should be set</param> 
        ///<param name="item">The new item</param> 
        protected override void SetItem(int index, T item)
        {
            lock (_syncRoot)
            {
                BaseSetItem(index, item);
            }
        }

        private void BaseSetItem(int index, T item)
        {
            base.SetItem(index, item);
        }

        /// <summary> 
        /// Fires the CollectionChanged Event 
        /// </summary> 
        ///<param name="e">The additional arguments of the event</param> 
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            lock (_syncRoot)
            {
                BaseOnCollectionChanged(e);
            }
        }

        private void BaseOnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        /// <summary> 
        /// Fires the PropertyChanged Event 
        /// </summary> 
        ///<param name="e">The additional arguments of the event</param> 
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            lock (_syncRoot)
            {
                BaseOnPropertyChanged(e);
            }
        }

        private void BaseOnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }
    }
}

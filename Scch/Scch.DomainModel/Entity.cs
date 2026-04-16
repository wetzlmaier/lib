using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Scch.Common.ComponentModel;

namespace Scch.DomainModel
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    [Serializable]
    public abstract class Entity<TKey> : DataErrorInfo, IEntity<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private TKey _id;
        /*[NonSerialized]
        private bool _isDeserializing;

        [NonSerialized]
        private bool _isSerializing;*/

        /// <summary>
        /// <see cref="IEntity{TKey}.Id"/>
        /// </summary>
        [Browsable(false)]
        public virtual TKey Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (!Equals(_id, value))
                {
                    _id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event on navigation properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        protected virtual void RaiseNavigationPropertyChanged<T>(Expression<Func<T>> property)
        {
            base.RaisePropertyChanged(property);
        }
/*
        /// <summary>
        /// Returns true, if the deserialization is in progress.
        /// </summary>
        protected bool IsDeserializing
        {
            get { return _isDeserializing; }
            private set { _isDeserializing = value; }
        }

        /// <summary>
        /// <see cref="IEntity{TKey}.OnDeserializingMethod"/>
        /// </summary>
        /// <param name="context"></param>
        [OnDeserializing]
        public void OnDeserializingMethod(StreamingContext context)
        {
            IsDeserializing = true;
        }

        /// <summary>
        /// <see cref="IEntity{TKey}.OnDeserializedMethod"/>
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            IsDeserializing = false;
        }

        /// <summary>
        /// Returns true, if the serialization is in progress.
        /// </summary>
        protected bool IsSerializing
        {
            get { return _isSerializing; }
            private set { _isSerializing = value; }
        }

        /// <summary>
        /// <see cref="IEntity{TKey}.OnSerializingMethod"/>
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        public void OnSerializingMethod(StreamingContext context)
        {
            IsSerializing = true;
        }

        /// <summary>
        /// <see cref="IEntity{TKey}.OnSerializedMethod"/>
        /// </summary>
        /// <param name="context"></param>
        [OnSerialized]
        public void OnSerializedMethod(StreamingContext context)
        {
            IsSerializing = false;
        }*/

        /// <summary>
        /// <see cref="IEntity{TKey}.IsTransient"/>
        /// </summary>
        [Browsable(false)]
        public abstract bool IsTransient { get; }

        /// <summary>
        /// <see cref="IComparable{T}.CompareTo"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(IEntity<TKey> other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}

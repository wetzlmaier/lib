using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Scch.Common.Linq.Expressions;

namespace Scch.DomainModel.EntityFramework
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    public abstract class EntityFrameworkEntity<TKey> : Entity<TKey>, IEntityFrameworkEntity<TKey> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private ObjectChangeTracker _changeTracker;
        private static readonly Dictionary<Type, List<PropertyInfo>> NavigationReferencePropertyChache;
        private static readonly Dictionary<Type, List<PropertyInfo>> NavigationCollectionPropertyChache;
        private static readonly MethodInfo ClearMethod;

        static EntityFrameworkEntity()
        {
            ClearMethod = typeof (IList).GetMethod("Clear");
            NavigationReferencePropertyChache = new Dictionary<Type, List<PropertyInfo>>();
            NavigationCollectionPropertyChache = new Dictionary<Type, List<PropertyInfo>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkEntity{TKey}"/> class.
        /// </summary>
        protected EntityFrameworkEntity()
        {
            List<PropertyInfo> navigationReferencePropertyChache;
            if (!NavigationReferencePropertyChache.TryGetValue(GetType(), out navigationReferencePropertyChache))
            {
                navigationReferencePropertyChache = new List<PropertyInfo>();
                lock (GetType())
                {
                    NavigationReferencePropertyChache.Add(GetType(), navigationReferencePropertyChache);
                }
            }

            List<PropertyInfo> navigationCollectionPropertyChache;
            if (!NavigationCollectionPropertyChache.TryGetValue(GetType(), out navigationCollectionPropertyChache))
            {
                navigationCollectionPropertyChache = new List<PropertyInfo>();
                lock (GetType())
                {
                    NavigationCollectionPropertyChache.Add(GetType(), navigationCollectionPropertyChache);
                }
            }

            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                if (typeof(IEntityFrameworkEntity<TKey>).IsAssignableFrom(propertyInfo.PropertyType))
                    lock (this)
                    {
                        navigationReferencePropertyChache.Add(propertyInfo);
                    }

                if (typeof(TrackableCollection<TKey, IEntityFrameworkEntity<TKey>>).IsAssignableFrom(propertyInfo.PropertyType))
                    lock (this)
                    {
                        navigationCollectionPropertyChache.Add(propertyInfo);
                    }
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if <see cref="ObjectChangeTracker.ChangeTrackingEnabled"/> is enabled.
        /// </summary>
        protected void ThrowExceptionIfChangeTrackingEnabled()
        {
            if (ChangeTracker.ChangeTrackingEnabled)
            {
                throw new InvalidOperationException("Cannot set the FixupChangeTrackingCollection when ChangeTracking is enabled");
            }
        }
        
        /// <summary>
        /// <see cref="IEntity{TKey}.Id"/>
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override TKey Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                if (!Equals(base.Id, value))
                {
                    if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added /*&& !IsDeserializing*/)
                    {
                        throw new InvalidOperationException("The property 'Id' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
                    }
                }

                base.Id = value;
            }
        }

        /// <summary>
        /// <see cref="IEntityFrameworkEntity{TKey}.RowVersion"/>
        /// </summary>
        [ConcurrencyCheck]
        public long RowVersion { get; set; }

        /// <summary>
        /// <see cref="IEntityFrameworkEntity{TKey}.IncrementVersion"/>
        /// </summary>
        public void IncrementVersion()
        {
            RowVersion++;
        }

        /// <summary>
        /// <see cref="IEntity{TKey}.IsTransient"/>
        /// </summary>
        [NotMapped]
        public override bool IsTransient
        {
            get { return Equals(Id, default(TKey)); }
        }

        #region ChangeTracking
        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        protected new void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
            {
                ChangeTracker.State = ObjectState.Modified;
            }

            base.RaisePropertyChanged(property);
        }

        /// <summary>
        /// <see cref="IObjectWithChangeTracker.ChangeTracker"/>
        /// </summary>
        [NotMapped]
        [DataMember]
        public ObjectChangeTracker ChangeTracker
        {
            get
            {
                if (_changeTracker == null)
                {
                    _changeTracker = new ObjectChangeTracker();
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
                return _changeTracker;
            }
            set
            {
                if (_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
                }
                _changeTracker = value;
                if (_changeTracker != null)
                {
                    _changeTracker.ObjectStateChanging += HandleObjectStateChanging;
                }
            }
        }

        private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
        {
            if (e.NewState == ObjectState.Deleted)
            {
                ClearNavigationProperties();
            }
        }

        /// <summary>
        /// <see cref="IEntityFrameworkEntity{TKey}.ClearNavigationProperties"/>
        /// </summary>
        public void ClearNavigationProperties()
        {
            foreach (PropertyInfo propertyInfo in NavigationReferencePropertyChache[GetType()])
                propertyInfo.SetValue(this, null, null);

            foreach (PropertyInfo propertyInfo in NavigationCollectionPropertyChache[GetType()])
            {
                object collection=propertyInfo.GetValue(this, null);
                ClearMethod.Invoke(collection, null);
            }
        }

        /// <summary>
        /// Fixes the references in a many to many relationship.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TNavigationEntity">The type of the navigation property.</typeparam>
        /// <param name="e"></param>
        /// <param name="targetPath"></param>
        /// <param name="sourcePath"></param>
        protected void FixupManyToManyProperty<TEntity, TNavigationEntity>(NotifyCollectionChangedEventArgs e, Expression<Func<TEntity, TrackableCollection<TKey, TNavigationEntity>>> targetPath, Expression<Func<TNavigationEntity, TrackableCollection<TKey, TEntity>>> sourcePath)
            where TNavigationEntity : IEntityFrameworkEntity<TKey>
            where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            if (targetPath == null)
                throw new ArgumentNullException("targetPath");

            PropertyInfo sourceProperty = null;
            if (sourcePath != null)
                sourceProperty = typeof(TNavigationEntity).GetProperty(ExpressionHelper.GetPropertyPath(sourcePath));

            var targetName = ExpressionHelper.GetPropertyPath(targetPath);
            var entity = this as TEntity;

            if (e.NewItems != null)
            {
                foreach (TNavigationEntity item in e.NewItems)
                {
                    if (sourceProperty != null)
                    {
                        var sourceValue = (TrackableCollection<TKey, TEntity>)sourceProperty.GetValue(item, null);

                        if (entity != null && !sourceValue.Contains(entity))
                        {
                            sourceValue.Add(entity);
                        }
                    }

                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties(targetName, item);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (TNavigationEntity item in e.OldItems)
                {
                    if (sourceProperty != null)
                    {
                        var sourceValue = (TrackableCollection<TKey, TEntity>) sourceProperty.GetValue(item, null);

                        if (entity!=null && sourceValue.Contains(entity))
                        {
                            sourceValue.Remove(entity);
                        }
                    }

                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties(targetName, item);
                    }
                }
            }
        }

        /// <summary>
        /// Fixes the references in an one to many relationship.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TNavigationEntity">The type of the navigation property.</typeparam>
        /// <param name="e"></param>
        /// <param name="targetPath"></param>
        /// <param name="sourcePath"></param>
        protected void FixupOneToManyProperty<TEntity, TNavigationEntity>(NotifyCollectionChangedEventArgs e, Expression<Func<TEntity, TrackableCollection<TKey, TNavigationEntity>>> targetPath, Expression<Func<TNavigationEntity, TEntity>> sourcePath)
            where TNavigationEntity : IEntityFrameworkEntity<TKey>
            where TEntity : class, IEntityFrameworkEntity<TKey>  
        {
            if (targetPath==null)
                throw new ArgumentNullException("targetPath");

            PropertyInfo sourceProperty=null;
            if (sourcePath!=null)
                sourceProperty = typeof(TNavigationEntity).GetProperty(ExpressionHelper.GetPropertyPath(sourcePath));

            var targetName = ExpressionHelper.GetPropertyPath(targetPath);

            if (e.NewItems != null)
            {
                foreach (TNavigationEntity item in e.NewItems)
                {
                    if (sourceProperty!=null)
                        sourceProperty.SetValue(item, this, null);

                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        if (!item.ChangeTracker.ChangeTrackingEnabled)
                        {
                            item.StartTracking();
                        }
                        ChangeTracker.RecordAdditionToCollectionProperties(targetName, item);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (TNavigationEntity item in e.OldItems)
                {
                    if (sourceProperty != null)
                    {
                        var sourceValue = sourceProperty.GetValue(item, null);

                        if (ReferenceEquals(sourceValue, this))
                        {
                            sourceProperty.SetValue(item, null, null);
                        }
                    }

                    if (ChangeTracker.ChangeTrackingEnabled)
                    {
                        ChangeTracker.RecordRemovalFromCollectionProperties(targetName, item);
                    }
                }
            }
        }

        /// <summary>
        /// Fixes the references in a many to one relationship.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TNavigationEntity">The type of the navigation property.</typeparam>
        /// <param name="previousValue"></param>
        /// <param name="targetPath"></param>
        /// <param name="sourcePath"></param>
        protected void FixupManyToOneProperty<TEntity, TNavigationEntity>(TNavigationEntity previousValue, Expression<Func<TEntity, TNavigationEntity>> targetPath, Expression<Func<TNavigationEntity, TrackableCollection<TKey, TEntity>>> sourcePath)
            where TNavigationEntity : IEntityFrameworkEntity<TKey>
            where TEntity : class, IEntityFrameworkEntity<TKey>  
        {
            if (targetPath==null)
                throw new ArgumentNullException("targetPath");

            PropertyInfo sourceProperty=null;
            if (sourcePath!=null)
                sourceProperty = typeof(TNavigationEntity).GetProperty(ExpressionHelper.GetPropertyPath(sourcePath));

            var entity = this as TEntity;

            if (!Equals(previousValue, default(TNavigationEntity)) && sourceProperty != null)
            {
                var previousSourceValue = (TrackableCollection<TKey, TEntity>)sourceProperty.GetValue(previousValue, null);
                if (entity != null && previousSourceValue.Contains(entity))
                {
                    Debug.Assert(previousSourceValue.Remove(entity));
                }
            }

            string targetName = ExpressionHelper.GetPropertyPath(targetPath);
            var targetProperty = typeof(TEntity).GetProperty(targetName);
            var targetValue = (TNavigationEntity)targetProperty.GetValue(this, null);
            var idProperty = typeof(TEntity).GetProperty(targetName + ExpressionHelper.GetPropertyPath<EntityFrameworkEntity<TKey>>(x => x.Id));

            if (!Equals(targetValue, default(TNavigationEntity)))
            {
                if (sourceProperty != null)
                {
                    var sourceValue = (TrackableCollection<TKey, TEntity>) sourceProperty.GetValue(targetValue, null);

                    if (entity != null && !sourceValue.Contains(entity))
                    {
                        sourceValue.Add(entity);
                    }
                }

                idProperty.SetValue(entity, targetValue.Id, null);
            }
            else 
            {
                if (idProperty.PropertyType == typeof(long?))
                    idProperty.SetValue(entity, null, null);
                else
                    idProperty.SetValue(entity, 0, null);
            } 

            if (ChangeTracker.ChangeTrackingEnabled)
            {
                if (ChangeTracker.OriginalValues.ContainsKey(targetName)
                    && (ReferenceEquals(ChangeTracker.OriginalValues[targetName], targetValue)))
                {
                    ChangeTracker.OriginalValues.Remove(targetName);
                }
                else
                {
                    ChangeTracker.RecordOriginalValue(targetName, previousValue);
                }
                if (!Equals(targetValue, default(TNavigationEntity)) && !targetValue.ChangeTracker.ChangeTrackingEnabled)
                {
                    targetValue.StartTracking();
                }
            }
        }
        #endregion
    }
}

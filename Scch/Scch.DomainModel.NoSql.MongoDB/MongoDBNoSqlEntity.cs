using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scch.Common.Linq.Expressions;

namespace Scch.DomainModel.NoSql.MongoDB
{
    [Serializable]
    [XmlRoot]
    public abstract class MongoDBNoSqlEntity : NoSqlEntity<ObjectId>, IMongoDBNoSqlEntity
    {
        [BsonIgnore]
        [XmlIgnore]
        public override bool IsTransient
        {
            get { return Equals(Id, default(ObjectId)); }
        }

        [BsonIgnore]
        [XmlIgnore]
        public override bool IsValid
        {
            get { return base.IsValid; }
            protected set { base.IsValid = value; }
        }

        protected void FixupOneToManyProperty<TEntity, TNavigationEntity>(NotifyCollectionChangedEventArgs e, Expression<Func<TNavigationEntity, TEntity>> sourcePath)
            where TNavigationEntity : IMongoDBNoSqlEntity
            where TEntity : class, IMongoDBNoSqlEntity
        {
            PropertyInfo sourceProperty = null;
            if (sourcePath != null)
                sourceProperty = typeof(TNavigationEntity).GetProperty(ExpressionHelper.GetPropertyPath(sourcePath));

            if (e.NewItems != null)
            {
                foreach (TNavigationEntity item in e.NewItems)
                {
                    if (sourceProperty != null)
                        sourceProperty.SetValue(item, this, null);
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
                }
            }
        }

        protected void FixupManyToOneProperty<TEntity, TNavigationEntity>(TNavigationEntity previousValue, Expression<Func<TEntity, TNavigationEntity>> targetPath, Expression<Func<TNavigationEntity, ObservableCollection<TEntity>>> sourcePath)
            where TNavigationEntity : IMongoDBNoSqlEntity
            where TEntity : class, IMongoDBNoSqlEntity
        {
            if (targetPath == null)
                throw new ArgumentNullException("targetPath");

            PropertyInfo sourceProperty = null;
            if (sourcePath != null)
                sourceProperty = typeof(TNavigationEntity).GetProperty(ExpressionHelper.GetPropertyPath(sourcePath));

            var entity = this as TEntity;

            if (!Equals(previousValue, default(TNavigationEntity)) && sourceProperty != null)
            {
                var previousSourceValue = (ObservableCollection<TEntity>)sourceProperty.GetValue(previousValue, null);
                if (entity != null && previousSourceValue.Contains(entity))
                {
                    Debug.Assert(previousSourceValue.Remove(entity));
                }
            }

            string targetName = ExpressionHelper.GetPropertyPath(targetPath);
            var targetProperty = typeof(TEntity).GetProperty(targetName);
            var targetValue = (TNavigationEntity)targetProperty.GetValue(this, null);

            if (!Equals(targetValue, default(TNavigationEntity)))
            {
                if (sourceProperty != null)
                {
                    var sourceValue = (ObservableCollection<TEntity>)sourceProperty.GetValue(targetValue, null);

                    if (entity != null && !sourceValue.Contains(entity))
                    {
                        sourceValue.Add(entity);
                    }
                }
            }
        }

        protected void FixupManyToManyProperty<TEntity, TNavigationEntity>(NotifyCollectionChangedEventArgs e, Expression<Func<TNavigationEntity, ObservableCollection<TEntity>>> sourcePath)
            where TNavigationEntity : IMongoDBNoSqlEntity
            where TEntity : class, IMongoDBNoSqlEntity
        {
            PropertyInfo sourceProperty = null;
            if (sourcePath != null)
                sourceProperty = typeof(TNavigationEntity).GetProperty(ExpressionHelper.GetPropertyPath(sourcePath));

            var entity = this as TEntity;

            if (e.NewItems != null)
            {
                foreach (TNavigationEntity item in e.NewItems)
                {
                    if (sourceProperty != null)
                    {
                        var sourceValue = (ObservableCollection<TEntity>)sourceProperty.GetValue(item, null);

                        if (entity != null && !sourceValue.Contains(entity))
                        {
                            sourceValue.Add(entity);
                        }
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (TNavigationEntity item in e.OldItems)
                {
                    if (sourceProperty != null)
                    {
                        var sourceValue = (ObservableCollection<TEntity>)sourceProperty.GetValue(item, null);

                        if (entity != null && sourceValue.Contains(entity))
                        {
                            sourceValue.Remove(entity);
                        }
                    }
                }
            }
        }
    }
}

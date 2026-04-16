using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using NHibernate;
using Scch.Common;
using Scch.DomainModel;
using Scch.DomainModel.Hibernate;
using Scch.Logging;

namespace Scch.DataAccess.Hibernate
{
    public class HibernateRepository<TKey> : Disposable, IHibernateRepository<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private bool _disposed;
        private IHibernateUnitOfWork _unitOfWork;
        private readonly IStatelessSession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="IHibernateRepositoryFactory{TKey}"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public HibernateRepository(IStatelessSession session)
        {
            _session = session;
            Logger.Write(new DebugLogEntry("HibernateRepository.ctor for thread '{0}'.",
                Thread.CurrentThread.ManagedThreadId));
        }
/*
        public TEntity GetByKey<TEntity>(object keyValue) where TEntity : class, IHibernateEntity<TKey>
        {
            EntityKey key = GetEntityKey<TEntity>(keyValue);

            object originalItem;
            if (_context.ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                Logger.Write(new DebugLogEntry(
                    "HibernateRepository.GetByKey returns entity of type '{0}' with id '{1}'.", typeof(TEntity).Name,
                    ((TEntity)originalItem).Id));
                return (TEntity)originalItem;
            }

            return default(TEntity);
        }*/

        private IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IHibernateEntity<TKey>
        {
            return _session.Query<TEntity>();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().AsQueryable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IHibernateEntity<TKey>
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    .AsEnumerable();
            }

            return GetQuery<TEntity>().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria,
            Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IHibernateEntity<TKey>
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQueryable(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    .AsEnumerable();
            }

            return GetQueryable(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .AsEnumerable();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().Single(criteria);
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().SingleOrDefault(criteria);
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().First(predicate);
        }
        
        public void Delete<TEntity>(TEntity entity) where TEntity : class, IHibernateEntity<TKey>
        {
            if (Equals(entity, null))
            {
                throw new ArgumentNullException("entity");
            }

            Logger.Write(new DebugLogEntry("HibernateRepository.Delete deletes entity of type '{0}' with id '{1}'.",
                typeof(TEntity).Name, entity.Id));

            if (entity is IDeletable)
            {
                ((IDeletable)entity).IsDeleted = true;
            }
            else
            {
                //entity.MarkAsDeleted();
            }
        }
        
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IHibernateEntity<TKey>
        {
            IEnumerable<TEntity> records = Find(criteria);

            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().AsEnumerable();
        }
        
        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().Where(criteria);
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().Where(criteria).FirstOrDefault();
        }

        public long Count<TEntity>() where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().Count();
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IHibernateEntity<TKey>
        {
            return GetQuery<TEntity>().Count(criteria);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_unitOfWork != null)
                    _unitOfWork.Dispose();

                Logger.Write(new DebugLogEntry("HibernateRepository.Dispose for thread '{0}'.",
                    Thread.CurrentThread.ManagedThreadId));
                _disposed = true;
            }
        }

        public void ApplyChanges<TEntity>(TEntity entity) where TEntity : class, IHibernateEntity<TKey>
        {
            if (entity.IsTransient)
                _session.Insert(entity);
            else
                _session.Update(entity);
        }

        public IHibernateUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new HibernateUnitOfWork<TKey>(_session)); }
        }

        public IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities)
        {
            if (typeof(IDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                var deletableEntities = (IQueryable<IDeletable>)entities;
                return deletableEntities.Where(entity => !entity.IsDeleted).Cast<TEntity>();
            }

            return entities;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Scch.Common;
using Scch.DomainModel;
using Scch.DomainModel.EntityFramework;
using Scch.Logging;
using EntityKey = System.Data.Entity.Core.EntityKey;
using EntityKeyMember = System.Data.Entity.Core.EntityKeyMember;
using EntityState = System.Data.Entity.EntityState;

namespace Scch.DataAccess.EntityFramework
{
    /// <summary>
    /// Generic repository
    /// </summary>
    public class EntityFrameworkRepository<TKey> : Disposable, IEntityFrameworkRepository<TKey> 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly PluralizationService _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
        private bool _disposed;
        readonly ExtendedDbContext<TKey> _context;
        private IEntityFrameworkUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="IEntityFrameworkRepositoryFactory{TKey}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EntityFrameworkRepository(ExtendedDbContext<TKey> context)
        {
            _context = context;
            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.ctor for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
        }

        public TEntity GetByKey<TEntity>(object keyValue) where TEntity : class, IEntityFrameworkEntity<TKey> 
        {
            EntityKey key = GetEntityKey<TEntity>(keyValue);

            object originalItem;
            if (_context.ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                Logger.Write(new DebugLogEntry("EntityFrameworkRepository.GetByKey returns entity of type '{0}' with id '{1}'.", typeof(TEntity).Name, ((TEntity)originalItem).Id));
                return (TEntity)originalItem;
            }

            return default(TEntity);
        }

        public IObjectQuery<TEntity> GetQuery<TEntity>() where TEntity : IEntityFrameworkEntity<TKey>
        {
            /* 
             * From CTP4, I could always safely call this to return an IQueryable on DbContext 
             * then performed any with it without any problem:
             */
            // return DbContext.Set<TEntity>();

            /*
             * but with 4.1 release, when I call GetQuery<TEntity>().AsEnumerable(), there is an exception:
             * ... System.ObjectDisposedException : The ObjectContext instance has been disposed and can no longer be used for operations that require a connection.
             */

            // here is a work around: 
            // - cast DbContext to IObjectContextAdapter then get ObjectContext from it
            // - call CreateQuery<TEntity>(entityName) method on the ObjectContext
            // - perform querying on the returning IQueryable, and it works!

            var entityName = GetEntityName<TEntity>();
            ObjectQuery<TEntity> query = _context.ObjectContext.CreateQuery<TEntity>(entityName);
            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.GetQuery returns query of type '{0}' with name '{1}'.", typeof(TEntity).Name, query.Name));
            return new ObjectQueryWrapper<TEntity>(query);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().AsQueryable();
        }

        /*
                public IQueryable<TEntity> GetQuery<TEntity>(ISpecification<TEntity> criteria) where TEntity : IEntityFrameworkEntity
                {
                    return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>());
                }
        */
        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<TEntity>().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQuery<TEntity>().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQueryable(criteria).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            }
            return GetQueryable(criteria).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
        }
        /*
                public IEnumerable<TEntity> Get<TEntity, TOrderBy>(ISpecification<TEntity> specification, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IEntity
                {
                    if (sortOrder == SortOrder.Ascending)
                    {
                        return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                    }
                    return specification.SatisfyingEntitiesFrom(GetQuery<TEntity>()).OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                }
        */
        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().Single(criteria);
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().SingleOrDefault(criteria);
        }

        /*
                public TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
                {
                    return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
                }
        */
        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().First(predicate);
        }
        /*
                public TEntity First<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, IEntity
                {
                    return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).First();
                }
        */
        public EntityState State<TEntity>(TEntity entity) where TEntity : class,  IEntityFrameworkEntity<TKey>
        {
            var state = _context.Entry(entity);
            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.EntityState returns entity of type '{0}' with state '{1}'.", typeof(TEntity).Name, state.State));
            return state.State;
        }

        public void UpdateState<TEntity>(TEntity entity, EntityState state) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            var oldState = _context.Entry(entity);
            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.UpdateEntityState updates entity of type '{0}' with state '{1}' (old state '{2}'.", typeof(TEntity).Name, state, oldState.State));
            oldState.State = state;
        }

        public void DetectChanges()
        {
            _context.ChangeTracker.DetectChanges();
        }

        public void UpdateChangeTrackingState<TEntity>(TEntity entity) where TEntity : class,  IEntityFrameworkEntity<TKey>
        {
            switch (entity.ChangeTracker.State)
            {
                case ObjectState.Unchanged:
                    UpdateState(entity, EntityState.Unchanged);
                    break;
                case ObjectState.Added:
                    UpdateState(entity, EntityState.Added);
                    break;
                case ObjectState.Modified:
                    UpdateState(entity, EntityState.Modified);
                    break;
                case ObjectState.Deleted:
                    UpdateState(entity, EntityState.Deleted);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /*
        public void Attach<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] paths) where TEntity : IEntityFrameworkEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _context.Set<TEntity>().Attach(entity);

            
            UpdateChangeTrackingState(entity);

            foreach (var path in paths)
            {
                var members = new List<PropertyInfo>();
                EntityFrameworkHelper.CollectRelationalMembers(path, members);

                Entity currentEntity = entity;
                foreach (var member in members)
                {
                    var value = (Entity)member.GetValue(currentEntity, null);

                    if (value != null)
                    {
                        value= (Entity)_context.Set(value.GetType()).Attach(entity);
                        UpdateChangeTrackingState(value);
                    }

                    //member.SetValue(currentEntity, value, null);
                    currentEntity = value;
                }
            }
        }
        
        public void AttachAll<TEntity>(IEnumerable<TEntity> entities, params Expression<Func<TEntity, object>>[] paths) where TEntity : IEntityFrameworkEntity
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            foreach (TEntity entity in entities)
                Attach(entity, paths);
        }*/

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            if (Equals(entity, null))
            {
                throw new ArgumentNullException("entity");
            }

            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.Delete deletes entity of type '{0}' with id '{1}'.", typeof(TEntity).Name, entity.Id));

            if (entity is IDeletable)
            {
                ((IDeletable)entity).IsDeleted = true;
            }
            else
            {
                entity.MarkAsDeleted();
            }
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            IEnumerable<TEntity> records = Find(criteria);

            foreach (TEntity record in records)
            {
                Delete(record);
            }
        }
        /*
                public void Delete<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, IEntity
                {
                    IEnumerable<TEntity> records = Find<TEntity>(criteria);
                    foreach (TEntity record in records)
                    {
                        Delete<TEntity>(record);
                    }
                }
        */

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().AsEnumerable();
        }

        public void ApplyChanges<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.ApplyChanges applies changes of type '{0}' with id '{1}'.", typeof(TEntity).Name, entity.Id));

            string name = GetEntityName<TEntity>();
            _context.ObjectContext.ApplyChanges(name, entity);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().Where(criteria);
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().Where(criteria).FirstOrDefault();
        }
        /*
                public TEntity FindOne<TEntity>(ISpecification<TEntity> criteria) where TEntity : IEntityFrameworkEntity
                {
                    return criteria.SatisfyingEntityFrom(GetQuery<TEntity>());
                }

                public IEnumerable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : IEntityFrameworkEntity
                {
                    return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).AsEnumerable();
                }
        */
        public long Count<TEntity>() where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().Count();
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            return GetQuery<TEntity>().Count(criteria);
        }
        /*
                public int Count<TEntity>(ISpecification<TEntity> criteria) where TEntity : IEntityFrameworkEntity
                {
                    return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>()).Count();
                }
        */
        private EntityKey GetEntityKey<TEntity>(object keyValue) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            var entitySetName = GetEntityName<TEntity>();
            var objectSet = _context.ObjectContext.CreateObjectSet<TEntity>();
            var keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var entityKey = new EntityKey(entitySetName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });
            Logger.Write(new DebugLogEntry("EntityFrameworkRepository.GetEntityKey returns key of type '{0}' with value '{1}'.", typeof(TEntity).Name, keyValue));
            return entityKey;
        }

        private string GetEntityName<TEntity>() where TEntity : IEntityFrameworkEntity<TKey>
        {
            return string.Format("{0}.{1}", _context.ObjectContext.DefaultContainerName, _pluralizer.Pluralize(typeof(TEntity).Name));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_unitOfWork != null)
                    _unitOfWork.Dispose();

                if (_context.ObjectContext.Connection.State != ConnectionState.Closed)
                {
                    _context.ObjectContext.Connection.Close();
                }

                _context.Dispose();

                Logger.Write(new DebugLogEntry("EntityFrameworkRepository.Dispose for thread '{0}'.", Thread.CurrentThread.ManagedThreadId));
                _disposed = true;
            }
        }

        public IEntityFrameworkUnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new EntityFrameworkUnitOfWork<TKey>(_context)); }
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

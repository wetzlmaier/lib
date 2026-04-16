using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Scch.Common;
using Scch.DataAccess.EntityFramework;
using Scch.DomainModel;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.Tests
{
    public class MockRepository<TKey> : InMemoryDatabase<TKey, IEntityFrameworkEntity<TKey>>, IEntityFrameworkRepository<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public MockRepository()
        {
            UnitOfWork = new MockUnitOfWork<TKey>(this);
        }

        IEntityFrameworkUnitOfWork IEntityFrameworkRepository<TKey>.UnitOfWork
        {
            get { return UnitOfWork; }
        }

        public MockUnitOfWork<TKey> UnitOfWork { get; }

        public IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities)
        {
            if (typeof(IDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                var deletableEntities = (IQueryable<IDeletable>)entities;
                return deletableEntities.Where(entity => !entity.IsDeleted).Cast<TEntity>();
            }

            return entities;
        }

        public void UpdateState(EntityState state)
        {
            throw new NotImplementedException();
        }

        public TEntity GetByKey<TEntity>(object keyValue) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            return (TEntity)ObjectCloner.DeepClone((from e in GetList<TEntity>() where Equals(e.Id, keyValue) select e).Single());
        }

        public IObjectQuery<TEntity> GetQuery<TEntity>() where TEntity : IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();

            var list = GetList<TEntity>();
            return new MockObjectQuery<TEntity>(list.AsQueryable());
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            return GetQueryable<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            return GetList<TEntity>().AsQueryable();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            throw new NotImplementedException();
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public EntityState State<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            throw new NotImplementedException();
        }

        public void UpdateState<TEntity>(TEntity entity, EntityState state) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            throw new NotImplementedException();
        }

        public void DetectChanges()
        {
            throw new NotImplementedException();
        }
        /*
        public void Attach<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] paths) where TEntity : Entity
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public void AttachAll<TEntity>(IEnumerable<TEntity> entities, params Expression<Func<TEntity, object>>[] paths) where TEntity : Entity
        {
            CheckIfDisposed();

            foreach (var entity in entities)
                Attach(entity, paths);
        }*/

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();

            GetList<TEntity>().Remove(entity);
            entity.ChangeTracker.State = ObjectState.Deleted;
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public void ApplyChanges<TEntity>(TEntity entity) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();

            switch (entity.ChangeTracker.State)
            {
                case ObjectState.Unchanged:
                    break;
                case ObjectState.Added:
                    throw new NotImplementedException();
                    //entity.Id = NextId<TEntity>();
                    //GetInternalList<TEntity>().Add(entity);
                    //break;
                case ObjectState.Modified:
                    break;
                case ObjectState.Deleted:
                    Remove(entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ChangesApplied = true;
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        public long Count<TEntity>() where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            return GetList<TEntity>().Count;
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, IEntityFrameworkEntity<TKey>
        {
            CheckIfDisposed();
            throw new NotImplementedException();
        }

        internal bool ChangesApplied { get; set; }
    }
}

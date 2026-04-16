using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scch.DataAccess.Hibernate;
using Scch.DomainModel.Hibernate;
using Scch.Logging;

namespace Scch.BusinessLogic.Hibernate
{
    public abstract class HibernateCrudService<TKey, TEntity> : IHibernateCrudService<TKey, TEntity>
        where TEntity : class, IHibernateEntity<TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected IHibernateRepositoryFactory<TKey> Factory { get; private set; }

        protected HibernateCrudService(IHibernateRepositoryFactory<TKey> factory)
        {
            Factory = factory;
        }

        public TEntity Create()
        {
            var entity = new TEntity();
            entity.StartTracking();
            return entity;
        }

        public TEntity LoadById(TKey id, EntityView<TEntity> view = null)
        {
            if (Equals(id, default(TKey)))
                throw new ArgumentNullException("id");

            try
            {
                Logger.Write(new DebugLogEntry("CrudService.LoadById loads entity with id '{0}' and view '{1}'.", id, view));

                using (var repository = Factory.Create())
                {
                    var result = BusinessHelper.CreateResult<TKey, TEntity>(LoadById(repository, id, view));
                    Logger.Write(new DebugLogEntry("CrudService.LoadById returns entity '{0}'.", result.ToString()));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw HibernateExceptionHelper.HandleException(ex);
            }
        }

        public TEntity LoadById(IHibernateRepository<TKey> repository, TKey id, EntityView<TEntity> view = null)
        {
            IObjectQuery<TEntity> query = repository.GetQuery<TEntity>();
            query = BusinessHelper.ApplyView(query, view);
            return (from c in query where Equals(c.Id, id) select c).SingleOrDefault();
        }

        /// <summary>
        /// <see cref="http://stackoverflow.com/questions/18976495/linq-to-entities-only-supports-casting-edm-primitive-or-enumeration-types-with-i"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private static bool Equals<T>(T v1, T v2)
        {
            return object.Equals(v1, v2);
        }

        public IList<TEntity> LoadByIds(IEnumerable<TKey> ids, EntityView<TEntity> view = null)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            var idCollection=new List<TKey>(ids);
            if (!idCollection.Any())
                return new List<TEntity>();

            try
            {
                Logger.Write(new DebugLogEntry("CrudService.LoadByIds loads entities with view '{1}'.", view));

                using (var repository = Factory.Create())
                {
                    var result = BusinessHelper.CreateResult<TKey, TEntity>(LoadByIds(repository, idCollection, view));
                    Logger.Write(new DebugLogEntry("CrudService.LoadByIds returns '{0}' entities.", result.Count));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw HibernateExceptionHelper.HandleException(ex);
            }
        }

        public IEnumerable<TEntity> LoadByIds(IHibernateRepository<TKey> repository, IEnumerable<TKey> ids, EntityView<TEntity> view = null)
        {
            IObjectQuery<TEntity> query = repository.GetQuery<TEntity>();
            query = BusinessHelper.ApplyView(query, view);
            return from c in query where ids.Contains(c.Id) select c;
        }

        /// <summary>
        /// <see cref="IHibernateCrudService{TKey, TEntity}.Exists(TKey)"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(TKey id)
        {
            try
            {
                using (var repository = Factory.Create())
                {
                    var start = DateTime.Now;
                    var result = Exists(repository, id);
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }

        public bool Exists(IHibernateRepository<TKey> repository, TKey id)
        {
            Logger.Write(new DebugLogEntry("CrudService.Exists checks existence of entity with id '{0}'.", id));
            var result = repository.Count<TEntity>(e => Equals(e.Id, id)) > 0;
            Logger.Write(new DebugLogEntry("CrudService.Exists returns '{0}'.", result));
            return result;
        }

        public IList<TEntity> LoadAll(EntityView<TEntity> view = null)
        {
            try
            {
                Logger.Write(new DebugLogEntry("CrudService.LoadAll loads all entities of type '{0}'.", typeof(TEntity).Name));
                using (var repository = Factory.Create())
                {
                    var result=BusinessHelper.CreateResult<TKey, TEntity>(LoadAll(repository, view));
                    Logger.Write(new DebugLogEntry("CrudService.LoadAll returns '{0}' entities.", result.Count));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw HibernateExceptionHelper.HandleException(ex);
            }
        }

        public IEnumerable<TEntity> LoadAll(IHibernateRepository<TKey> repository, EntityView<TEntity> view = null)
        {
            IObjectQuery<TEntity> query = repository.GetQuery<TEntity>();
            query = BusinessHelper.ApplyView(query, view);
            return (from c in query select c);
        }

        public void Save(TEntity entity)
        {
            if (Equals(entity, default(TEntity)))
                throw new ArgumentNullException("entity");
            try
            {
                using (var repository = Factory.Create())
                {
                    var start = DateTime.Now;
                    Save(repository, entity);
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                throw HibernateExceptionHelper.HandleException(ex);
            }
        }

        public void Save(IHibernateRepository<TKey> repository, TEntity entity)
        {
            Logger.Write(new DebugLogEntry("CrudService.Save saves entity with id '{0}'.", entity.Id));
            repository.UnitOfWork.BeginTransaction();
            repository.ApplyChanges(entity);
            repository.UnitOfWork.CommitTransaction();
            entity.AcceptChanges();
            Logger.Write(new DebugLogEntry("CrudService.Save saved entity with id '{0}'.", entity.Id));
        }

        public void SaveAll(IEnumerable<TEntity> entities)
        {
            if (entities.Any(entity => Equals(entity, default(TEntity))))
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                using (var repository = Factory.Create())
                {
                    var start = DateTime.Now;
                    SaveAll(repository, entities);
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                throw HibernateExceptionHelper.HandleException(ex);
            }
        }

        public void SaveAll(IHibernateRepository<TKey> repository, IEnumerable<TEntity> entities)
        {
            repository.UnitOfWork.BeginTransaction();

            foreach ( var entity in entities)
                repository.ApplyChanges(entity);

            repository.UnitOfWork.CommitTransaction();

            foreach (var entity in entities)
                entity.AcceptChanges();

            Logger.Write(new DebugLogEntry("CrudService.Save saved '{0}' entities.", entities.Count()));
        }

        public void Delete(IHibernateRepository<TKey> repository, TEntity entity)
        {
            if (Equals(entity, default(TEntity)))
                throw new ArgumentNullException("entity");

            repository.UnitOfWork.BeginTransaction();
            repository.Delete(entity);
            repository.ApplyChanges(entity);
            repository.UnitOfWork.CommitTransaction();
        }

        public void Delete(TEntity entity)
        {
            if (Equals(entity, default(TEntity)))
                throw new ArgumentNullException("entity");

            try
            {
                Logger.Write(new DebugLogEntry("CrudService.Delete deletes entity with id '{0}'.", entity.Id));
                using (var repository = Factory.Create())
                {
                    Delete(repository, entity);
                    Logger.Write(new DebugLogEntry("CrudService.Delete deleted entity with id '{0}'.", entity.Id));
                }
            }
            catch (Exception ex)
            {
                throw HibernateExceptionHelper.HandleException(ex);
            }
        }

        public IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria, EntityView<TEntity> view = null)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            try
            {
                Logger.Write(new DebugLogEntry("CrudService.Find loads entitiey with criteria '{0}' and view '{1}'.", criteria, view));

                using (var repository = Factory.Create())
                {
                    var start = DateTime.Now;
                    var result = BusinessHelper.CreateResult<TKey, TEntity>(Find(repository, criteria, view));
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                    Logger.Write(new DebugLogEntry("CrudService.Find returns '{0}' entitiey.", result.Count));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }

        public IEnumerable<TEntity> Find(IHibernateRepository<TKey> repository, Expression<Func<TEntity, bool>> criteria, EntityView<TEntity> view = null)
        {
            IObjectQuery<TEntity> query = repository.GetQuery<TEntity>();
            query = BusinessHelper.ApplyView(query, view);
            return query.Where(criteria);
        }
    }
}

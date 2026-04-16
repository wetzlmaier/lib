using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scch.DataAccess.NoSql;
using Scch.DomainModel.NoSql;
using Scch.Logging;

namespace Scch.BusinessLogic.NoSql
{
    /// <summary>
    /// Business service for create, read, update and delete operations performed on the <see cref="INoSqlEntity{TKey}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class NoSqlCrudService<TKey, TEntity> : INoSqlCrudService<TKey, TEntity>
        where TEntity : class, INoSqlEntity<TKey>, new() 
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly INoSqlRepositoryFactory<TKey> _factory;

        protected NoSqlCrudService(INoSqlRepositoryFactory<TKey> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.Create"/>
        /// </summary>
        /// <returns></returns>
        public TEntity Create()
        {
            var entity = new TEntity();
            return entity;
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.LoadById(TKey)"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity LoadById(TKey id)
        {
            if (Equals(id, default(TKey)))
                throw new ArgumentNullException("id");

            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    var result = LoadById(repository, id);
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

        public TEntity LoadById(INoSqlRepository<TKey> repository, TKey id)
        {
            Logger.Write(new DebugLogEntry("CrudService.LoadById loads entity with id '{0}'.", id));
            var result = repository.FindOne<TEntity>(e => Equals(e.Id, id));
            Logger.Write(new DebugLogEntry("CrudService.LoadById returns entity '{0}'.", result));
            return result;
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.LoadByIds(IEnumerable{TKey})"/>
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IList<TEntity> LoadByIds(IEnumerable<TKey> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids");

            var list = ids.ToList();
            if (!list.Any())
                return new List<TEntity>();

            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    var result = new List<TEntity>(LoadByIds(repository, list));
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

        public IEnumerable<TEntity> LoadByIds(INoSqlRepository<TKey> repository, ICollection<TKey> ids)
        {
            Logger.Write(new DebugLogEntry("CrudService.LoadByIds loads entity with ids '{0}'.", ids.ToArray()));
            return repository.Find<TEntity>(e => ids.Contains(e.Id));
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.Exists(TKey)"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(TKey id)
        {
            try
            {
                using (var repository = _factory.Create())
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

        public bool Exists(INoSqlRepository<TKey> repository, TKey id)
        {
            Logger.Write(new DebugLogEntry("CrudService.Exists checks existence of entity with id '{0}'.", id));
            var result = repository.Count<TEntity>(e => Equals(e.Id, id)) > 0;
            Logger.Write(new DebugLogEntry("CrudService.Exists returns '{0}'.", result));
            return result;
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.LoadAll()"/>
        /// </summary>
        /// <returns></returns>
        public IList<TEntity> LoadAll()
        {
            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    var result = LoadAll(repository);
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

        public IList<TEntity> LoadAll(INoSqlRepository<TKey> repository)
        {
            Logger.Write(new DebugLogEntry("CrudService.LoadAll loads all entities of type '{0}'.", typeof(TEntity).Name));
            var result = new List<TEntity>(repository.GetAll<TEntity>());
            Logger.Write(new DebugLogEntry("CrudService.LoadAll returns '{0}' entities.", result.Count));
            return result;
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.Save(TEntity)"/>
        /// </summary>
        /// <param name="entity"></param>
        public TEntity Save(TEntity entity)
        {
            if (Equals(entity, default(TEntity)))
                throw new ArgumentNullException("entity");

            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    var result = Save(repository, entity);
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

        public TEntity Save(INoSqlRepository<TKey> repository, TEntity entity)
        {
            Logger.Write(new DebugLogEntry("CrudService.Save saves entity with id '{0}'.", entity.Id));
            var result = repository.Save(entity);
            Logger.Write(new DebugLogEntry("CrudService.Save saved entity with id '{0}'.", entity.Id));

            return result;
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.Delete(TEntity)"/>
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            if (Equals(entity, default(TEntity)))
                throw new ArgumentNullException("entity");

            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    Delete(repository, entity);
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }

        public void Delete(INoSqlRepository<TKey> repository, TEntity entity)
        {
            Logger.Write(new DebugLogEntry("CrudService.Delete deletes entity with id '{0}'.", entity.Id));
            repository.Delete(entity);
            Logger.Write(new DebugLogEntry("CrudService.Delete deleted entity with id '{0}'.", entity.Id));
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.DeleteAll()"/>
        /// </summary>
        public void DeleteAll()
        {
            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    DeleteAll(repository);
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }

        public void DeleteAll(INoSqlRepository<TKey> repository)
        {
            Logger.Write(new DebugLogEntry("CrudService.DeleteAll."));
            repository.DeleteAll<TEntity>();
            Logger.Write(new DebugLogEntry("CrudService.DeleteAll finished."));
        }

        /// <summary>
        /// <see cref="INoSqlCrudService{TKey, TEntity}.Find(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        /// <param name="criteria"></param>
        public IList<TEntity> Find(Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    var result = Find(repository, criteria);
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }

        public IEnumerable<TEntity> Find(INoSqlRepository<TKey> repository, Expression<Func<TEntity, bool>> criteria)
        {
            return repository.Find(criteria);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Elastic.Clients.Elasticsearch;
using Scch.Common;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.ElasticSearch
{
    public class ElasticSearchNoSqlRepository : Disposable, IElasticSearchNoSqlRepository
    {
        private readonly ElasticsearchClient _client;

        public ElasticSearchNoSqlRepository(IElasticSearchContext context)
        {
            _client = context.GetClient();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public long Count<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void InsertBatch<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            _client.Index(entity);
            return entity;
        }

        public void UpdatePush<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void UpdatePull<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void AddToSet<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, TValue value) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void DropDatabase()
        {
            throw new NotImplementedException();
        }
    }
}

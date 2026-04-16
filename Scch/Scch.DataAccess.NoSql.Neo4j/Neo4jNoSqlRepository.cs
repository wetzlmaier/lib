using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Neo4jClient;
using Scch.Common;
using Scch.DomainModel.NoSql;
using Scch.Neo4j;

namespace Scch.DataAccess.NoSql.Neo4j
{
    public class Neo4jNoSqlRepository : Disposable, INeo4jNoSqlRepository
    {
        private IGraphClient _client;

        public Neo4jNoSqlRepository(INeo4jContext context)
        {
            _client = context.GetClient();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public long Count<TEntity>() where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll<TEntity>() where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void InsertBatch<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void UpdatePush<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void UpdatePull<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void AddToSet<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, TValue value) where TEntity : class, INoSqlEntity<Neo4jObjectId>
        {
            throw new NotImplementedException();
        }

        public void DropDatabase()
        {
            _client.Cypher.Match("(n)").OptionalMatch("(n)-[r]-()").Delete("r,n").ExecuteWithoutResultsAsync().GetAwaiter().GetResult();
        }

        public void DropGraph<TEntity>()
        {
            _client.Cypher.Match(string.Format("(n:{0})", GetEntityName<TEntity>())).OptionalMatch("(n)-[r]-()").Delete("r,n").ExecuteWithoutResultsAsync().GetAwaiter().GetResult();
        }

        public void CreateUniqueConstraint(string label, string property)
        {
            string typeParam = "c:" + label;
            string typePropertyParam = "c." + property;
            _client.Cypher.CreateUniqueConstraint(typeParam, typePropertyParam).ExecuteWithoutResultsAsync().GetAwaiter().GetResult();
        }

        public void DropUniqueConstraint(string label, string property)
        {
            string typeParam = "c:" + label;
            string typePropertyParam = "c." + property;
            _client.Cypher.DropUniqueConstraint(typeParam, typePropertyParam).ExecuteWithoutResultsAsync().GetAwaiter().GetResult();
        }

        private string GetEntityName<TEntity>()
        {
            return typeof (TEntity).Name;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                var disposableClient = _client as IDisposable;
                if (disposableClient != null)
                {
                    disposableClient.Dispose();
                    _client = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}

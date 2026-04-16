using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Scch.Common;
using Scch.Common.Linq.Expressions;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.MongoDB
{
    public class MongoDBNoSqlRepository : Disposable, IMongoDBNoSqlRepository
    {
        private readonly IMongoDatabase _database;

        public MongoDBNoSqlRepository(IMongoDBContext context)
        {
            _database = context.GetDatabase();
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {

            }

            base.Dispose(disposing);
        }

        private IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetQueryable<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetCollection<TEntity>().AsQueryable();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetQueryable(criteria).Single();
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetQueryable(criteria).SingleOrDefault();
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetQueryable(predicate).First();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<ObjectId>
        {
            Delete<TEntity>(e => e.Id == entity.Id);
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<ObjectId>
        {
            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.Where(criteria);
            collection.DeleteMany(filter);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetQueryable(criteria);
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return Find(criteria).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetCollection<TEntity>().AsQueryable().AsEnumerable();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<ObjectId>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<ObjectId>
        {
            throw new NotImplementedException();
        }

        public long Count<TEntity>() where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetCollection<TEntity>().CountDocuments(_ => true);
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<ObjectId>
        {
            return GetCollection<TEntity>().CountDocuments(Builders<TEntity>.Filter.Where(criteria));
        }

        public IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll<TEntity>() where TEntity : class, INoSqlEntity<ObjectId>
        {
            GetCollection<TEntity>().DeleteMany(_ => true);

            var directory = Path.Combine("Xml", typeof(TEntity).Name);
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<ObjectId>
        {
            GetCollection<TEntity>().InsertOne(entity);
            return entity;
        }

        public void InsertBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, INoSqlEntity<ObjectId>
        {
            GetCollection<TEntity>().InsertMany(entities);
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<ObjectId>
        {
            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            collection.ReplaceOne(filter, entity, new ReplaceOptions { IsUpsert = true });
            return entity;
        }

        private void AddToCollection<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, IEnumerable<TValue> values)
            where TEntity : class, INoSqlEntity<ObjectId>
        {
            var propertyValue = ExpressionHelper.GetPropertyValue(expression, entity) as ICollection<TValue>;

            if (propertyValue != null)
                foreach (var value in values)
                    propertyValue.Add(value);
        }

        private void RemoveFromCollection<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, IEnumerable<TValue> values)
            where TEntity : class, INoSqlEntity<ObjectId>
        {
            var propertyValue = ExpressionHelper.GetPropertyValue(expression, entity) as ICollection<TValue>;

            if (propertyValue != null)
                foreach (var value in values)
                    propertyValue.Remove(value);
        }

        public void UpdatePush<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<ObjectId>
        {
            if (entity.IsTransient)
                throw new NoSqlException("Entity is transient.");

            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            var update = Builders<TEntity>.Update.PushEach(expression, values);
            collection.UpdateOne(filter, update);
        }

        public void UpdatePull<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<ObjectId>
        {
            if (entity.IsTransient)
                throw new NoSqlException("Entity is transient.");

            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            var update = Builders<TEntity>.Update.PullAll(expression, values);
            collection.UpdateOne(filter, update);
        }

        public void AddToSet<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, TValue value) where TEntity : class, INoSqlEntity<ObjectId>
        {
            if (entity.IsTransient)
                throw new NoSqlException("Entity is transient.");

            var collection = GetCollection<TEntity>();
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            var update = Builders<TEntity>.Update.AddToSet(expression, value);
            collection.UpdateOne(filter, update);
        }

        public void DropDatabase()
        {
            throw new NotImplementedException();
        }

        public void DropCollection<TEntity>()
        {
            _database.DropCollection(typeof(TEntity).Name);
        }
    }
}

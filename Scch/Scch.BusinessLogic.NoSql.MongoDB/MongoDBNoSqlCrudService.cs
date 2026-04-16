using System;
using MongoDB.Bson;
using Scch.DataAccess.NoSql.MongoDB;
using Scch.DomainModel.NoSql;
using Scch.Logging;

namespace Scch.BusinessLogic.NoSql.MongoDB
{
    public abstract class MongoDBNoSqlCrudService<TEntity> : NoSqlCrudService<ObjectId, TEntity>, IMongoDBNoSqlCrudService<TEntity>
        where TEntity : class, INoSqlEntity<ObjectId>, new()
    {
        private readonly IMongoDBNoSqlRepositoryFactory _factory;

        protected MongoDBNoSqlCrudService(IMongoDBNoSqlRepositoryFactory factory)
            : base(factory)
        {
            _factory = factory;
        }

        public void DropCollection()
        {
            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    repository.DropCollection<TEntity>();
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }
    }
}

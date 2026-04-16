using MongoDB.Bson;
using Scch.DomainModel.NoSql;

namespace Scch.BusinessLogic.NoSql.MongoDB
{
    public interface IMongoDBNoSqlCrudService<TEntity> : INoSqlCrudService<ObjectId, TEntity>
        where TEntity : INoSqlEntity<ObjectId>
    {
        void DropCollection();
    }
}

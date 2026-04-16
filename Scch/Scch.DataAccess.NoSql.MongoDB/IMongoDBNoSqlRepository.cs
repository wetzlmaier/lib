using MongoDB.Bson;

namespace Scch.DataAccess.NoSql.MongoDB
{
    public interface IMongoDBNoSqlRepository : INoSqlRepository<ObjectId>
    {
        void DropCollection<TEntity>();

    }
}

using MongoDB.Bson;

namespace Scch.DataAccess.NoSql.MongoDB
{
    public interface IMongoDBNoSqlRepositoryFactory : INoSqlRepositoryFactory<ObjectId>
    {
        new IMongoDBNoSqlRepository Create();
    }
}

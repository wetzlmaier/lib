using MongoDB.Bson;

namespace Scch.DomainModel.NoSql.MongoDB
{
    public interface IMongoDBNoSqlEntity : INoSqlEntity<ObjectId>
    {
    }
}

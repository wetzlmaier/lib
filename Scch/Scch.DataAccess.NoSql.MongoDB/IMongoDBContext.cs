using MongoDB.Driver;

namespace Scch.DataAccess.NoSql.MongoDB
{
    public interface IMongoDBContext
    {
        IMongoDatabase GetDatabase();
    }
}

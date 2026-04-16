using MongoDB.Driver;
using Scch.Common.Configuration;

namespace Scch.DataAccess.NoSql.MongoDB
{
    public class MongoDBContext : IMongoDBContext
    {
        public IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(GetConnectionString());
            return client.GetDatabase(GetDatabaseName());
        }

        private string GetConnectionString()
        {
            return ConfigurationHelper.Current.ReadString("MongoDbConnectionString", "mongodb://{SERVER}/{DB_NAME}?safe=true")
                .Replace("{SERVER}", GetServer())
                .Replace("{DB_NAME}", GetDatabaseName());
        }

        private string GetDatabaseName()
        {
            return ConfigurationHelper.Current.ReadString("MongoDbDatabaseName");
        }

        private string GetServer()
        {
            return ConfigurationHelper.Current.ReadString("MongoDbServer", "localhost");
        }
    }
}

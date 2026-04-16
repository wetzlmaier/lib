using System.ComponentModel.Composition;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Scch.Common.ComponentModel;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.MongoDB
{
    [Export(typeof(IMongoDBNoSqlRepositoryFactory))]
    [Export(typeof(INoSqlRepositoryFactory<ObjectId>))]
    public class MongoDBNoSqlRepositoryFactory : IMongoDBNoSqlRepositoryFactory
    {
        private readonly IMongoDBContext _context;

        static MongoDBNoSqlRepositoryFactory()
        {
            BsonClassMap.RegisterClassMap<DataErrorInfo>(cm => {
                cm.AutoMap();
                cm.UnmapProperty(c => c.IsValid);
            });
        }

        public MongoDBNoSqlRepositoryFactory()
            : this(new MongoDBContext())
        {

        }

        [ImportingConstructor]
        public MongoDBNoSqlRepositoryFactory(IMongoDBContext context)
        {
            _context = context;
        }

        public IMongoDBNoSqlRepository Create()
        {
            return new MongoDBNoSqlRepository(_context);
        }
        
        INoSqlRepository<ObjectId> IRepositoryFactory<INoSqlRepository<ObjectId>, ObjectId, INoSqlEntity<ObjectId>>.Create()
        {
            return Create();
        }

        public void Drop()
        {
            throw new System.NotImplementedException();
        }
    }
}

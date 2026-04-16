using System.ComponentModel.Composition;
using Scch.DomainModel.NoSql;
using Scch.Neo4j;

namespace Scch.DataAccess.NoSql.Neo4j
{
    [Export(typeof(INeo4jNoSqlRepositoryFactory))]
    [Export(typeof(INoSqlRepositoryFactory<Neo4jObjectId>))]
    public class Neo4jNoSqlRepositoryFactory : INeo4jNoSqlRepositoryFactory
    {
        private readonly INeo4jContext _context;

        public Neo4jNoSqlRepositoryFactory()
            : this(new Neo4jContext())
        {

        }

        [ImportingConstructor]
        public Neo4jNoSqlRepositoryFactory(INeo4jContext context)
        {
            _context = context;
        }

        public INeo4jNoSqlRepository Create()
        {
            return new Neo4jNoSqlRepository(_context);
        }

        INoSqlRepository<Neo4jObjectId> IRepositoryFactory<INoSqlRepository<Neo4jObjectId>, Neo4jObjectId, INoSqlEntity<Neo4jObjectId>>.Create()
        {
            return Create();
        }

        public void Drop()
        {
            using (var repository = Create())
            {
                repository.DropDatabase();
            }
        }
    }
}

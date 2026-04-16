using Scch.Neo4j;

namespace Scch.DataAccess.NoSql.Neo4j
{
    public interface INeo4jNoSqlRepositoryFactory : INoSqlRepositoryFactory<Neo4jObjectId>
    {
        new INeo4jNoSqlRepository Create();
    }
}

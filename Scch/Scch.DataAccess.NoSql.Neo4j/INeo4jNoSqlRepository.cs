using Scch.Neo4j;

namespace Scch.DataAccess.NoSql.Neo4j
{
    public interface INeo4jNoSqlRepository : INoSqlRepository<Neo4jObjectId>
    {
        void DropGraph<TEntity>();

        void CreateUniqueConstraint(string identity, string property);

        void DropUniqueConstraint(string label, string property);
    }
}

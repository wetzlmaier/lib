using Neo4jClient;

namespace Scch.DataAccess.NoSql.Neo4j
{
    public interface INeo4jContext
    {
        IGraphClient GetClient();
    }
}

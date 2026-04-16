using Newtonsoft.Json.Bson;
using Scch.Neo4j;

namespace Scch.DomainModel.NoSql.Neo4j
{
    public interface INeo4jNoSqlEntity : INoSqlEntity<Neo4jObjectId>
    {
    }
}

using System;
using Neo4jClient;
using Scch.Common.Configuration;

namespace Scch.DataAccess.NoSql.Neo4j
{
    public class Neo4jContext : INeo4jContext
    {
        public IGraphClient GetClient()
        {
            IGraphClient graphClient = new GraphClient(new Uri(GetConnectionString()), GetUsername(), GetPassword());
            graphClient.ConnectAsync().GetAwaiter().GetResult();
            return graphClient;
        }

        private string GetConnectionString()
        {
            return ConfigurationHelper.Current.ReadString("Neo4jConnectionString", "http://localhost:7474/db/data");
        }

        private string GetUsername()
        {
            return ConfigurationHelper.Current.ReadString("Username", "neo4j");
        }
        private string GetPassword()
        {
            return ConfigurationHelper.Current.ReadString("Password", "neo4j");
        }
    }
}

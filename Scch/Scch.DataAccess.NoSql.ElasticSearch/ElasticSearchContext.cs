using System;
using Elastic.Clients.Elasticsearch;
using Scch.Common.Configuration;

namespace Scch.DataAccess.NoSql.ElasticSearch
{
    public class ElasticSearchContext : IElasticSearchContext
    {
        public ElasticsearchClient GetClient()
        {
            return new ElasticsearchClient(new Uri(GetConnectionString()));
        }

        private string GetConnectionString()
        {
            return ConfigurationHelper.Current.ReadString("ElasticSearchConnectionString", "http://{SERVER}:9200")
                .Replace("{SERVER}", GetServer());
        }

        private string GetServer()
        {
            return ConfigurationHelper.Current.ReadString("ElasticSearchServer", "localhost");
        }
    }
}

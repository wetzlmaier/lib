using Elastic.Clients.Elasticsearch;
namespace Scch.DataAccess.NoSql.ElasticSearch
{
    public interface IElasticSearchContext
    {
        ElasticsearchClient GetClient();
    }
}

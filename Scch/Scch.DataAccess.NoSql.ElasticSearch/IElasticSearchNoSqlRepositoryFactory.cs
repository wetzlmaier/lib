using System;

namespace Scch.DataAccess.NoSql.ElasticSearch
{
    public interface IElasticSearchNoSqlRepositoryFactory : INoSqlRepositoryFactory<Guid>
    {
        new IElasticSearchNoSqlRepository Create();
    }
}

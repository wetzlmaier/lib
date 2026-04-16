using System;

namespace Scch.DomainModel.NoSql.ElasticSearch
{
    public class ElasticSearchEntity : NoSqlEntity<Guid>, IElasticSearchEntity
    {
        public override bool IsTransient => Id == Guid.Empty;
    }
}

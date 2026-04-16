using System;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.Xml
{
    public interface IXmlNoSqlRepository : INoSqlRepository<Guid> 
    {
        void DropCollection<TEntity>() where TEntity : class, INoSqlEntity<Guid>;
    }
}

using System;
using Scch.DomainModel.NoSql;

namespace Scch.BusinessLogic.NoSql.Xml
{
    public interface IXmlNoSqlCrudService<TEntity> : INoSqlCrudService<Guid, TEntity>
        where TEntity : INoSqlEntity<Guid>
    {
        void DropCollection();
    }
}

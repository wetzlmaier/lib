using System;

namespace Scch.DataAccess.NoSql.Xml
{
    public interface IXmlNoSqlRepositoryFactory : INoSqlRepositoryFactory<Guid>
    {
        new IXmlNoSqlRepository Create();
    }
}

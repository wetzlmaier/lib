using System;
using Scch.DataAccess.NoSql.Xml;
using Scch.DomainModel.NoSql;
using Scch.Logging;

namespace Scch.BusinessLogic.NoSql.Xml
{
    public abstract class XmlNoSqlCrudService<TEntity> : NoSqlCrudService<Guid, TEntity>, IXmlNoSqlCrudService<TEntity>
        where TEntity : class, INoSqlEntity<Guid>, new()
    {
        private readonly IXmlNoSqlRepositoryFactory _factory;

        protected XmlNoSqlCrudService(IXmlNoSqlRepositoryFactory factory)
            : base(factory)
        {
            _factory = factory;
        }

        public void DropCollection()
        {
            try
            {
                using (var repository = _factory.Create())
                {
                    var start = DateTime.Now;
                    repository.DropCollection<TEntity>();
                    Logger.Write(new PerformanceLogEntry(start, DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
                throw;
            }
        }
    }
}

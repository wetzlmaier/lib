using System;
using System.ComponentModel.Composition;
using System.IO;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.Xml
{
    [Export(typeof(IXmlNoSqlRepositoryFactory))]
    [Export(typeof(INoSqlRepositoryFactory<Guid>))]
    public class XmlNoSqlRepositoryFactory : IXmlNoSqlRepositoryFactory
    {
        private readonly IXmlContext _context;

        public XmlNoSqlRepositoryFactory()
            : this(new XmlContext())
        {

        }

        [ImportingConstructor]
        public XmlNoSqlRepositoryFactory(IXmlContext context)
        {
            _context = context;
        }

        public IXmlNoSqlRepository Create()
        {
            return new XmlNoSqlRepository(_context.RootDirectory, _context.Encoding);
        }

        INoSqlRepository<Guid> IRepositoryFactory<INoSqlRepository<Guid>, Guid, INoSqlEntity<Guid>>.Create()
        {
            return Create();
        }

        public void Drop()
        {
            if (Directory.Exists(_context.RootDirectory))
                Directory.Delete(_context.RootDirectory, true);
        }
    }
}

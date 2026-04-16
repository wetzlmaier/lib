using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.ElasticSearch
{
    [Export(typeof(IElasticSearchNoSqlRepositoryFactory))]
    [Export(typeof(INoSqlRepositoryFactory<Guid>))]
    public class ElasticSearchNoSqlRepositoryFactory: IElasticSearchNoSqlRepositoryFactory
    {
        private readonly IElasticSearchContext _context;

        public ElasticSearchNoSqlRepositoryFactory()
            : this(new ElasticSearchContext())
        {
        }

        [ImportingConstructor]
        public ElasticSearchNoSqlRepositoryFactory(IElasticSearchContext context)
        {
            _context = context;
        }

        public IElasticSearchNoSqlRepository Create()
        {
            return new ElasticSearchNoSqlRepository(_context);
        }

        INoSqlRepository<Guid> IRepositoryFactory<INoSqlRepository<Guid>, Guid, INoSqlEntity<Guid>>.Create()
        {
            return Create();
        }

        public void Drop()
        {
            using (var repository = Create())
            {
                repository.DropDatabase();
            }
        }
    }
}

using System.ComponentModel.Composition;
using Scch.BusinessLogic.Hibernate;
using Scch.DataAccess.Hibernate;

namespace Scch.BusinessLogic.EntityFramework
{
    [Export(typeof(IRepositoryFactory))]
    public class RepositoryFactory : HibernateRepositoryFactory<long>, IRepositoryFactory
    {
        [ImportingConstructor]
        public RepositoryFactory(IModelContext modelContext)
            : base(modelContext.ConnectionStringName, modelContext.EntityAssemblies)
        {

        }
    }
}

using System.ComponentModel.Composition;
using Scch.DataAccess.EntityFramework;
using Scch.DomainModel.EntityFramework;

namespace Scch.BusinessLogic.EntityFramework
{
    [Export(typeof(IRepositoryFactory))]
    public class RepositoryFactory : EntityFrameworkRepositoryFactory<long>, IRepositoryFactory
    {
        [ImportingConstructor]
        public RepositoryFactory(IModelContext modelContext)
            : base(modelContext.ConnectionStringName, modelContext.EntityAssemblies, modelContext.DeleteDatabaseIfExists, modelContext.LazyLoadingEnabled, modelContext.AuditingEnabled, modelContext.SaveSqlScript, modelContext.MasterDataScript)
        {

        }
    }
}

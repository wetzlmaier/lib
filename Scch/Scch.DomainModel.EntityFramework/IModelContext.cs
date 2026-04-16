namespace Scch.DomainModel.EntityFramework
{
    public interface IModelContext
    {
        string[] EntityAssemblies { get; }

        bool DeleteDatabaseIfExists { get; }

        bool LazyLoadingEnabled { get; }

        string ConnectionStringName { get; }

        bool AuditingEnabled { get; }

        bool SaveSqlScript { get; }

        string MasterDataScript { get; }
    }
}

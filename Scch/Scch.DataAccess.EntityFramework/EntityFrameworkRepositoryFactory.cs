using System;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.EntityFramework
{
    public abstract class EntityFrameworkRepositoryFactory<TKey> : IEntityFrameworkRepositoryFactory<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        private static bool _isInitialized;
        private readonly string _connectionStringName;
        private readonly string _masterDataScript;

        protected EntityFrameworkRepositoryFactory(string connectionStringName, string[] entityAssemblies, bool deleteDatabaseIfExists, bool lazyLoadingEnabled, bool auditingEnabled, bool saveSqlScript, string masterDataScript)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already initialized");

            _connectionStringName = connectionStringName;
            DbContextManager<TKey>.Init(connectionStringName, entityAssemblies, deleteDatabaseIfExists, lazyLoadingEnabled, auditingEnabled, saveSqlScript);
            _masterDataScript = masterDataScript;
            _isInitialized = true;
        }

        public IEntityFrameworkRepository<TKey> Create()
        {
            return new EntityFrameworkRepository<TKey>(DbContextManager<TKey>.CreateContext(_connectionStringName, _masterDataScript));
        }


        IEntityFrameworkRepository<TKey> IRepositoryFactory<IEntityFrameworkRepository<TKey>, TKey, IEntityFrameworkEntity<TKey>>.Create()
        {
            return Create();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }
    }
}

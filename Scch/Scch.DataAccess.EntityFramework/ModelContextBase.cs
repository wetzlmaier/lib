using System.Collections.Generic;
using System.IO;
using Scch.Common.Configuration;

namespace Scch.DataAccess.EntityFramework
{
    public abstract class ModelContextBase : IModelContext
    {
        protected ModelContextBase(string connectionStringName, string[] entityAssemblies, string masterDataScript)
            : this(connectionStringName, entityAssemblies, masterDataScript, ConfigurationHelper.Current.ReadBool("DeleteDatabaseIfExists", false),
            ConfigurationHelper.Current.ReadBool("LazyLoadingEnabled", true), ConfigurationHelper.Current.ReadBool("AuditingEnabled", false),
            ConfigurationHelper.Current.ReadBool("SaveSqlScript", false))
        {

        }

        protected ModelContextBase(string connectionStringName, string[] entityAssemblies, string masterDataScript, bool deleteDatabaseIfExists, bool lazyLoadingEnabled, bool auditingEnabled, bool saveSqlScript)
        {
            ConnectionStringName = connectionStringName;
            var list = new List<string>(entityAssemblies) { "Scch.DomainModel", "Scch.DomainModel.EntityFramework" };
            EntityAssemblies = list.ToArray();
            MasterDataScript = masterDataScript;
            DeleteDatabaseIfExists = deleteDatabaseIfExists;
            LazyLoadingEnabled = lazyLoadingEnabled;
            AuditingEnabled = auditingEnabled;
            SaveSqlScript = saveSqlScript;
        }

        protected static string ReadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            return File.ReadAllText(fileName);
        }

        public virtual string[] EntityAssemblies { get; private set; }

        public virtual bool DeleteDatabaseIfExists { get; private set; }

        public bool LazyLoadingEnabled { get; private set; }

        public string ConnectionStringName { get; private set; }

        public bool AuditingEnabled { get; private set; }

        public bool SaveSqlScript { get; private set; }

        public string MasterDataScript { get; private set; }
    }
}

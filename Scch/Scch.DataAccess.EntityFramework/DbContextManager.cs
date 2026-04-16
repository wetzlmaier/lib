using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Scch.DataAccess.EntityFramework
{
    public static class DbContextManager<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Maintains a dictionary of db context builders, one per database.  The key is a 
        /// connection string name used to look up the associated database, and used to decorate respective
        /// repositories. If only one database is being used, this dictionary contains a single
        /// factory with a key of <see cref="DefaultConnectionStringName" />.
        /// </summary>
        private static readonly Dictionary<string, IDbContextBuilder<ExtendedDbContext<TKey>, TKey>> DbContextBuilders = new Dictionary<string, IDbContextBuilder<ExtendedDbContext<TKey>, TKey>>();

        public static void Init(string[] mappingAssemblies, bool deleteDatabaseIfExists = false, bool lazyLoadingEnabled = false, bool auditingEnabled = false, bool saveSqlScript = false)
        {
            Init(DefaultConnectionStringName, mappingAssemblies, deleteDatabaseIfExists, lazyLoadingEnabled, auditingEnabled, saveSqlScript);
        }

        public static void Init(string connectionStringName, string[] mappingAssemblies, bool deleteDatabaseIfExists = false, bool lazyLoadingEnabled = false, bool auditingEnabled = false, bool saveSqlScript = false)
        {
            if (deleteDatabaseIfExists)
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];

                var factory = DbProviderFactories.GetFactory(settings.ProviderName);
                using (DbConnection conn = factory.CreateConnection())
                {
                    if (conn != null)
                    {
                        conn.ConnectionString = settings.ConnectionString;

                        if (Database.Exists(conn))
                        {
                            var b = (SqlConnectionStringBuilder)factory.CreateConnectionStringBuilder();
                            b.ConnectionString = settings.ConnectionString;

                            try
                            {
                                conn.Open();
                                var cmd = conn.CreateCommand();
                                cmd.CommandText = string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", b.InitialCatalog);
                                cmd.ExecuteNonQuery();
                            }
                            finally
                            {
                                conn.Close();
                            }

                            Database.Delete(conn);
                        }
                    }
                }
            }

            AddConfiguration(connectionStringName, mappingAssemblies, lazyLoadingEnabled, auditingEnabled, saveSqlScript);
        }

        /// <summary>
        /// The default connection string name used if only one database is being communicated with.
        /// </summary>
        public static readonly string DefaultConnectionStringName = "Database";

        public static ExtendedDbContext<TKey> CreateContext()
        {
            lock (typeof(DbContextManager<TKey>))
            {
                return CreateContext(DefaultConnectionStringName);
            }
        }

        /// <summary>
        /// Used to get the current DbContext associated with a key; i.e., the key 
        /// associated with an object context for a specific database.
        /// 
        /// If you're only communicating with one database, you should call <see cref="CreateContext()" /> instead,
        /// although you're certainly welcome to call this if you have the key available.
        /// </summary>
        public static ExtendedDbContext<TKey> CreateContext(string key, string masterDataScript = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            lock (typeof(DbContextManager<TKey>))
            {
                if (!DbContextBuilders.ContainsKey(key))
                {
                    throw new ApplicationException("An DbContextBuilder does not exist with a key of " + key);
                }

                return DbContextBuilders[key].BuildDbContext(masterDataScript);
            }
        }

        private static void AddConfiguration(string connectionStringName, string[] mappingAssemblies, bool lazyLoadingEnabled = true, bool auditingEnabled = false, bool saveSqlScript = false)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionStringName");
            }

            if (mappingAssemblies == null)
            {
                throw new ArgumentNullException("mappingAssemblies");
            }

            lock (typeof(DbContextManager<TKey>))
            {
                DbContextBuilders.Add(connectionStringName,
                    new DbContextBuilder<ExtendedDbContext<TKey>, TKey>(connectionStringName, mappingAssemblies, lazyLoadingEnabled, auditingEnabled, saveSqlScript));
            }
        }
    }
}

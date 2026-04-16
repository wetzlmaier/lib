using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Scch.Common.IO;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.EntityFramework
{
    public sealed class DbContextBuilder<TContext, TKey> : DbModelBuilder, IDbContextBuilder<TContext, TKey>
        where TContext : ExtendedDbContext<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly DbProviderFactory _factory;
        private readonly ConnectionStringSettings _cnStringSettings;
        private readonly bool _lazyLoadingEnabled;
        private readonly bool _auditingEnabled;
        private readonly object _syncRoot = new object();
        private readonly IDictionary<Type, DbTypeMetadata> _metadata;
        private readonly bool _saveSqlScript;

        public DbContextBuilder(string connectionStringName, string[] mappingAssemblies, bool lazyLoadingEnabled, bool auditingEnabled=false, bool saveSqlScript=false)
        {
            _metadata = new Dictionary<Type, DbTypeMetadata>();

#pragma warning disable 612,618
            Conventions.Remove<IncludeMetadataConvention>();
#pragma warning restore 612,618
            Conventions.Remove<PluralizingTableNameConvention>();

            _cnStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

            _factory = DbProviderFactories.GetFactory(_cnStringSettings.ProviderName);
            _lazyLoadingEnabled = lazyLoadingEnabled;
            _auditingEnabled = auditingEnabled;
            _saveSqlScript = saveSqlScript;

            AddConfigurations(mappingAssemblies);
        }

        /// <summary>
        /// Creates a new <see cref="DbContext"/>.
        /// </summary>
        /// <c>lazyLoadingEnabled</c> if set to <c>true</c> [lazy loading enabled].
        /// <c>recreateDatabaseIfExist</c> if set to <c>true</c> [recreate database if exist].
        /// <returns></returns>
        public TContext BuildDbContext(string masterDataScript=null)
        {
            var cn = _factory.CreateConnection();
            if (cn == null)
                throw new ConfigurationErrorsException("Could not create a connection.");

            cn.ConnectionString = _cnStringSettings.ConnectionString;
            var dbModel = Build(cn);

            var ctx = dbModel.Compile().CreateObjectContext<ObjectContext>(cn);
            ctx.ContextOptions.LazyLoadingEnabled = _lazyLoadingEnabled;
            ctx.ContextOptions.ProxyCreationEnabled = false;

            var dbContext = (TContext)new ExtendedDbContext<TKey>(ctx, true, _metadata.Values, _auditingEnabled);

            if (!dbContext.DatabaseExists())
            {
                lock (_syncRoot)
                {
                    if (!dbContext.DatabaseExists())
                    {
                        dbContext.CreateDatabase();

                        if (_saveSqlScript)
                        {
                            File.WriteAllText(_cnStringSettings.Name + ".SQL", dbContext.CreateDatabaseScript());
                        }

                        if (!string.IsNullOrEmpty(masterDataScript))
                        {
                            dbContext.FillDatabase(masterDataScript);
                        }
                    }
                }
            }

            return dbContext;
        }

        /// <summary>
        /// Adds mapping classes contained in provided assemblies and register entities as well
        /// </summary>
        /// <param name="mappingAssemblies"></param>
        private void AddConfigurations(string[] mappingAssemblies)
        {
            if (mappingAssemblies == null || mappingAssemblies.Length == 0)
            {
                throw new ArgumentNullException("mappingAssemblies", "You must specify at least one mapping assembly");
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += AssemblyResolve;
            bool hasMappingClass = false;
            foreach (string mappingAssembly in mappingAssemblies)
            {
                Assembly asm = currentDomain.Load(MakeLoadReadyAssemblyName(mappingAssembly));

                foreach (Type type in asm.GetTypes())
                {
                    if (!type.IsAbstract)
                    {
                        if (IsMappingClass(type.BaseType))
                        {
                            hasMappingClass = true;

                            // http://areaofinterest.wordpress.com/2010/12/08/dynamically-load-entity-configurations-in-ef-codefirst-ctp5/
                            dynamic configurationInstance = Activator.CreateInstance(type);
                            Configurations.Add(configurationInstance);
                        }

                        if (IsEntityClass(type))
                        {
                            if (_metadata.ContainsKey(type))
                                continue;

                            _metadata.Add(type, new DbTypeMetadata(type));
                        }
                    }
                }
            }

            currentDomain.AssemblyResolve -= AssemblyResolve;

            if (!hasMappingClass)
            {
                throw new ArgumentException("No mapping class found!");
            }
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            try
            {
                assembly = Assembly.LoadFrom(args.Name);
            }
            catch (FileNotFoundException)
            {
            }
            catch (FileLoadException)
            {
            }

            if (assembly == null)
            {
                string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string fullname = (directory != null) ? Path.Combine(directory, args.Name) : args.Name;
                assembly = Assembly.LoadFrom(fullname);
            }

            return assembly;
        }

        /// <summary>
        /// Determines whether a type is a subclass of entity mapping type
        /// </summary>
        /// <param name="mappingType">Type of the mapping.</param>
        /// <returns>
        /// 	<c>true</c> if it is mapping class; otherwise, <c>false</c>.
        /// </returns>
        private bool IsMappingClass(Type mappingType)
        {
            Type baseType = typeof(EntityTypeConfiguration<>);
            if (mappingType.IsGenericType && mappingType.GetGenericTypeDefinition() == baseType)
            {
                return true;
            }

            if ((mappingType != typeof(object)))
            {
                return IsMappingClass(mappingType.BaseType);
            }
            return false;
        }

        private bool IsEntityClass(Type entityType)
        {
            return typeof(IEntityFrameworkEntity<TKey>).IsAssignableFrom(entityType);
        }

        /// <summary>
        /// Ensures the assembly name is qualified
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private static string MakeLoadReadyAssemblyName(string assemblyName)
        {
            if (assemblyName == null)
                throw new ArgumentNullException("assemblyName");

            return (assemblyName.IndexOf(FileHelper.DllFileType, StringComparison.Ordinal) == -1)
                ? assemblyName.Trim() + FileHelper.DllFileType
                : assemblyName.Trim();
        }
    }
}

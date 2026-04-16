using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Scch.DomainModel.Hibernate;

namespace Scch.DataAccess.Hibernate
{
    public class HibernateRepositoryFactory<TKey> : IHibernateRepositoryFactory<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>
    {
        private static bool _isInitialized;
        private readonly string _connectionString;
        private readonly ISessionFactory _sessionFactory;

        protected HibernateRepositoryFactory(string connectionString, string[] entityAssemblies)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already initialized");

            _connectionString = connectionString;

            var configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString)
                    .Driver<MicrosoftDataSqlClientDriver>()
                    .ShowSql())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true));

            foreach (var assembly in entityAssemblies) 
            {
                configuration= configuration.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load(assembly)));
            }

            _sessionFactory = configuration.BuildSessionFactory();

            _isInitialized = true;
        }

        public IHibernateRepository<TKey> Create()
        {
            return new HibernateRepository<TKey>(_sessionFactory.OpenStatelessSession());
        }
        
        IHibernateRepository<TKey> IRepositoryFactory<IHibernateRepository<TKey>, TKey, IHibernateEntity<TKey>>.Create()
        {
            return Create();
        }

        public void Drop()
        {
            throw new NotImplementedException();
        }
    }
}

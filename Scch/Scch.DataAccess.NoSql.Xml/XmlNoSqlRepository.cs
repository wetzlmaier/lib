using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Scch.Common.IO;
using Scch.Common.Threading.Tasks;
using Scch.Common.Xml;
using Scch.DomainModel.NoSql;

namespace Scch.DataAccess.NoSql.Xml
{
    public class XmlNoSqlRepository : IXmlNoSqlRepository
    {
        private const string Extension = ".xml";
        private const string Filter = "*" + Extension;
        private readonly string _rootDirectory;
        private readonly IDictionary<Type, IDictionary<Guid, INoSqlEntity<Guid>>> _collections;
        private readonly Encoding _encoding;
        private readonly object _syncRoot;

        public XmlNoSqlRepository(string rootDirectory, Encoding encoding)
        {
            _syncRoot = new object();
            _rootDirectory = rootDirectory;
            _encoding = encoding;

            _collections = new Dictionary<Type, IDictionary<Guid, INoSqlEntity<Guid>>>();

            CreateDirectory(_rootDirectory);
        }

        private IDictionary<Guid, INoSqlEntity<Guid>> GetCollection<TEntity>()
        {
            if (!_collections.ContainsKey(typeof(TEntity)))
            {
                lock (_syncRoot)
                {
                    if (!_collections.ContainsKey(typeof(TEntity)))
                    {
                        var newCollection = new Dictionary<Guid, INoSqlEntity<Guid>>();
                        _collections.Add(typeof(TEntity), newCollection);
                        return newCollection;
                    }
                }
            }

            return _collections[typeof(TEntity)];
        }

        private void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                lock (_syncRoot)
                {
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                }
            }
        }

        private string GetDirectory<TEntity>()
        {
            var directory = Path.Combine(_rootDirectory, typeof(TEntity).Name);
            CreateDirectory(directory);
            return directory;
        }

        private string GetFileName<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            return Path.Combine(GetDirectory<TEntity>(), entity.Id + Extension);
        }

        public void Dispose()
        {
        }

        public void DeleteAll<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            lock (_syncRoot)
            {
                ParallelHelper.ExecuteAction(Directory.GetFiles(GetDirectory<TEntity>()), fileName =>
                {
                    var file = new FileInfo(fileName);

                    if (file.Exists)
                        file.Delete();
                });

                GetCollection<TEntity>().Clear();
            }
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void InsertBatch<TEntity>(IEnumerable<TEntity> entity) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity Save<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            if (entity.IsTransient)
                entity.Id = Guid.NewGuid();

            Serializer.SerializeToFile(new FileInfo(GetFileName(entity)), entity, _encoding);
            return entity;
        }

        public void UpdatePush<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void UpdatePull<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, params TValue[] values) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void AddToSet<TEntity, TValue>(TEntity entity, Expression<Func<TEntity, IEnumerable<TValue>>> expression, TValue value) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void DropDatabase()
        {
            throw new NotImplementedException();
        }

        public void DropCollection<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            DeleteAll<TEntity>();

            var collectionDirectory = GetDirectory<TEntity>();
            if (Directory.Exists(collectionDirectory))
                Directory.Delete(collectionDirectory, true);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<Guid>
        {
            return GetQueryable<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            return GetAll<TEntity>().AsQueryable();
        }

        public TEntity Single<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public TEntity First<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, INoSqlEntity<Guid>
        {
            lock (_syncRoot)
            {
                var file = new FileInfo(GetFileName(entity));
                if (file.Exists)
                    file.Delete();

                if (GetCollection<TEntity>().ContainsKey(entity.Id))
                    GetCollection<TEntity>().Remove(entity.Id);
            }
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            foreach (var entity in GetQueryable(criteria).ToArray())
                Delete(entity);
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            return GetQueryable<TEntity>().Where(criteria);
        }

        public TEntity FindOne<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            return GetQueryable<TEntity>().Where(criteria).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            lock (_syncRoot)
            {
                var localSyncRoot = new object();

                var collection = GetCollection<TEntity>();

                ParallelHelper.ExecuteAction(Directory.GetFiles(GetDirectory<TEntity>(), Filter), file =>
                {
                    var fileInfo = new FileInfo(file);
                    var id = Guid.Parse(FileHelper.FileNameWithoutExtension(fileInfo.Name));

                    if (collection.ContainsKey(id))
                        return;

                    var xml = File.ReadAllText(fileInfo.FullName);
                    var entity = (TEntity)Serializer.DeserializeFromString(typeof(TEntity), xml, _encoding);

                    lock (localSyncRoot)
                    {
                        if (!collection.ContainsKey(entity.Id))
                            collection.Add(entity.Id, entity);
                    }
                });

                return collection.Values.Cast<TEntity>();
            }
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public long Count<TEntity>() where TEntity : class, INoSqlEntity<Guid>
        {
            return Directory.GetFiles(GetDirectory<TEntity>(), Filter).Length;
        }

        public long Count<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class, INoSqlEntity<Guid>
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FilterDeleted<TEntity>(IQueryable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}

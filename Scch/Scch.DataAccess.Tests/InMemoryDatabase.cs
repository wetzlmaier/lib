using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scch.Common;
using Scch.DomainModel.EntityFramework;

namespace Scch.DataAccess.Tests
{
    public abstract class InMemoryDatabase<TKey, TEntityBase> : Disposable
        where TEntityBase : IEntityFrameworkEntity<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private IDictionary<Type, IList> _database;

        protected InMemoryDatabase()
        {
            ResetDatabase();
        }

        public IList<T> GetList<T>() where T : IEntityFrameworkEntity<TKey>
        {
            return (IList<T>)GetList(typeof(T));
        }

        public IList GetList(Type type)
        {
            return (IList)ObjectCloner.DeepClone(GetInternalList(type));
        }

        public IList<T> GetInternalList<T>() where T : IEntityFrameworkEntity<TKey>
        {
            return (IList<T>)GetInternalList(typeof(T));
        }

        public IList GetInternalList(Type type)
        {
            if (!_database.ContainsKey(type))
            {
                var genericListType = typeof(List<>).MakeGenericType(type);
                _database.Add(type, (IList)Activator.CreateInstance(genericListType));
            }

            return _database[type];
        }

        protected void ResetDatabase()
        {
            _database = new Dictionary<Type, IList>();
        }

        public TKey NextId<T>() where T : TEntityBase
        {
            return NextId(typeof(T));
        }

        public TKey NextId(Type type)
        {
            throw new NotImplementedException();
            /*
            var list = GetInternalList(type).Cast<TEntityBase>().ToArray();

            if (!list.Any())
                return 1;

            return list.Max(e => e.Id) + 1;*/
        }

        public ICollection<Type> Types
        {
            get { return _database.Keys; }
        }

        protected void Remove<T>(T entity) where T : IEntityFrameworkEntity<TKey>
        {
            var list = GetInternalList<T>();
            var reference = list.Single(e => Equals(e.Id, entity.Id));
            list.Remove(reference);
        }
    }
}

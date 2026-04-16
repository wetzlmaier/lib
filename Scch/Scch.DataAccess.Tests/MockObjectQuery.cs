using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using Scch.Common;
using Scch.DataAccess.EntityFramework;

namespace Scch.DataAccess.Tests
{
    public class MockObjectQuery<TEntity> : IObjectQuery<TEntity>
    {
        private readonly List<TEntity> _entities;

        public MockObjectQuery()
        {
            _entities = new List<TEntity>();
        }

        internal MockObjectQuery(IEnumerable<TEntity> entities)
        {
            _entities = new List<TEntity>(entities);
        }

        private IQueryable<TEntity> AsQueryable()
        {
            return _entities.AsQueryable();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression
        {
            get { return AsQueryable().Expression; }
        }

        public Type ElementType
        {
            get { return typeof(TEntity); }
        }

        public IQueryProvider Provider
        {
            get { return AsQueryable().Provider; }
        }

        public IList GetList()
        {
            return _entities.Select(e=>ObjectCloner.DeepClone(e)).ToList();
        }

        public bool ContainsListCollection
        {
            get { return true; }
        }

        public IObjectQuery<TEntity> Distinct()
        {
            return new MockObjectQuery<TEntity>(AsQueryable().Distinct());
        }

        public IObjectQuery<TEntity> Except(IObjectQuery<TEntity> query)
        {
            return new MockObjectQuery<TEntity>(AsQueryable().Except(query));
        }

        public IObjectQuery<TEntity> Include(Expression<Func<TEntity, object>> path)
        {
            return this;
        }

        public IObjectQuery<TEntity> Include(string path)
        {
            return this;
        }

        public IObjectQuery<TEntity> Intersect(IObjectQuery<TEntity> query)
        {
            return new MockObjectQuery<TEntity>(AsQueryable().Intersect(query));
        }

        public IObjectQuery<TResultType> OfType<TResultType>() 
        {
            return new MockObjectQuery<TResultType>(_entities.OfType<TResultType>());
        }

        public IObjectQuery<TEntity> Union(IObjectQuery<TEntity> query)
        {
            return new MockObjectQuery<TEntity>(AsQueryable().Union(query));
        }

        public IObjectQuery<TEntity> UnionAll(IObjectQuery<TEntity> query)
        {
            var result=new List<TEntity>(query.AsEnumerable());
            result.AddRange(_entities.Select(entity => (TEntity) ObjectCloner.DeepClone(entity)));
            return new MockObjectQuery<TEntity>(result.AsQueryable());
        }

        public string ToTraceString()
        {
            return string.Empty;
        }

        public MergeOption MergeOption
        {
            get;
            set;
        }
    }
}

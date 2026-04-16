using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using Scch.Common.Linq.Expressions;

namespace Scch.DataAccess.EntityFramework
{
    public class ObjectQueryWrapper<TEntity> : IObjectQuery<TEntity>
    {
        private readonly ObjectQuery<TEntity> _query;

        public ObjectQueryWrapper(ObjectQuery<TEntity> query)
        {
            _query = query;
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return ((IEnumerable<TEntity>)_query).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_query).GetEnumerator();
        }

        Expression IQueryable.Expression
        {
            get { return ((IQueryable)_query).Expression; }
        }

        Type IQueryable.ElementType
        {
            get { return ((IQueryable)_query).ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return ((IQueryable)_query).Provider; }
        }

        IList IListSource.GetList()
        {
            return ((IListSource)_query).GetList();
        }

        public bool ContainsListCollection
        {
            get { return ((IListSource)_query).ContainsListCollection; }
        }

        public IObjectQuery<TEntity> Distinct()
        {
            return new ObjectQueryWrapper<TEntity>(_query.Distinct());
        }

        public IObjectQuery<TEntity> Except(IObjectQuery<TEntity> query)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Except((ObjectQuery<TEntity>)query));
        }

        public IObjectQuery<TEntity> Include(string path)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Include(path));
        }

        public IObjectQuery<TEntity> Include(Expression<Func<TEntity, object>> path)
        {
            return Include(ExpressionHelper.GetPropertyPath(path));
        }
        
        public IObjectQuery<TEntity> Intersect(IObjectQuery<TEntity> query)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Intersect((ObjectQuery<TEntity>)query));
        }

        public IObjectQuery<TResultType> OfType<TResultType>()
        {
            return new ObjectQueryWrapper<TResultType>(_query.OfType<TResultType>());
        }

        public IObjectQuery<TEntity> OrderBy(string keys, params ObjectParameter[] parameters)
        {
            return new ObjectQueryWrapper<TEntity>(_query.OrderBy(keys, parameters));
        }

        public IObjectQuery<DbDataRecord> Select(string projection, params ObjectParameter[] parameters)
        {
            return new ObjectQueryWrapper<DbDataRecord>(_query.Select(projection, parameters));
        }

        public IObjectQuery<TResultType> SelectValue<TResultType>(string projection, params ObjectParameter[] parameters)
        {
            return new ObjectQueryWrapper<TResultType>(_query.SelectValue<TResultType>(projection, parameters));
        }

        public IObjectQuery<TEntity> Skip(string keys, string count, params ObjectParameter[] parameters)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Skip(keys, count, parameters));
        }

        public IObjectQuery<TEntity> Top(string count, params ObjectParameter[] parameters)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Top(count, parameters));
        }

        public IObjectQuery<TEntity> Union(IObjectQuery<TEntity> query)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Union((ObjectQuery<TEntity>)query));
        }

        public IObjectQuery<TEntity> UnionAll(IObjectQuery<TEntity> query)
        {
            return new ObjectQueryWrapper<TEntity>(_query.UnionAll((ObjectQuery<TEntity>)query));
        }

        public IObjectQuery<TEntity> Where(string predicate, params ObjectParameter[] parameters)
        {
            return new ObjectQueryWrapper<TEntity>(_query.Where(predicate, parameters));
        }

        public string ToTraceString()
        {
            return _query.ToTraceString();
        }

        public MergeOption MergeOption
        {
            get { return _query.MergeOption; }
            set { _query.MergeOption = value; }
        }
    }
}

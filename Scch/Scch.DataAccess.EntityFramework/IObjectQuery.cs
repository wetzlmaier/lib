using System;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Scch.DataAccess.EntityFramework
{
    public interface IObjectQuery<T> : IOrderedQueryable<T>, IListSource
    {
        IObjectQuery<T> Distinct();
        IObjectQuery<T> Except(IObjectQuery<T> query);
        //ObjectResult<T> Execute(MergeOption mergeOption);
        //IObjectQuery<DbDataRecord> GroupBy(string keys, string projection, params ObjectParameter[] parameters);
        IObjectQuery<T> Include(Expression<Func<T, object>> path);
        IObjectQuery<T> Include(string path);
        IObjectQuery<T> Intersect(IObjectQuery<T> query);
        IObjectQuery<TResultType> OfType<TResultType>();
        //IObjectQuery<T> OrderBy(string keys, params ObjectParameter[] parameters);
        //IObjectQuery<DbDataRecord> Select(string projection, params ObjectParameter[] parameters);
        //IObjectQuery<TResultType> SelectValue<TResultType>(string projection, params ObjectParameter[] parameters);
        //IObjectQuery<T> Skip(string keys, string count, params ObjectParameter[] parameters);
        //IObjectQuery<T> Top(string count, params ObjectParameter[] parameters);
        IObjectQuery<T> Union(IObjectQuery<T> query);
        IObjectQuery<T> UnionAll(IObjectQuery<T> query);
        //IObjectQuery<T> Where(string predicate, params ObjectParameter[] parameters);

        //TypeUsage GetResultType();
        string ToTraceString();

        //string CommandText { get; }
        //ObjectContext Context { get; }
        //bool EnablePlanCaching { get; set; }
        MergeOption MergeOption { get; set; }
        //ObjectParameterCollection Parameters { get; }
        //void Detach(T entity, params Expression<Func<T, object>>[] paths);
    }
}

using System;

namespace Scch.DataAccess.EntityFramework
{
    public interface IDbContextBuilder<out TContext, TKey>
        where TContext : ExtendedDbContext<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        TContext BuildDbContext(string masterDataScript = null);
    }
}

using System.Collections.Generic;

namespace Scch.Common.Collections.Generic
{
    public static class IListExtensions
    {
        public static void AddRange<TSource>(this IList<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}

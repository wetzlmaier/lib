using System.Collections.Generic;
using System.Linq;

namespace Scch.Common.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static TSource[] ToArray<TSource>(IEnumerable<TSource> source)
        {
            return source.Cast<TSource>().ToArray();
        }
    }
}

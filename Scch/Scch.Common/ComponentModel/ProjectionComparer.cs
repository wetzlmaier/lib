using System;
using System.Collections.Generic;

namespace Scch.Common.ComponentModel
{
    public static class ProjectionComparer
    {
        public static ProjectionComparer<TSource, TKey> Create<TSource, TKey>(Func<TSource, TKey> keySelector)
        {
            return new ProjectionComparer<TSource, TKey>(keySelector);
        }
    }

    public sealed class ProjectionComparer<TSource, TKey> : Comparer<TSource>
    {
        private readonly Func<TSource, TKey> _keySelector;
        private readonly IComparer<TKey> _keyComparer;

        public ProjectionComparer(Func<TSource, TKey> keySelector)
        {
            _keySelector = keySelector;
            _keyComparer = Comparer<TKey>.Default;
        }

        public override int Compare(TSource x, TSource y)
        {
            var xKey = _keySelector(x);
            var yKey = _keySelector(y);

            return _keyComparer.Compare(xKey, yKey);
        }
    }
}

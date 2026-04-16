using System;
using System.Collections.Generic;

namespace Scch.Common
{
    public sealed class FunctorComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public FunctorComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }
    }
}

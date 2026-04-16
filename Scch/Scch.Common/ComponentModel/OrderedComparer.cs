using System.Collections.Generic;
using System.Linq;

namespace Scch.Common.ComponentModel
{
    /*
        var comparer = OrderedComparer.Create(
            ProjectionComparer.Create((TreeNode tn) => tn.Text.Substring(0, 1)),
            ProjectionComparer.Create((TreeNode tn) => Convert.ToInt32(tn.Text.Substring(1)))
        );
    */
    public static class OrderedComparer
    {
        public static OrderedComparer<TSource> Create<TSource>(params IComparer<TSource>[] comparers)
        {
            return new OrderedComparer<TSource>(comparers);
        }
    }

    public sealed class OrderedComparer<TSource> : Comparer<TSource>
    {
        private readonly IComparer<TSource>[] _comparers;

        public OrderedComparer(params IComparer<TSource>[] comparers)
        {
            _comparers = comparers.ToArray();
        }

        public override int Compare(TSource x, TSource y)
        {
            var cmp = 0;

            foreach (var comparer in _comparers)
                if ((cmp = comparer.Compare(x, y)) != 0)
                    break;

            return cmp;
        }
    }
}

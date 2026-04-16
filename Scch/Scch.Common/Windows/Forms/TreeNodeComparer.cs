using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Scch.Common.Windows.Forms
{
    public class TreeNodeComparer : Comparer<TreeNode>
    {
        public override int Compare(TreeNode x, TreeNode y)
        {
            if (x != null)
            {
                return (y != null) ? CompareInternal(x, y) : 1;
            }
            else
            {
                return (y != null) ? -1 : 0;
            }
        }

        private int CompareInternal(TreeNode x, TreeNode y)
        {
            long xValue;
            long yValue;

            if (long.TryParse(x.Text, out xValue) && long.TryParse(y.Text, out yValue))
            {
                return xValue.CompareTo(yValue);
            }

            return CultureInfo.CurrentCulture.CompareInfo.Compare(x.Text, y.Text, CompareOptions.None);
        }
    }
}

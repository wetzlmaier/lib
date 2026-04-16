using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace Scch.Common.Windows.Media
{
    public static class BrushHelper
    {
        public static IList<BrushInfo> GetBrushes()
        {
            PropertyInfo[] colorInfo = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);
            return colorInfo.Select(info => new BrushInfo(info.Name, (Brush)info.GetValue(null, null))).ToList();
        }
    }
}

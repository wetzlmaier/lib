using System.Windows.Media;

namespace Scch.Common.Windows.Media
{
    public class BrushInfo
    {
        public BrushInfo(string name, Brush brush)
        {
            Name = name;
            Brush = brush;
        }
        public string Name { get; private set; }
        public Brush Brush { get; private set; }
    }
}

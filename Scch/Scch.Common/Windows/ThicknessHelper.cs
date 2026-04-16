using System.Collections.Generic;
using System.Windows;

namespace Scch.Common.Windows
{
    public static class ThicknessHelper
    {
        public static IList<Thickness> GetThicknesses()
        {
            var thicknesses = new List<Thickness>();
            
            for (double i = 0; i < 5; i+=.5)
            {
                thicknesses.Add(new Thickness(i));
            }
            return thicknesses;
        }
    }
}

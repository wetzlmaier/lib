using System;
using System.Collections.Generic;
using System.Drawing;

namespace Scch.ModelBasedTesting.Tests
{
    internal static class ModelDomain
    {
        public static IList<Tuple<int, KnownColor>> CalledActions { get; }

        static ModelDomain()
        {
            CalledActions = new List<Tuple<int, KnownColor>>();
        }

        [Action]
        public static void DomainAction([Domain("number")] int number, [Domain("color")] KnownColor color)
        {
            CalledActions.Add(new Tuple<int, KnownColor>(number, color));
        }

        [Domain("number")]
        public static int[] NumberDomain()
        {
            return new[] { 1, 3, 5, 7 };
        }

        [Domain("color")]
        public static KnownColor[] ColorDomain => new[] { KnownColor.Red, KnownColor.Green, KnownColor.Blue };

        public static void Reset()
        {
            CalledActions.Clear();
        }
    }
}

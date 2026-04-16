using System;
using System.Text;

namespace Scch.Common
{
    public static class RandomHelper
    {
        private static readonly Random _random;

        static RandomHelper()
        {
            _random = new Random();
        }

        public static bool NextBool()
        {
            return NextInt(2) == 0;
        }

        public static int NextInt(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public static double NextDouble()
        {
            return _random.NextDouble();
        }

        public static int NextInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static string NextSequence(int maxLength)
        {
            int count = NextInt(maxLength) + 1;

            var sequence = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sequence.Append((char)(NextInt(256 - 32) + 32));
            }

            return sequence.ToString();
        }
    }
}

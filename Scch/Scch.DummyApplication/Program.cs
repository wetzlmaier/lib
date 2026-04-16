using System;

namespace Scch.DummyApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Arguments:");
            foreach (var arg in args)
            {
                Console.WriteLine("  " + arg);
            }
        }
    }
}

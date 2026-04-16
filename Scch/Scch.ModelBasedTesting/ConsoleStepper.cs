using System;

namespace Scch.ModelBasedTesting
{
    public abstract class ConsoleStepper
    {
        protected void WriteLine(string text = null, params object[] args)
        {
            Console.WriteLine(text ?? string.Empty, args);
        }
    }
}

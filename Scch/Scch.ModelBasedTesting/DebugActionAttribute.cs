using System;

namespace Scch.ModelBasedTesting
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DebugActionAttribute : ActionAttribute
    {
    }
}
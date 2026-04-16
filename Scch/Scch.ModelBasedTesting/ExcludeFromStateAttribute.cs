using System;

namespace Scch.ModelBasedTesting
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ExcludeFromStateAttribute : Attribute
    {
    }
}

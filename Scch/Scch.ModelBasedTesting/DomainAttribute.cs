using System;

namespace Scch.ModelBasedTesting
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class DomainAttribute : Attribute
    {
        public DomainAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty");

            Name = name;
        }

        public string Name { get; }
    }
}


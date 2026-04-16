using System;
using System.Reflection;

namespace Scch.Common.ComponentModel.Composition
{
    public abstract class NameMetadataBase : INameMetadata
    {
        public string Name { get; }

        protected NameMetadataBase()
        {
            var attribute = (NamedExportAttribute)GetType().GetCustomAttribute(typeof(NamedExportAttribute), true);
            if (attribute == null)
                throw new NotSupportedException($"Missing NamedExportAttribute on type '{GetType().Name}'.");

            Name = attribute.Name;
        }
    }
}

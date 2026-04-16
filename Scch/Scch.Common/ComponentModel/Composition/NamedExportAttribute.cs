using System;
using System.ComponentModel.Composition;

namespace Scch.Common.ComponentModel.Composition
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [MetadataAttribute]
    public class NamedExportAttribute : ExportAttribute, INameMetadata
    {
        public NamedExportAttribute(string name)
        {
            Name = name;
        }

        public NamedExportAttribute(string name, string contractName)
            : base(contractName)
        {
            Name = name;
        }

        public NamedExportAttribute(string name, Type contractType)
            : base(contractType)
        {
            Name = name;
        }

        public NamedExportAttribute(string name, String contractName, Type contractType)
            : base(contractName, contractType)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}

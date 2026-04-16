using System;
using System.ComponentModel;

namespace Scch.Common.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _descriptionResourceName;
        private readonly Type _descriptionResourceType;
        private string _description;

        public LocalizedDescriptionAttribute(Type type, Type descriptionResourceType)
            : this(type.Name + "_Description", descriptionResourceType)
        {

        }

        public LocalizedDescriptionAttribute(Type type, string propertyName, Type descriptionResourceType)
            : this(type.Name + "_" + propertyName + "_Description", descriptionResourceType)
        {

        }

        public LocalizedDescriptionAttribute(string descriptionResourceName, Type descriptionResourceType)
        {
            _descriptionResourceName = descriptionResourceName;
            _descriptionResourceType = descriptionResourceType;
        }

        public override string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                    _description = LocalizedAttributeHelper.Localize(DescriptionResourceName, DescriptionResourceType);

                return _description;
            }
        }

        public string DescriptionResourceName
        {
            get { return _descriptionResourceName; }
        }

        public Type DescriptionResourceType
        {
            get { return _descriptionResourceType; }
        }
    }
}

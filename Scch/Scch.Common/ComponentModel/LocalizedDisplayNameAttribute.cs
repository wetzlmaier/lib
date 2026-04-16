using System;
using System.ComponentModel;

namespace Scch.Common.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _displayNameResourceName;
        private readonly Type _displayNameResourceType;
        private string _displayName;

        public LocalizedDisplayNameAttribute(Type type, Type displayNameResourceType)
            : this(type.Name + "_DisplayName", displayNameResourceType)
        {

        }

        public LocalizedDisplayNameAttribute(Type type, string propertyName, Type displayNameResourceType)
            : this(type.Name + "_" + propertyName + "_DisplayName", displayNameResourceType)
        {

        }

        public LocalizedDisplayNameAttribute(string displayNameResourceName, Type displayNameResourceType)
        {
            _displayNameResourceName = displayNameResourceName;
            _displayNameResourceType = displayNameResourceType;
        }

        public override string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                    _displayName = LocalizedAttributeHelper.Localize(DisplayNameResourceName, DisplayNameResourceType);

                return _displayName;
            }
        }

        public string DisplayNameResourceName
        {
            get { return _displayNameResourceName; }
        }

        public Type DisplayNameResourceType
        {
            get { return _displayNameResourceType; }
        }
    }
}

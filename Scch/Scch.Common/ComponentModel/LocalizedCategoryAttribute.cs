using System;
using System.ComponentModel;

namespace Scch.Common.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        private readonly Type _categoryResourceType;

        public LocalizedCategoryAttribute(string category, Type categoryResourceType)
            : base(category)
        {
            _categoryResourceType = categoryResourceType;
        }

        protected override string GetLocalizedString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return base.GetLocalizedString(value);

            return LocalizedAttributeHelper.Localize(value + "_Category", CategoryResourceType);
        }

        public Type CategoryResourceType
        {
            get { return _categoryResourceType; }
        }
    }
}

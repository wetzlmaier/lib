using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace Scch.Common.Windows.Markup
{
    [MarkupExtensionReturnType(typeof(Type))]
    public class GenericType : MarkupExtension
    {
        private Type _baseType;
        private Type[] _innerTypes;

        public GenericType()
        {
            
        }

        public GenericType(Type baseType, params Type[] innerTypes)
        {
            if (baseType == null)
            {
                throw new ArgumentNullException("baseType");
            }

            if (innerTypes == null)
            {
                throw new ArgumentNullException("innerTypes");
            }

            BaseType = baseType;
            InnerTypes = innerTypes;
        }

        public GenericType(Type baseType, Type innerType) :this(baseType, new[] {innerType})
        {
            if (innerType == null)
            {
                throw new ArgumentNullException("innerType");
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return BaseType.MakeGenericType(InnerTypes);
        }

        [DefaultValue((string)null), ConstructorArgument("baseType")]
        public Type BaseType
        {
            get
            {
                return _baseType;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _baseType = value;
            }
        }

        [DefaultValue((string)null), ConstructorArgument("innerTypes")]
        public Type[] InnerTypes
        {
            get
            {
                return _innerTypes;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _innerTypes = value;
            }
        }


        [DefaultValue((string)null), ConstructorArgument("innerType")]
        public Type InnerType
        {
            get
            {
                if (InnerTypes.Length!=1)
                    throw new InvalidOperationException();

                return InnerTypes[0];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                InnerTypes = new[] { value };
            }
        }
    }
}

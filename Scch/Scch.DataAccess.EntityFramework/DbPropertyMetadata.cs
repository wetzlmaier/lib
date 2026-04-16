using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scch.DataAccess.EntityFramework
{
    internal class DbPropertyMetadata
    {
        internal DbPropertyMetadata(PropertyInfo propertyInfo, DbTypeMetadata type)
        {
            PropertyInfo = propertyInfo;
            Type = type;
            Attributes = propertyInfo.GetCustomAttributes(true).Cast<Attribute>().ToList();
        }

        internal DbTypeMetadata Type { get; private set; }
        internal PropertyInfo PropertyInfo { get; private set; }

        internal IEnumerable<Attribute> Attributes { get; private set; }
    }
}

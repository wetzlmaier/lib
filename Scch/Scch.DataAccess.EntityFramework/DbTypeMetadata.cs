using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scch.DataAccess.EntityFramework
{
    public class DbTypeMetadata
    {
        internal DbTypeMetadata(Type itemType)
        {
            ItemType = itemType;
            var itemProperties = new List<DbPropertyMetadata>();

            var itemTypeProperties = new List<PropertyInfo>(itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy));
            itemTypeProperties.ForEach(one => itemProperties.Add(new DbPropertyMetadata(one, this)));
            Properties = itemProperties;
        }

        internal Type ItemType { get; private set; }

        internal IEnumerable<DbPropertyMetadata> Properties { get; private set; }
    }
}

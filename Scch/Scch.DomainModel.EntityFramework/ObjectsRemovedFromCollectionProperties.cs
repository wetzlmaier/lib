using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scch.DomainModel.EntityFramework
{
    [CollectionDataContract(Name = "ObjectsRemovedFromCollectionProperties",
        ItemName = "DeletedObjectsForProperty", KeyName = "CollectionPropertyName", ValueName = "DeletedObjects")]
    public class ObjectsRemovedFromCollectionProperties : Dictionary<string, ObjectList> { }

}

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scch.DomainModel.EntityFramework
{
    [CollectionDataContract(Name = "ObjectsAddedToCollectionProperties",
        ItemName = "AddedObjectsForProperty", KeyName = "CollectionPropertyName", ValueName = "AddedObjects")]
    public class ObjectsAddedToCollectionProperties : Dictionary<string, ObjectList> { }

}

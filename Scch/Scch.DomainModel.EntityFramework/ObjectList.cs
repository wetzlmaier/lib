using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scch.DomainModel.EntityFramework
{
    [CollectionDataContract(ItemName = "ObjectValue")]
    public class ObjectList : List<object> { }
}

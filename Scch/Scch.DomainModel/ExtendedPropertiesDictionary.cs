using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scch.DomainModel
{
    [CollectionDataContract(Name = "ExtendedPropertiesDictionary",
        ItemName = "ExtendedProperties", KeyName = "Name", ValueName = "ExtendedProperty")]
    public class ExtendedPropertiesDictionary : Dictionary<string, Object> { }

}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scch.DomainModel.EntityFramework
{
    [CollectionDataContract(Name = "OriginalValuesDictionary",
        ItemName = "OriginalValues", KeyName = "Name", ValueName = "OriginalValue")]
    public class OriginalValuesDictionary : Dictionary<string, Object> { }

}

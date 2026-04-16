using System;

namespace Scch.DomainModel.EntityFramework
{
    [Flags]
    public enum ObjectState
    {
        Unchanged = 0x1,
        Added = 0x2,
        Modified = 0x4,
        Deleted = 0x8
    }
}

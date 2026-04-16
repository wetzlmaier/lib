using System;

namespace Scch.DomainModel.EntityFramework
{
    public class ObjectStateChangingEventArgs : EventArgs
    {
        public ObjectState NewState { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Scch.Common.Runtime.Serialization;

namespace Scch.Common.Collections.Generic
{
    [Serializable]
    public abstract class SerializableList<T> : List<T>, ISerializable
    {
        private const int Version = 1;

        protected SerializableList() { }

        protected SerializableList(int capacity) : base(capacity) { }

        protected SerializableList(IEnumerable<T> collection) : base(collection) { }

        protected SerializableList(SerializationInfo info, StreamingContext context)
        {
            SerializationHelper.Deserialize(this, info, context);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationHelper.Serialize(this, Version, info, context);
        }
    }
}

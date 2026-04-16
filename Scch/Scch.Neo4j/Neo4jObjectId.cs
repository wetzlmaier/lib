using System;
using Newtonsoft.Json.Bson;

namespace Scch.Neo4j
{
    public class Neo4jObjectId : IComparable<Neo4jObjectId>, IEquatable<Neo4jObjectId>
    {
        public Neo4jObjectId(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length != 12)
            {
                throw new ArgumentException("An ObjectId must be 12 bytes", nameof(value));
            }

            Value = value;
        }

        public byte[] Value { get; }

        public int CompareTo(Neo4jObjectId other)
        {
            if (Value == null)
                return -1;

            if (other.Value == null)
                return 1;

            if (Value.Length != other.Value.Length)
                return Value.Length.CompareTo(other.Value.Length);

            for (int i = 0; i < Math.Min(Value.Length, other.Value.Length); i++)
            {
                int compare = Value[i].CompareTo(other.Value[i]);

                if (compare != 0)
                    return compare;
            }

            return 0;
        }

        public bool Equals(Neo4jObjectId other)
        {
            return CompareTo(other) == 0;
        }
    }
}

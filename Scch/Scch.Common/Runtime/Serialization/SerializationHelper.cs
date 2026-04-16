using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Scch.Common.Runtime.Serialization
{
    public static class SerializationHelper
    {
        public static void Deserialize<T>(IList<T> list, SerializationInfo info, StreamingContext context)
        {
            int version = info.GetInt32("Version");

            switch (version)
            {
                case 1:
                    int count = info.GetInt32("Count");
                    for (int item = 0; item < count; item++)
                        list.Add((T)info.GetValue("Item" + item, typeof(T)));
                    break;

                default:
                    throw new NotSupportedException(string.Format("Version {0} is not supported.", version));
            }
        }

        public static void Serialize<T>(IList<T> list, int version, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Version", version);
            info.AddValue("Count", list.Count);

            for (int item = 0; item < list.Count; item++)
                info.AddValue("Item" + item, list[item]);

        }
    }
}

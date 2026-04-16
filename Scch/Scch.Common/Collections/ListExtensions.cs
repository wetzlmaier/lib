using System.Collections;

namespace Scch.Common.Collections
{
    public static class ListExtensions
    {
        public static void Move(this IList list, int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
                return;
            /*
            var temp = list[oldIndex];
            if (oldIndex < newIndex)
            {
                for (int i = oldIndex; i < newIndex; i++)
                    list[i] = list[i + 1];
            }
            else
            {
                for (int i = oldIndex; i > newIndex; i--)
                    list[i] = list[i - 1];
            }
            list[newIndex] = temp;*/

            var item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }
    }
}

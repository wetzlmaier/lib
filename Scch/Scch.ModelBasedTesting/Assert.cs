using System.Collections.Generic;
using Scch.ModelBasedTesting.Osmo;

namespace Scch.ModelBasedTesting
{
    public static class Assert
    {
        public static void IsTrue(bool condition, string message = "AssertionException")
        {
            if (!condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void IsFalse(bool condition, string message = "AssertionException")
        {
            if (condition)
            {
                throw new AssertionException(message);
            }
        }

        public static void AreEqual<T>(ICollection<T> expectedItems, ICollection<T> actualItems, string message = null)
        {
            AreEqual(expectedItems.Count, actualItems.Count);

            foreach (var item in expectedItems)
            {
                IsTrue(actualItems.Contains(item), $"Expected item '{item}' is not contained in actualItems");
            }
        }

        public static void AreEqual<T>(T left, T right, string message = null)
        {
            IsTrue(Equals(left, right), message);
        }

        public static void IsNotNull(object value)
        {
            IsFalse(value == null);
        }

        public static void IsNull(object value)
        {
            IsTrue(value == null);
        }
    }
}

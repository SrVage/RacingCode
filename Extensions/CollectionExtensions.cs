using UnityEngine;

namespace Code.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddVector(this Vector2[] collection, Vector2 newValue)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (i < collection.Length - 1)
                    collection[i] = collection[i + 1];
                else
                    collection[i] = newValue;
            }
        }

        public static Vector2 SmoothVector(this Vector2[] collection)
        {
            Vector2 smoothVector = Vector2.zero;
            for (int i = 0; i < collection.Length; i++)
            {
                smoothVector += collection[i];
            }
            smoothVector /= collection.Length;
            return smoothVector;
        }
    }
}
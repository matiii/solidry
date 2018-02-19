using System.Collections.Generic;

namespace Solidry.Extensions
{
    public static class Iterator
    {
        public static bool IsEmpty<T>(this IReadOnlyList<T> collection)
        {
            return collection.Count == 0;
        }
    }
}
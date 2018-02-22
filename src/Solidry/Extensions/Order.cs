using System;
using System.Collections.Generic;
using Solidry.Results;

namespace Solidry.Extensions
{
    public static class Order
    {
        public static T First<T>(this IReadOnlyList<T> collection)
        {
            if (collection.IsEmpty())
            {
                throw new ArgumentOutOfRangeException("Collection has 0 elements.");
            }

            return collection[0];
        }

        public static Option<T> TryFirst<T>(this IReadOnlyList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return Option<T>.Empty;
            }

            return Option<T>.Create(collection[0]);
        }

        public static T Second<T>(this IReadOnlyList<T> collection)
        {
            if (collection.Count < 2)
            {
                throw new ArgumentOutOfRangeException("Collection has less than 2 elements.");
            }

            return collection[1];
        }

        public static Option<T> TrySecond<T>(this IReadOnlyList<T> collection)
        {
            if (collection.Count < 2)
            {
                return Option<T>.Empty;
            }

            return Option<T>.Create(collection[1]);
        }

        public static T BeforeLast<T>(this IReadOnlyList<T> collection)
        {
            if (collection.Count < 2)
            {
                throw new ArgumentOutOfRangeException("Collection has less than 2 elements.");
            }

            return collection[collection.Count - 2];
        }

        public static Option<T> TryBeforeLast<T>(this IReadOnlyList<T> collection)
        {
            if (collection.Count < 2)
            {
                return Option<T>.Empty;
            }

            return Option<T>.Create(collection[collection.Count - 2]);
        }

        public static T Last<T>(this IReadOnlyList<T> collection)
        {
            if (collection.IsEmpty())
            {
                throw new ArgumentOutOfRangeException("Collection has 0 elements.");
            }
            
            return collection[collection.Count - 1];
        }

        public static Option<T> TryLast<T>(this IReadOnlyList<T> collection)
        {
            if (collection.IsEmpty())
            {
                return Option<T>.Empty;
            }

            return Option<T>.Create(collection[collection.Count - 1]);
        }
    }
}
using System;
using System.Collections.Generic;
using Solidry.Results.Iterator;

namespace Solidry.Extensions
{
    public static class Iterator
    {
        /// <summary>
        /// Check if collection is empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IReadOnlyList<T> collection)
        {
            return collection.Count == 0;
        }

        /// <summary>
        /// Performs the specified action on each element of collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IReadOnlyList<T> collection, Action<T> action)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                action(collection[i]);
            }
        }

        /// <summary>
        /// Partition collection doubly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static PartitionResult<T> Partition<T>(this IReadOnlyList<T> collection, Func<int, T, bool> predicate)
        {
            var @true = new List<T>(collection.Count / 2);
            var @false = new List<T>(collection.Count / 2);

            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(i, collection[i]))
                {
                    @true.Add(collection[i]);
                }
                else
                {
                    @false.Add(collection[i]);
                }
            }

            return new PartitionResult<T>(@true, @false);
        }
    }
}
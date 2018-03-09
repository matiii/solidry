using System;
using System.Collections.Generic;
using System.Linq;
using Solidry.Helpers;

namespace Solidry.Extensions
{
    public static class Iterator
    {
        /// <summary>
        /// Check if collection is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IReadOnlyList<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Performs the specified action on each element of collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action">(item)</param>
        public static void Each<T>(this IReadOnlyList<T> collection, Action<T> action)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                action(collection[i]);
            }
        }

        /// <summary>
        /// Performs the specified action on each element of collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">source</param>
        /// <param name="action">(index, item)</param>
        public static void Each<T>(this IReadOnlyList<T> collection, Action<int,T> action)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                action(i,collection[i]);
            }
        }

        /// <summary>
        /// Concatenate collection of arrays into one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T[] ToOne<T>(this IReadOnlyList<T[]> collection)
        {
            var result = new T[collection.Sum(a => a.Length)];

            int offset = 0;

            for (int x = 0; x < collection.Count; x++)
            {
                collection[x].CopyTo(result, offset);
                offset += collection[x].Length;
            }

            return result;
        }

        /// <summary>
        /// Swap elements in array from end to begining until predicate is false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="predicate"></param>
        public static void SwapRightUntil<T>(this T[] array, Func<T, T, bool> predicate)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                if (predicate(array[i-1], array[i]))
                {
                    UtilsHelper.Swap(ref array[i-1], ref array[i]);
                }
                else
                {
                    break;
                }
            }
        }
    }
}   
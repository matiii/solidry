using System;
using System.Collections.Generic;
using Solidry.Helpers;
using Solidry.Results.Partitioning;

namespace Solidry.Extensions
{
    /// <summary>
    /// Partition collection extensions.
    /// </summary>
    public static class Partitioning
    {
        /// <summary>
        /// Partitioning array for minimal and rest elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Source</param>
        /// <param name="capacity">Number of minimal elements</param>
        /// <returns></returns>
        public static MinResult<T> Min<T>(this T[] array, int capacity = 1) where T : IComparable<T>
        {
            if (capacity < 1)
            {
                throw new InvalidOperationException($"Argument {nameof(capacity)} has to be greater than 0.");
            }

            if (capacity > array.Length)
            {
                throw new InvalidOperationException($"Argument {nameof(capacity)} has to be equal or less than collection.");
            }

            var min = new T[capacity];
            var rest = new T[array.Length - capacity];

            Array.Copy(array,min,capacity);
            Array.Copy(array,capacity, rest, 0, array.Length-capacity);
            Array.Sort(min);

            int lastIndex = capacity - 1;

            for (int i = 0; i < rest.Length; i++)
            {
                T e = rest[i];

                int c = e.CompareTo(min[lastIndex]);

                if (c < 0)
                {
                    UtilsHelper.Swap(ref rest[i], ref min[lastIndex]);
                        
                    min.SwapRightUntil((left, right) => left.CompareTo(right) > -1);
                }
            }

            return new MinResult<T>(min, rest);
        }

        /// <summary>
        /// Partitioning array for maximal and rest elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="capacity">Number of maximal elements</param>
        /// <returns></returns>
        public static MaxResult<T> Max<T>(this T[] array, int capacity = 1) where T: IComparable<T>
        {
            if (capacity < 1)
            {
                throw new InvalidOperationException($"Argument {nameof(capacity)} has to be greater than 0.");
            }

            if (capacity > array.Length)
            {
                throw new InvalidOperationException($"Argument {nameof(capacity)} has to be equal or less than collection.");
            }

            var max = new T[capacity];
            var rest = new T[array.Length - capacity];

            Array.Copy(array, max, capacity);
            Array.Copy(array, capacity, rest, 0, array.Length - capacity);
            Array.Sort(max, Comparer<T>.Create((x, y) => -x.CompareTo(y)));

            int lastIndex = capacity - 1;

            for (int i = 0; i < rest.Length; i++)
            {
                T e = rest[i];

                int c = e.CompareTo(max[lastIndex]);

                if (c > 0)
                {
                    UtilsHelper.Swap(ref rest[i], ref max[lastIndex]);

                    max.SwapRightUntil((left, right) => left.CompareTo(right) < 1);
                }
            }

            return new MaxResult<T>(max, rest);
        }

        /// <summary>
        /// Partitioning collection by predicate.
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
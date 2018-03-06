using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Solidry.Helpers;
using Solidry.Results.Analytics;

namespace Solidry.Extensions
{
    public static class Analytics
    {

        /// <summary>
        /// Partition array for minimal and rest elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Source</param>
        /// <param name="capacity">Number of minimal elements</param>
        /// <returns></returns>
        public static MinResult<T> Min<T>(this T[] array, int capacity = 1) where T : class, IComparable<T>
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

        //TODO: memory copy
        public static MinResult<T> _Min<T>(this T[] array, int capacity = 1) where T : struct, IComparable<T> 
        {
//            array._Min()
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

            Array.Copy(array, min, capacity);
            Array.Copy(array, capacity, rest, 0, array.Length - capacity);
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


        //TODO: add Max<T>

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
using System;
using System.Collections.Generic;
using Solidry.Helpers;
using Solidry.Results.Analytics;

namespace Solidry.Extensions
{
    public static class Analytics
    {
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
                    Utils.Swap(ref rest[i], ref min[lastIndex]);

                    Array.Sort(min);
                }
            }

            return new MinResult<T>(min, rest);
        }
    }
}
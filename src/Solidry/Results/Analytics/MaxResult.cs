using System;

namespace Solidry.Results.Analytics
{
    public struct MaxResult<T> : IEquatable<MaxResult<T>> where T : IComparable<T>
    {
        public MaxResult(T[] max, T[] rest)
        {
            Max = max;
            Rest = rest;
        }

        public T[] Max { get; }

        public T[] Rest { get; }

        public bool Equals(MaxResult<T> other)
        {
            return Equals(Max, other.Max) && Equals(Rest, other.Rest);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MaxResult<T> && Equals((MaxResult<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Max != null ? Max.GetHashCode() : 0) * 397) ^ (Rest != null ? Rest.GetHashCode() : 0);
            }
        }
    }
}
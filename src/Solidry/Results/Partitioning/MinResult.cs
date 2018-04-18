using System;

namespace Solidry.Results.Partitioning
{
    public struct MinResult<T>: IEquatable<MinResult<T>> where T : IComparable<T>
    {
        public MinResult(T[] min, T[] rest)
        {
            Min = min;
            Rest = rest;
        }

        public T[] Min { get; }

        public T[] Rest { get; }

        public bool Equals(MinResult<T> other)
        {
            return Equals(Min, other.Min) && Equals(Rest, other.Rest);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MinResult<T> && Equals((MinResult<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Min != null ? Min.GetHashCode() : 0) * 397) ^ (Rest != null ? Rest.GetHashCode() : 0);
            }
        }
    }
}
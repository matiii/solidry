using System;
using System.Collections.Generic;

namespace Solidry.Results.Analytics
{
    public struct PartitionResult<T>: IEquatable<PartitionResult<T>>
    {
        public PartitionResult(List<T> @true, List<T> @false)
        {
            True = @true;
            False = @false;
        }

        public List<T> True { get; }

        public List<T> False { get; }

        public bool Equals(PartitionResult<T> other)
        {
            return Equals(True, other.True) && Equals(False, other.False);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is PartitionResult<T> && Equals((PartitionResult<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((True != null ? True.GetHashCode() : 0) * 397) ^ (False != null ? False.GetHashCode() : 0);
            }
        }
    }
}
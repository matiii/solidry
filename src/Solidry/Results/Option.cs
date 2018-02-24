using System;
using System.Collections.Generic;

namespace Solidry.Results
{
    /// <inheritdoc cref="IEquatable{TOption}" />
    /// <summary>
    /// Instead return null, return option.
    /// </summary>
    /// <typeparam name="T">Payload</typeparam>
    public struct Option<T>: IEquatable<Option<T>>
    {
        public T Value { get; } 

        public bool HasValue { get; }

        public Option(T value)
        {
            HasValue = true;
            Value = value;
        }

        public static Option<T> Create(T value) => new Option<T>(value);

        public static Option<T> Empty { get; } = new Option<T>();

        public bool Equals(Option<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value) && HasValue == other.HasValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Option<T> && Equals((Option<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Value) * 397) ^ HasValue.GetHashCode();
            }
        }
    }
}
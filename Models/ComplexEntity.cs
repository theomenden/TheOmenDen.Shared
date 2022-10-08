namespace TheOmenDen.Shared.Models;

/// <summary>
/// A sample entity, that can be used to define a <paramref name="Key"/> that can be complex and type safe, associated with a <paramref name="Value"/>
///  <inheritdoc cref="IComparable{T}"/>
/// </summary>
/// <typeparam name="TKey">The type of key we want associated</typeparam>
/// <typeparam name="TValue">The value we need to work with on a key</typeparam>
/// <param name="Key">The provided key</param>
/// <param name="Value">The provided value</param>
public abstract record ComplexEntity<TKey, TValue>(IEntityKey<TKey> Key, TValue Value) : IEntity<TKey, TValue>
, IComparable<ComplexEntity<TKey, TValue>>
where TKey : IComparable<TKey>
{
    public virtual int CompareTo(ComplexEntity<TKey, TValue>? other) =>
    other?.Key.Id.CompareTo(Key.Id) ?? 1;

    public virtual bool Equals(ComplexEntity<TKey, TValue>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Key.Equals(other.Key) 
               && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode() => HashCode.Combine(Key, Value);
}
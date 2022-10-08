namespace TheOmenDen.Shared.Models;

/// <summary>
/// A common marker interface
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Provides a unique identity containing various information regarding the underlying entity
    /// </summary>
    IEntityKey Key { get; }
}

/// <summary>
/// Generic wrapper for <see cref="IEntity"/>, 
/// <inheritdoc cref="IEntity"/>
/// </summary>
/// <typeparam name="T">The underlying type data - <c>Covariant</c></typeparam>
public interface IEntity<out T> : IEntity
{
    /// <summary>
    /// The data present in the entity
    /// </summary>
    /// <value>
    /// An object representation
    /// </value>
    T Data { get; }
}

/// <summary>
/// Allows for the definition of a more complex entity that depends on a composite type of key
/// </summary>
/// <typeparam name="TKey">The underlying type for the key</typeparam>
/// <typeparam name="TValue">The underlying type</typeparam>
public interface IEntity<out TKey,out TValue>
where TKey : IComparable<TKey>
{
    IEntityKey<TKey> Key { get; }

    TValue Value { get; }
}
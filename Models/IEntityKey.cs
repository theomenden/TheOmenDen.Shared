namespace TheOmenDen.Shared.Models;

/// <summary>
/// A set of constraints for a given Entity
/// </summary>
public interface IEntityKey
{
    /// <summary>
    /// The entity's unique Id
    /// </summary>
    /// <value>
    /// A unique Id
    /// </value>
    Guid Id { get; }

    /// <summary>
    /// The originating date of the entity
    /// </summary>
    /// <value>
    /// A timestamp for when the entity was first initialized/created
    /// </value>
    DateTime CreatedAt { get; }

    /// <summary>
    /// The location where the entity originated - The "Where"
    /// </summary>
    /// <value>
    /// A provided tenant that we can subscribe to
    /// </value>
    public ITenant Tenant { get; }

    /// <summary>
    /// The originator/creator of the entity - The "Who" 
    /// </summary>
    /// <value>
    /// A record of the creator of this entity
    /// </value>
    public IUser Creator { get; }
}

/// <summary>
/// Wrapper for <see cref="IEntityKey"/>, providing the ability to use a strongly typed composite key that implements <see cref="IComparable{T}"/>
/// </summary>
/// <typeparam name="TKey">The underlying composite key type - must implement <see cref="IComparable{T}"/></typeparam>
public interface IEntityKey<out TKey> : IEntityKey
where TKey : IComparable<TKey>
{
    /// <summary>
    /// The composite value
    /// </summary>
    /// <value>A comparable key to work with</value>
    TKey Composite { get; }
}
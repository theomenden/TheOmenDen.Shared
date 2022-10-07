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

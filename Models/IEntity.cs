namespace TheOmenDen.Shared.Models;

/// <summary>
/// A common marker interface
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Provides a unique identity containing various informatino regarding the underlying entity
    /// </summary>
    EntityKey Key { get; }
}

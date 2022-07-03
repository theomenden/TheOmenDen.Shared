namespace TheOmenDen.Shared.Models;

/// <summary>
/// A common marker interface
/// </summary>
public interface IEntity
{
    /// <summary>
    /// A unique ID for each implemented
    /// </summary>
    public Guid Id { get; }
}

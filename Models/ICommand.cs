namespace TheOmenDen.Shared.Models;

/// <summary>
/// A maker interface for Commands in a CQRS pattern, with basic support for simple event sourcing
/// </summary>
public interface ICommand: IEntity
{
    public Event RaiseEvent();
}

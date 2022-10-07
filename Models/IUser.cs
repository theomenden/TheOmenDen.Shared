namespace TheOmenDen.Shared.Models;

/// <summary>
/// Provides basic information about a particular user
/// </summary>
public interface IUser
{
    /// <summary>
    /// The user's unique Id
    /// </summary>
    Guid Id { get; }
    /// <summary>
    /// The user's provided email
    /// </summary>
    String Email { get; }
    /// <summary>
    /// The user's name
    /// </summary>
    String Name { get; }
    /// <summary>
    /// Checking if the user is authenticated where relevant
    /// </summary>
    Boolean IsAuthenticated { get; }
    /// <summary>
    /// A unique integer key for the user's provided information
    /// </summary>
    Int32 Key { get; }
}


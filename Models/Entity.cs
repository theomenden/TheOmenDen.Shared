namespace TheOmenDen.Shared.Models;

/// <summary>
/// A base record to create other Entities.
/// </summary>
/// <typeparam name="T">The underlying type that our data will shape from</typeparam>
/// <param name="Key">Container for relevant creation information</param>
/// <param name="Data">The information we're working with</param>
public abstract record Entity<T>(IEntityKey Key, T Data) : IEntity<T>;

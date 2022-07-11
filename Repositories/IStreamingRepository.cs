namespace TheOmenDen.Shared.Repositories;

/// <summary>
/// <inheritdoc cref="IAsyncEnumerable{T}"/>
/// </summary>
/// <typeparam name="T">The type that we're exposing</typeparam>
public interface IStreamingRepository<out T> : IAsyncEnumerable<T>
{
}

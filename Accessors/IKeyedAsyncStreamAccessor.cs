namespace TheOmenDen.Shared.Accessors;

/// <summary>
/// Defines asynchronous streaming methods to allow retrival of entities matching a supplied key
/// </summary>
/// <typeparam name="TResult">The result of the operation</typeparam>
/// <typeparam name="TEntity">The entity returned</typeparam>
public interface IKeyedAsyncStreamAccessor<TResult, TEntity>
{
    /// <summary>
    /// A relatively stable retriveal operation meant to provide the caller with a per-entity result based on the provided <paramref name="key"/>
    /// </summary>
    /// <typeparam name="TKey">The type of key we will be using</typeparam>
    /// <param name="key">The provided key to search under</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>An asynchronous stream of the coupling between (<typeparamref name="TResult"/> and <typeparamref name="TEntity"/>) allowing for further processing and error handling per response</returns>
    IAsyncEnumerable<(TResult, TEntity)> GetAllAsyncStream<TKey>(TKey key, CancellationToken cancellationToken = new())
        where TKey : IEntityKey;

    /// <summary>
    /// A relatively stable retriveal operation meant to provide the caller with a per-entity result based on the provided list of <paramref name="keys"/>
    /// </summary>
    /// <typeparam name="TKey">The type of key we will be using</typeparam>
    /// <param name="key">The provided key to search under</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>An asynchronous stream of the coupling between (<typeparamref name="TResult"/> and <typeparamref name="TEntity"/>) allowing for further processing and error handling per response</returns>

    IAsyncEnumerable<(TResult, TEntity)> GetAllThatMatchKeysAsyncStream<TKey>(IEnumerable<TKey> keys, CancellationToken cancellationToken = new())
        where TKey : IEntityKey;
}

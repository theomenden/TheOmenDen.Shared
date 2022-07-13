namespace TheOmenDen.Shared.Accessors;
/// <summary>
/// Defines a set of methods for retrieving an entity by a provided key
/// </summary>
/// <typeparam name="TKey">The key to search with</typeparam>
/// <typeparam name="TResult">The operation result</typeparam>
/// <typeparam name="TEntity">The entity we want to return</typeparam>
public interface IKeyedAccessor<in TKey,TResult, TEntity> where TKey : IEntityKey
{
    /// <summary>
    /// Returns a single <typeparamref name="TEntity"/> based off of the provided <typeparamref name="TKey"/> <paramref name="key"/>
    /// </summary>
    /// <param name="key">The key we're using</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>A coupling of the <typeparamref name="TResult"/> and <typeparamref name="TKey"/></returns>
    ValueTask<(TResult, TEntity)> GetByKeyAsync(TKey key, CancellationToken cancellationToken = new());

    /// <summary>
    /// Returns a collection of <typeparamref name="TEntity"/>s that match the specified properties given by the <typeparamref name="TKey"/>  <paramref name="key"/>
    /// </summary>
    /// <param name="key">The key we're using</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>A coupling of the <typeparamref name="TResult"/> and <typeparamref name="TKey"/></returns>
    ValueTask<(TResult, IEnumerable<TEntity>)> GetAllAsync(TKey key, CancellationToken cancellationToken = new());

    /// <summary>
    /// Returns a collection of <typeparamref name="TEntity"/>s that match the specified properties given by the provided set of <typeparamref name="TKey"/>  <paramref name="key"/>s
    /// </summary>
    /// <param name="key">The key we're using</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns>A coupling of the <typeparamref name="TResult"/> and <typeparamref name="TKey"/></returns>
    ValueTask<(TResult, IEnumerable<TEntity>)> GetAllThatMatchKeysAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = new());
}
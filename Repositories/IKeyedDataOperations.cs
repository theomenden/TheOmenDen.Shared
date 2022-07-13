namespace TheOmenDen.Shared.Repositories;

/// <summary>
/// <para>This interface aims to define the basic Create, Update, and Delete functionalities for entities within the domain</para>
/// <para> By manipulating our results from a given store, a return <see cref="ValueTuple{T1, T2}"/> is possible to reflect the changes that were made</para>
/// <para>This allows for further processing and error handling based on the results, without needing to throw exceptions</para>    
/// <inheritdoc cref="IEnumerable{T}"/>
/// <inheritdoc cref="IAsyncEnumerable{T}"/>
/// </summary>
/// <typeparam name="TKey">The key object that we are looking for, must implement <see cref="IEntityKey"/></typeparam>
/// <typeparam name="TEntity">The value we aim to manipulate, must implement <see cref="IEntity"/></typeparam>
/// <typeparam name="TResult">The result object we aim to have returned to the caller - for example, a <see cref="Boolean"/> to indicate a success/fail</typeparam>
/// <remarks>Not meant to be compatible with <see cref="IDataOperations{T, TResult}"/></remarks>
public interface IKeyedDataOperations<in TKey, in TValue, TEntity, TResult>: IEnumerable<(TEntity,TResult)>, IAsyncEnumerable<(TEntity,TResult)>
  where TKey : IEntityKey
{
    /// <summary>
    /// An attempt at a relatively stable insertion operation that respects the given <paramref name="key"/>, and allows for underlying <paramref name="manipulativeValues"/> to be distinguished from the originating key.
    /// </summary>
    /// <param name="key">The key object that we are aiming to check against</param>
    /// <param name="manipulativeValues">The values we're aiming to provide with the key</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><see cref="Tuple{T1, T2}"/>: A coupling of the result of the operation <typeparamref name="TResult"/>, and the completed object,<typeparamref name="TEntity"/></returns>
    ValueTask<(TResult, TEntity)> AddAsync(TKey key, TValue manipulativeValues, CancellationToken cancellationToken = new());

    /// <summary>
    /// An attempt at a relatively stable update operation that respects the given <paramref name="key"/>, and allows for underlying <paramref name="manipulativeValues"/> to be distinguished from the originating key.
    /// </summary>
    /// <param name="key">The key object that we are aiming to check against</param>
    /// <param name="manipulativeValues">The values we're aiming to provide with the key</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><see cref="Tuple{T1, T2}"/>: A coupling of the result of the operation <typeparamref name="TResult"/>, and the completed object,<typeparamref name="TEntity"/></returns>
    ValueTask<(TResult, TEntity)> UpdateAsync(TKey key, TValue manipulativeValues, CancellationToken cancellationToken = new());

    /// <summary>
    /// An attempt at a relatively stable update operation that respects the provided <paramref name="keys"/> and allows for logic to take place with the underlying <paramref name="manipulativeValues"/> to be distinguished from the originating keys.
    /// </summary>
    /// <param name="keys">THe key objects we are aiming to remove</param>
    /// <param name="manipulativeValues">The values we'd like to update in the objects that are referenced by the provided <paramref name="keys"/></param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><see cref="IAsyncEnumerable{T}"/>: <see cref="Tuple{T1, T2}"/>: A streaming coupling of the result of the operation <typeparamref name="TResult"/>, and the completed object,<typeparamref name="TEntity"/></returns>
    IAsyncEnumerable<(TResult, TEntity)> UpdateAsyncStream(IEnumerable<TKey> keys, IEnumerable<TValue> manipulativeValues, CancellationToken cancellationToken = new());

    /// <summary>
    /// An attempt at a relatively stable deletion operation that respects the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key object that we are aiming to check against</param>
    /// <param name="manipulativeValues">The values we're aiming to provide with the key</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><see cref="Tuple{T1, T2}"/>: A coupling of the result of the operation <typeparamref name="TResult"/>, and the completed object,<typeparamref name="TEntity"/></returns>
    ValueTask<(TResult, TEntity)> DeleteAsync(TKey key, CancellationToken cancellationToken = new());

    /// <summary>
    /// An attempt at a relatively stable deletion operation that aims to eliminate entities that match the provided <paramref name="keys"/>, and still allow individual error/logical handlings on the calling side
    /// </summary>
    /// <param name="keys">THe key objects we are aiming to remove</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><see cref="IAsyncEnumerable{T}"/>: <see cref="Tuple{T1, T2}"/>: A streaming coupling of the result of the operation <typeparamref name="TResult"/>, and the completed object,<typeparamref name="TEntity"/></returns>
    IAsyncEnumerable<(TResult, TEntity)> DeleteAsyncStream(IEnumerable<TKey> keys, CancellationToken cancellationToken = new());
}
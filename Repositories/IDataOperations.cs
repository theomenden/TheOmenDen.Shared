namespace TheOmenDen.Shared.Repositories;
/// <summary>
/// <para>Defines a set of Add, Update, and delete methods for the type <typeparamref name="T"/></para>
/// <inheritdoc cref="IRepository{T}"/>
/// <inheritdoc cref="IStreamingRepository{T}"/>
/// </summary>
/// <typeparam name="T">The underlying type affected</typeparam>
/// <typeparam name="TResult">The result of the operation</typeparam>
public interface IDataOperations<T, TResult> : IRepository<T>, IStreamingRepository<T>
    where T : class
{
    /// <summary>
    /// Adds a given <paramref name="entity"/> of <typeparamref name="T"/> to an established store
    /// </summary>
    /// <param name="entity">The <typeparamref name="T" /> entity we are trying to add to the respective table</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <typeparam name="T">Type of entity to add</typeparam>
    /// <returns><typeparamref name="TResult"/></returns>
    ValueTask<TResult> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds the specified <paramref name="entities"/> of type <typeparamref name="T"/> to an established store 
    /// </summary>
    /// <param name="entities">The entities that we want to add</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><typeparamref name="TResult"/></returns>
    ValueTask<TResult> AddManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the provided <paramref name="entity"/> of type <typeparamref name="T"/> in an established store
    /// </summary>
    /// <param name="entity">The entity we want to update</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><typeparamref name="TResult"/></returns>
    ValueTask<TResult> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the provided <paramref name="entities"/> of type <typeparamref name="T"/> in an established store
    /// </summary>
    /// <param name="entities">The entities we want to update</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><typeparamref name="TResult"/></returns>
    ValueTask<TResult> UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the provided <paramref name="entity"/> from an established store
    /// </summary>
    /// <param name="entity">The entity we want to delete</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><typeparamref name="TResult"/></returns>
    ValueTask<TResult> DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the provided <paramref name="entities"/> from an established store
    /// </summary>
    /// <param name="entities">The entities we want to delete</param>
    /// <param name="cancellationToken"><inheritdoc cref="CancellationToken"/></param>
    /// <returns><typeparamref name="TResult"/></returns>
    ValueTask<TResult> DeleteManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
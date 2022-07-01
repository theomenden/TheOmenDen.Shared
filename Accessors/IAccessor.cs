using System.Linq.Expressions;

namespace TheOmenDen.Shared.Accessors;

/// <summary>
/// Defines methods for retrieving entities
/// </summary>
/// <typeparam name="T">The type of entity we are returning</typeparam>
/// <remarks>Only defines READ methods</remarks>
public interface IAccessor<T>
{
    /// <summary>
    /// Returns all <typeparamref name="T"/> entities from the source
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>An Enumerable of the entities</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = new());

    /// <summary>
    /// Returns all <typeparamref name="T"/> entities from the source that match the given <paramref name="predicate"/> 
    /// </summary>
    /// <param name="predicate">The supplied condition to match against, compiling down to a <see cref="Expression{Delegate}"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns>An Enumerable of the matching Entities</returns>
    Task<IEnumerable<T>> GetAllThatMatchAsync(Expression<Func<T, Boolean>> predicate, CancellationToken cancellationToken = new()); 

    /// <summary>
    /// Returns a single entity of Type <typeparamref name="T"/> from the source
    /// </summary>
    /// <param name="entityId">The supplied Id for lookup</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A <see cref="Task{T}"/> of <typeparamref name="T"/></returns>
    Task<T> WithIdAsync(Guid entityId, CancellationToken cancellationToken = new());
}
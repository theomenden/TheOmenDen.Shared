using System.Linq.Expressions;


namespace TheOmenDen.Shared.Accessors;

/// <summary>
/// Defines methods for retrieving entities
/// </summary>
/// <typeparam name="TResponse">The type of entity we are returning</typeparam>
/// <remarks>Only defines READ methods</remarks>
public interface IAccessor<TResponse>
{
    /// <summary>
    /// Returns all <typeparamref name="TResponse"/> entities from the source
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>An Enumerable of the entities</returns>
    ValueTask<IEnumerable<TResponse>> GetAllAsync(CancellationToken cancellationToken = new());

    /// <summary>
    /// Returns all <typeparamref name="TResponse"/> entities from the source that match the given <paramref name="predicate"/> 
    /// </summary>
    /// <param name="predicate">The supplied condition to match against, compiling down to a <see cref="Expression{Delegate}"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="ValueTask{TResponse}"/> : An Enumerable of the matching Entities</returns>
    ValueTask<IEnumerable<TResponse>> GetAllThatMatchAsync(Expression<Func<TResponse, Boolean>> predicate, CancellationToken cancellationToken = new());
}
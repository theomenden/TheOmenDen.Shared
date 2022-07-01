using System.Linq.Expressions;

namespace TheOmenDen.Shared.Accessors;
/// <summary>
/// Defines methods for retrieving entities of  Type <typeparamref name="T"/> as a stream of data
/// </summary>
/// <typeparam name="T">The underlying type to return</typeparam>
/// <remarks>Only defines READ methods</remarks>
public interface IAsyncStreamAccessor<T>
{
    /// <summary>
    /// Retrieves all entities of type <typeparam name="T"></typeparam> from their respective tables in the database
    /// </summary>    
    /// <param name="dbContext">The Caller Supplied DbContext</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An asynchronous stream of the entities (<see cref="IAsyncEnumerable{T}"/>) of type <typeparamref name="T"/></returns>
    IAsyncEnumerable<T> GetAllAsync(CancellationToken cancellationToken = new());

    /// <summary>
    /// Retrieves all entities of type <typeparam name="T"></typeparam> that match the given <param name="predicate"></param>
    /// </summary>
    /// <param name="dbContext">The Caller supplied DbContext</param>
    /// <param name="predicate">A list of conditions to match against</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An asynchronous stream of the entities (<see cref="IAsyncEnumerable{T}"/>) of type <typeparamref name="T"/></returns>
    IAsyncEnumerable<T> GetAllThatMatchAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}

using System.Collections.Concurrent;

namespace TheOmenDen.Shared.Extensions;
/// <summary>
/// Creates a way to manage a pool of <see cref="StringBuilder"/>s to avoid creating them every time we need one
/// </summary>
/// <remarks>Should be able to register this as a singleton in the AddServices methods</remarks>
public class StringBuilderPool
{
    private readonly ConcurrentBag<StringBuilder> _builders = new();

    /// <summary>
    /// Retrieves a <see cref="StringBuilder"/> from the underlying pool.
    /// </summary>
    /// <returns>A <see cref="StringBuilder"/> from the pool, if one fails to exist, we create a new instance</returns>
    /// <remarks>The underlying pool is a <see cref="ConcurrentBag{T}"/></remarks>0
    public StringBuilder GetStringBuilderFromPool => _builders.TryTake(out var sb) ? sb : new();

    /// <summary>
    /// Returns the cleared <see cref="StringBuilder"/> instance to the pool
    /// </summary>
    /// <param name="sb">The <see cref="StringBuilder"/> instance we want to return to the pool</param>
    public void ReturnStringBuilderToPool(StringBuilder sb)
    {
        sb.Clear();
        _builders.Add(sb);
    }
}

namespace TheOmenDen.Shared.Extensions;

/// <summary>
/// Provides a set of LINQ Equivalent Methods to the <see cref="ReadOnlySpan{T}"/> type
/// </summary>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// Evaluates the given <paramref name="predicate"/> over the <paramref name="source"/> <see cref="ReadOnlySpan{T}"/>
    /// </summary>
    /// <typeparam name="T">The underlying Span type</typeparam>
    /// <param name="source">The provided span</param>
    /// <param name="predicate">The condition to match against</param>
    /// <returns><c>True</c> on the first match of the given <paramref name="predicate"/>, <c>False</c> otherwise</returns>
    public static bool Any<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        for (var i = 0; i < source.Length - 1; i++)
        {
            if (predicate(source[i]))
            {
                return true;
            }
        }

        return false;
    }
}

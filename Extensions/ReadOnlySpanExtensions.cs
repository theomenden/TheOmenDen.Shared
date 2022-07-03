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
        if (source.IsEmpty)
        {
            return false;
        }

        for (var i = 0; i < source.Length - 1; i++)
        {
            if (predicate(source[i]))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Finds the first occurrence of a <typeparamref name="T"/> from the <paramref name="source"/> that matches the given <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="T">The type to be returned</typeparam>
    /// <param name="source">The source <see cref="ReadOnlySpan{T}"/></param>
    /// <param name="predicate">The provided predicate to match against</param>
    /// <returns>The first matching <typeparamref name="T"/></returns>
    public static T FirstOrDefault<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        if (source.IsEmpty)
        {
            return default;
        }

        for (var i = 0; i < source.Length - 1; i++)
        {
            if (predicate(source[i]))
            {
                return source[i];
            }
        }

        return default;
    }

    /// <summary>
    /// Finds the last occurrence of a <typeparamref name="T"/> from the <paramref name="source"/> that matches the given <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="T">The type to be returned</typeparam>
    /// <param name="source">The source <see cref="ReadOnlySpan{T}"/></param>
    /// <param name="predicate">The provided predicate to match against</param>
    /// <returns>The last matching <typeparamref name="T"/></returns>
    public static T LastOrDefault<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        if (source.IsEmpty)
        {
            return default;
        }

        for (var i= source.Length - 1; i >= 0; i--)
        {
            if (predicate(source[i]))
            {
                return source[i];
            }
        }

        return default;
    }

    /// <summary>
    /// Removes values from supplied <paramref name="source"/> that are duplicated
    /// </summary>
    /// <typeparam name="T">The underlying type to be returned</typeparam>
    /// <param name="source">The provided <see cref="ReadOnlySpan{T}"/></param>
    /// <returns>The distinct values in a new <see cref="ReadOnlySpan{T}"/></returns>
    public static IEnumerable<T> Distinct<T>(this ReadOnlySpan<T> source)
    {
        if (source.IsEmpty)
        {
            return Enumerable.Empty<T>();
        }

        var tempSet = new HashSet<T>(source.Length);

        for (var i = 0; i <= source.Length - 1; i++)
        {
            if (tempSet.Contains(source[i]))
            {
                continue;
            }

            tempSet.Add(source[i]);
        }

        return tempSet;
    }

    /// <summary>
    /// Matches values in the <paramref name="source"/> according to the given <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="T">The underling type to be returned</typeparam>
    /// <param name="source">The provided <see cref="ReadOnlySpan{T}"/></param>
    /// <param name="predicate">The provided predicate to match against</param>
    /// <returns>All the matching values in the <paramref name="source"/> that satisfy the <paramref name="predicate"/></returns>
    public static IEnumerable<T> Where<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        if (source.IsEmpty)
        {
            return Enumerable.Empty<T>();
        }

        var values = new List<T>(source.Length);

        for (var i = 0; i <= source.Length - 1; i++)
        {
            if (predicate(source[i]))
            {
                values.Add(source[i]);
            }
        }

        return values;
    }
}

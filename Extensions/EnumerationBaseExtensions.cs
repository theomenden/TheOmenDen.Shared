namespace TheOmenDen.Shared.Extensions;
public static class EnumerationBaseExtensions
{
    /// <summary>
    /// Retrieves all the underlying <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    public static IEnumerable<T> GetAll<T>()
        where T : EnumerationBase<T>
    {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);

        return fields
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    /// <summary>
    /// Attempts to create an <see cref="EnumerationBase{TKey}"/> of <typeparamref name="T"/> from the given <paramref name="name"/>
    /// </summary>
    /// <typeparam name="T">The type to parse out to</typeparam>
    /// <param name="name">The name to look up</param>
    /// <returns>An <see cref="EnumerationBase{TKey}"/>: <typeparamref name="T"/></returns>
    /// <remarks>Performs a case-insensitive search</remarks>
    public static T Parse<T>(String name)
        where T : EnumerationBase<T>
    {
        return Parse<T>(name, true);
    }

    /// <summary>
    /// <para>
    /// Attempts to create an <see cref="EnumerationBase{TKey}"/> of <typeparamref name="T"/> from the given <paramref name="name"/>
    /// </para>
    /// <para>
    /// Allows the search to be case insensitive, or case sensitive by passing in the <paramref name="ignoreCase"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type to parse out to</typeparam>
    /// <param name="name">The name to look up</param>
    /// <param name="ignoreCase">Should the search be case-sensitive</param>
    /// <returns>An <see cref="EnumerationBase{TKey}"/>: <typeparamref name="T"/></returns>
    public static T Parse<T>(String name, bool ignoreCase)
        where T : EnumerationBase<T>
    {
        var containingEnums = GetAll<T>();

        return containingEnums.First(ce => ce.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture));
    }

    /// <summary>
    /// Attempts to parse the provided <paramref name="name"/> into an <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of <see cref="EnumerationBase{TKey}"/> to return</typeparam>
    /// <param name="name">The provided name</param>
    /// <returns><see cref="ValueTuple"/>: <see cref="bool"/>, <typeparamref name="T"/></returns>
    public static (bool isSuccessful, T result) TryParse<T>(String name) 
        where T : EnumerationBase<T>
    {
        return TryParse<T>(name, true);
    }

    /// <summary>
    /// Attempts to parse the provided <paramref name="name"/> into an <typeparamref name="T"/> -and provides a mechanism to <paramref name="ignoreCase"/>
    /// </summary>
    /// <typeparam name="T">The type of <see cref="EnumerationBase{TKey}"/> to return</typeparam>
    /// <param name="name">The provided name</param>
    /// <param name="ignoreCase">If the attempt at parsing will ignore the provided <paramref name="name"/> casing</param>
    /// <returns><see cref="ValueTuple"/>: (<see cref="bool"/>, <typeparamref name="T"/>)</returns>
    /// <remarks>Uses <see cref="StringComparison.Ordinal"/> under the hood, and <see cref="StringComparison.OrdinalIgnoreCase"/> when <paramref name="ignoreCase"/> is <c>True</c></remarks>
    public static (bool isSuccessful, T result) TryParse<T>(String name, bool ignoreCase) 
        where T : EnumerationBase<T>
    {
        var containingEnums = GetAll<T>();

        var result = containingEnums
            .FirstOrDefault(ce => ce.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

        return result is null ? (false, default) : (true, result);
    }
}
namespace TheOmenDen.Shared.Enumerations;
#nullable disable
/// <summary>
/// <para>A replacement implementation for the standard Enumeration</para>
/// </summary>
/// <remarks>Implements <see cref="IEquatable{T}"/> | <seealso cref="IComparable"/></remarks>
public abstract class EnumerationBase : IEquatable<EnumerationBase>, IComparable
{
    protected EnumerationBase(string name, int id)
    {
        Name = name;
        Id = id;
    }

    /// <summary>
    /// The enumeration name
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// The integer representation
    /// </summary>
    public int Id { get; }

    public override string ToString() => Name;
    
    /// <summary>
    /// Retrieves all the underlying <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    public static IEnumerable<T> GetAll<T>() where T : EnumerationBase
    {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    /// <summary>
    /// Attempts to create an <see cref="EnumerationBase"/> of <typeparamref name="T"/> from the given <paramref name="name"/>
    /// </summary>
    /// <typeparam name="T">The type to parse out to</typeparam>
    /// <param name="name">The name to look up</param>
    /// <returns>An <see cref="EnumerationBase"/>: <typeparamref name="T"/></returns>
    /// <remarks>Performs a case-insensitive search</remarks>
    public static T Parse<T>(String name) where T : EnumerationBase
    {
        return Parse<T>(name, true);
    }

    /// <summary>
    /// <para>
    /// Attempts to create an <see cref="EnumerationBase"/> of <typeparamref name="T"/> from the given <paramref name="name"/>
    /// </para>
    /// <para>
    /// Allows the search to be case insensitive, or case sensitive by passing in the <paramref name="ignoreCase"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type to parse out to</typeparam>
    /// <param name="name">The name to look up</param>
    /// <param name="ignoreCase">Should the search be case-sensitive</param>
    /// <returns>An <see cref="EnumerationBase"/>: <typeparamref name="T"/></returns>
    public static T Parse<T>(String name, bool ignoreCase) where T : EnumerationBase
    {
        var containingEnums = GetAll<T>();

        return containingEnums.First(ce => ce.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture));
    }

    /// <summary>
    /// Attempts to parse the provided <paramref name="name"/> into an <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of <see cref="EnumerationBase"/> to return</typeparam>
    /// <param name="name">The provided name</param>
    /// <returns><see cref="ValueTuple"/>: <see cref="bool"/>, <typeparamref name="T"/></returns>
    public static (bool isSuccessful, T result) TryParse<T>(String name) where T : EnumerationBase
    {
        return TryParse<T>(name, true);
    }

    /// <summary>
    /// Attempts to parse the provided <paramref name="name"/> into an <typeparamref name="T"/> -and provides a mechanism to <paramref name="ignoreCase"/>
    /// </summary>
    /// <typeparam name="T">The type of <see cref="EnumerationBase"/> to return</typeparam>
    /// <param name="name">The provided name</param>
    /// <param name="ignoreCase">If the attempt at parsing will ignore the provided <paramref name="name"/> casing</param>
    /// <returns><see cref="ValueTuple"/>: (<see cref="bool"/>, <typeparamref name="T"/>)</returns>
    /// <remarks>Uses <see cref="StringComparison.Ordinal"/> under the hood, and <see cref="StringComparison.OrdinalIgnoreCase"/> when <paramref name="ignoreCase"/> is <c>True</c></remarks>
    public static (bool isSuccessful, T result) TryParse<T>(String name, bool ignoreCase) where T : EnumerationBase
    {
        var containingEnums = GetAll<T>();

        var result = containingEnums
            .FirstOrDefault(ce => ce.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

        return result is null ? (false, default) : (true, result);
    }

    #region Method Overrides
    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        var typeMatches = GetType() == obj.GetType();

        return obj is EnumerationBase other && Id.Equals(other.Id) && typeMatches;
    }

    public override int GetHashCode()
    {
        return 53 * Id.GetHashCode();
    }

    public Int32 CompareTo(object other) => Id.CompareTo(((EnumerationBase)other).Id);

    public bool Equals(EnumerationBase other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Id == Id && other.Name.Equals(Name);
    }
    #endregion
    #region Operator Overloads
    public static bool operator ==(EnumerationBase left, EnumerationBase right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(EnumerationBase left, EnumerationBase right)
    {
        return !(left == right);
    }

    public static bool operator <(EnumerationBase left, EnumerationBase right)
    {
        return left is null ? right is { } : left.CompareTo(right) < 0;
    }

    public static bool operator <=(EnumerationBase left, EnumerationBase right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(EnumerationBase left, EnumerationBase right)
    {
        return left is { } && left.CompareTo(right) > 0;
    }

    public static bool operator >=(EnumerationBase left, EnumerationBase right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
    #endregion
}
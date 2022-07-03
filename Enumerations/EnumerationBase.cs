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

    public String Name { get; }

    public int Id { get; }

    public override string ToString() => Name;
    
    public static IEnumerable<T> GetAll<T>() where T : EnumerationBase
    {
        var fields = typeof(T).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public static T Parse<T>(String name) where T : EnumerationBase
    {
        var containingEnums = GetAll<T>();

        return containingEnums.First(ce => ce.Name.Equals(name));
    }

    public static T Parse<T>(String name, bool ignoreCase) where T : EnumerationBase
    {
        var containingEnums = GetAll<T>();

        return containingEnums.First(ce => ce.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture));
    }

    public static (bool, T) TryParse<T>(String name) where T : EnumerationBase
    {
        var containingEnums = GetAll<T>();

        var result = containingEnums.FirstOrDefault(ce => ce.Name.Equals(name));

        return result is null ? (false, null) : (true, result);
    }

    public static (bool, T) TryParse<T>(String name, bool ignoreCase) where T : EnumerationBase
    {
        var containingEnums = GetAll<T>();


        var result = containingEnums
            .FirstOrDefault(ce => ce.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCulture));

        return result is null ? (false, null) : (true, result);
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
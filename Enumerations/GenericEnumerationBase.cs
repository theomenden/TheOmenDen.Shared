using TheOmenDen.Shared.Enumerations.Structs;

namespace TheOmenDen.Shared.Enumerations;
/// <summary>
/// <para>A replacement implementation for the standard Enumeration</para>
/// </summary>
/// <remarks>Implements <seealso cref="IComparable"/></remarks>
public abstract record EnumerationBase<TKey> :
    EnumerationBase<TKey, Int32>
    where TKey : EnumerationBase<TKey, Int32>
{
    protected EnumerationBase(String name, Int32 id)
        : base(name, id)
    {
    }
}

/// <summary>
/// <para>A replacement implementation for the standard Enumeration</para>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TEnumeration"></typeparam>
/// <remarks>Implements <see cref="IComparable{T}"/></remarks>
public abstract record EnumerationBase<TKey, TEnumeration> :
    IEnumerationBase,
    IComparable<EnumerationBase<TKey, TEnumeration>>
    where TKey : EnumerationBase<TKey, TEnumeration>
    where TEnumeration : IEquatable<TEnumeration>, IComparable<TEnumeration>
{
    #region Constructors
    protected EnumerationBase(String name, TEnumeration value)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;

        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
    #endregion
    #region Properties
    public String Name { get; }

    public TEnumeration Value { get; }
    #endregion
    #region Private Static Members
    private static readonly Lazy<TKey[]> Enumerations =
        new(GetAllEnumerations, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<Dictionary<String, TKey>> EnumValueFromNames =
        new(() => Enumerations.Value.ToDictionary(item => item.Name));

    private static readonly Lazy<Dictionary<String, TKey>> EnumValueFromNamesIgnoresCase =
        new(() => Enumerations.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private static readonly Lazy<Dictionary<TEnumeration, TKey>> EnumFromValue =
        new(() =>
        {
            var enumDictionary = new Dictionary<TEnumeration, TKey>();

            foreach (var item in Enumerations.Value)
            {
                if (enumDictionary.ContainsKey(item.Value))
                {
                    enumDictionary.Add(item.Value, item);
                }
            }

            return enumDictionary;
        });

    private static TKey[] GetAllEnumerations()
    {
        var baseEnum = typeof(TKey);

        return Assembly.GetAssembly(baseEnum)
            .GetTypes()
            .Where(type => baseEnum.IsAssignableFrom(type))
            .SelectMany(type => type.GetTypeFields<TKey>())
            .OrderBy(type => type.Name)
            .ToArray();
    }

    private static TKey GetByName(String name, Dictionary<String, TKey> searchDictionary)
    {
        if (!searchDictionary.TryGetValue(name, out var result))
        {
            throw new KeyNotFoundException(nameof(name));
        }

        return result;
    }
    #endregion
    #region Public Static Members
    public static IReadOnlyCollection<TKey> ReadOnlyEnumerationList
    => EnumValueFromNames.Value.Values
        .ToList()
        .AsReadOnly();

    public static TKey Parse(String name, Boolean ignoreCase = false)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return GetByName(name,
            ignoreCase
                ? EnumValueFromNamesIgnoresCase.Value
                : EnumValueFromNames.Value);
    }

    public static (Boolean result, TKey enumeration) TryParse(String name, Boolean ignoreCase)
    {
        var tkeyResult = default(TKey);

        if (String.IsNullOrWhiteSpace(name))
        {
            return new(false, tkeyResult);
        }

        if (ignoreCase)
        {
            EnumValueFromNamesIgnoresCase.Value.TryGetValue(name, out tkeyResult);

            return new(true, tkeyResult);
        }

        EnumValueFromNames.Value.TryGetValue(name, out tkeyResult);

        return new(true, tkeyResult);
    }

    public static TKey ParseFromValue(TEnumeration enumeration)
    {
        if (enumeration is null)
        {
            throw new ArgumentNullException(nameof(enumeration));
        }

        if (!EnumFromValue.Value.TryGetValue(enumeration, out var result))
        {
            throw new KeyNotFoundException(nameof(enumeration));
        }

        return result;
    }

    public static TKey ParseFromValue(TEnumeration enumeration, TKey defaultValue)
    {
        if (enumeration is null)
        {
            throw new ArgumentNullException(nameof(enumeration));
        }

        return !EnumFromValue.Value.TryGetValue(enumeration, out var result)
            ? defaultValue
            : result;
    }

    public static (Boolean result, TKey value) TryParseFromValue(TEnumeration? enumeration, TKey defaultValue)
    {
        if (enumeration is null)
        {
            return new(false, defaultValue);
        }

        EnumFromValue.Value.TryGetValue(enumeration, out var result);

        return new(true, result);
    }
    #endregion
    #region Conditions and Consequences
    public Consequence<TKey, TEnumeration> When(EnumerationBase<TKey, TEnumeration> enumerationCondition) 
        =>new (Equals(enumerationCondition), false, this);

    public Consequence<TKey, TEnumeration> When(IEnumerable<EnumerationBase<TKey, TEnumeration>> genericEnumerationConditions)
    => new (genericEnumerationConditions.Contains(this), false, this);

    public Consequence<TKey, TEnumeration> When(params EnumerationBase<TKey, TEnumeration>[] genericEnumerationConditions)
        => new (genericEnumerationConditions.Contains(this), false, this);
    #endregion
    #region Overrides
    public sealed override string ToString() => Name;

    public override int GetHashCode() => Value.GetHashCode();
    #endregion
    #region Implementations
    public virtual bool Equals(EnumerationBase<TKey, TEnumeration>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null
               && Value.Equals(other.Value);
    }

    public int CompareTo(EnumerationBase<TKey, TEnumeration> other)
    {
        return Value.CompareTo(other.Value);
    }
    #endregion
    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(EnumerationBase<TKey, TEnumeration> lhs,
        EnumerationBase<TKey, TEnumeration> rhs)
        => lhs.CompareTo(rhs) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(EnumerationBase<TKey, TEnumeration> lhs,
        EnumerationBase<TKey, TEnumeration> rhs)
        => lhs.CompareTo(rhs) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(EnumerationBase<TKey, TEnumeration> lhs,
        EnumerationBase<TKey, TEnumeration> rhs)
        => lhs.CompareTo(rhs) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(EnumerationBase<TKey, TEnumeration> lhs,
        EnumerationBase<TKey, TEnumeration> rhs)
        => lhs.CompareTo(rhs) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TEnumeration(EnumerationBase<TKey, TEnumeration> enumeration)
        => enumeration is not null ?
            enumeration.Value
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumerationBase<TKey, TEnumeration>(TEnumeration genericEnumeration)
        => ParseFromValue(genericEnumeration);
    #endregion
}
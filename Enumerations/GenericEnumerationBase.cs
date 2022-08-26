using TheOmenDen.Shared.Enumerations.Structs;

namespace TheOmenDen.Shared.Enumerations;
/// <summary>
/// <para>A replacement implementation for the standard Enumeration</para>
/// <para>Like the original <see langword="enum"/>, we use an <see cref="Int32"/> as the value field</para>
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
/// <typeparam name="TEnumKey"></typeparam>
/// <typeparam name="TEnumValue"></typeparam>
/// <remarks>Implements <see cref="IComparable{T}"/></remarks>
public abstract record EnumerationBase<TEnumKey, TEnumValue> :
    IEnumerationBase,
    IComparable<EnumerationBase<TEnumKey, TEnumValue>>
    where TEnumKey : EnumerationBase<TEnumKey, TEnumValue>
    where TEnumValue : IEquatable<TEnumValue>, IComparable<TEnumValue>
{
    #region Constructors
    protected EnumerationBase(String name, TEnumValue value)
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

    public TEnumValue Value { get; }
    #endregion
    #region Private Static Members
    private static readonly Lazy<TEnumKey[]> Enumerations =
        new(GetAllEnumerations, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<Dictionary<String, TEnumKey>> EnumValueFromNames =
        new(() => Enumerations.Value.ToDictionary(item => item.Name));

    private static readonly Lazy<Dictionary<String, TEnumKey>> EnumValueFromNamesIgnoresCase =
        new(() => Enumerations.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private static readonly Lazy<Dictionary<TEnumValue, TEnumKey>> EnumFromValue =
        new(() =>
        {
            var enumDictionary = new Dictionary<TEnumValue, TEnumKey>();

            foreach (var item in Enumerations.Value)
            {
                if (enumDictionary.ContainsKey(item.Value))
                {
                    enumDictionary.Add(item.Value, item);
                }
            }

            return enumDictionary;
        });

    private static TEnumKey[] GetAllEnumerations()
    {
        var baseEnum = typeof(TEnumKey);

        return Assembly.GetAssembly(baseEnum)
            .GetTypes()
            .Where(type => baseEnum.IsAssignableFrom(type))
            .SelectMany(type => type.GetTypeFields<TEnumKey>())
            .OrderBy(type => type.Name)
            .ToArray();
    }

    private static TEnumKey GetByName(String name, Dictionary<String, TEnumKey> searchDictionary)
    {
        if (!searchDictionary.TryGetValue(name, out var result))
        {
            throw new KeyNotFoundException(nameof(name));
        }

        return result;
    }
    #endregion
    #region Public Static Members
    public static IReadOnlyCollection<TEnumKey> ReadOnlyEnumerationList
    => EnumValueFromNames.Value.Values
        .ToList()
        .AsReadOnly();

    public static TEnumKey Parse(String name, Boolean ignoreCase = false)
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

    public static (Boolean result, TEnumKey enumeration) TryParse(String name, Boolean ignoreCase)
    {
        var tkeyResult = default(TEnumKey);

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

    public static TEnumKey ParseFromValue(TEnumValue enumeration)
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

    public static TEnumKey ParseFromValue(TEnumValue enumeration, TEnumKey defaultValue)
    {
        if (enumeration is null)
        {
            throw new ArgumentNullException(nameof(enumeration));
        }

        return !EnumFromValue.Value.TryGetValue(enumeration, out var result)
            ? defaultValue
            : result;
    }

    public static (Boolean result, TEnumKey value) TryParseFromValue(TEnumValue? enumeration, TEnumKey defaultValue)
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
    public Consequence<TEnumKey, TEnumValue> When(EnumerationBase<TEnumKey, TEnumValue> enumerationCondition) 
        =>new (Equals(enumerationCondition), false, this);

    public Consequence<TEnumKey, TEnumValue> When(IEnumerable<EnumerationBase<TEnumKey, TEnumValue>> genericEnumerationConditions)
    => new (genericEnumerationConditions.Contains(this), false, this);

    public Consequence<TEnumKey, TEnumValue> When(params EnumerationBase<TEnumKey, TEnumValue>[] genericEnumerationConditions)
        => new (genericEnumerationConditions.Contains(this), false, this);
    #endregion
    #region Overrides
    public sealed override string ToString() => Name;

    public override int GetHashCode() => Value.GetHashCode();
    #endregion
    #region Implementations
    public virtual bool Equals(EnumerationBase<TEnumKey, TEnumValue>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null
               && Value.Equals(other.Value);
    }

    public int CompareTo(EnumerationBase<TEnumKey, TEnumValue> other)
    {
        return Value.CompareTo(other.Value);
    }
    #endregion
    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(EnumerationBase<TEnumKey, TEnumValue> lhs,
        EnumerationBase<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(EnumerationBase<TEnumKey, TEnumValue> lhs,
        EnumerationBase<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(EnumerationBase<TEnumKey, TEnumValue> lhs,
        EnumerationBase<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(EnumerationBase<TEnumKey, TEnumValue> lhs,
        EnumerationBase<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TEnumValue(EnumerationBase<TEnumKey, TEnumValue> enumeration)
        => enumeration is not null ?
            enumeration.Value
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumerationBase<TEnumKey, TEnumValue>(TEnumValue genericEnumeration)
        => ParseFromValue(genericEnumeration);
    #endregion
}
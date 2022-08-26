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
    /// <summary>
    /// The Key/Name for the Enumeration
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// The underlying value for the Enumeration
    /// </summary>
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
                if (!enumDictionary.ContainsKey(item.Value))
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
            ?.GetTypes()
            .Where(type => baseEnum.IsAssignableFrom(type))
            .SelectMany(type => type.GetTypeFields<TEnumKey>())
            .OrderBy(type => type.Name)
            .ToArray() ?? Array.Empty<TEnumKey>();
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
    /// <summary>
    /// A readonly collection of the Enumerations in the particular type
    /// </summary>
    public static IReadOnlyCollection<TEnumKey> ReadOnlyEnumerationList
    => EnumValueFromNames.Value.Values
        .ToList()
        .AsReadOnly();

    /// <summary>
    /// Searches for the <paramref name="name"/> within the collection of <typeparamref name="TEnumKey"/>s
    /// </summary>
    /// <param name="name">The name to parse out</param>
    /// <param name="ignoreCase"><see langword="true"/> to ignore case during search; <see langword="false"/> as default</param>
    /// <returns><typeparamref name="TEnumKey"/> typed enumeration</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="name"/> is null or empty</exception>
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

    /// <summary>
    /// Searches for the <paramref name="name"/> within the collection of <typeparamref name="TEnumKey"/>s - DOES NOT THROW
    /// </summary>
    /// <param name="name">The name to search under</param>
    /// <param name="ignoreCase"><see langword="true"/> to ignore case during search; <see langword="false"/> as default</param>
    /// <returns>
    /// <para><see cref="ValueTuple{T1, T2}"/> of (<see cref="Boolean"/>, <typeparamref name="TEnumKey"/>)</para>
    ///<para><c>Result</c> is <see langword="true"/> when parsing is successful; <see langword="false"/> otherwise</para>
    /// </returns>
    public static (Boolean result, TEnumKey enumeration) TryParse(String name, Boolean ignoreCase = false)
    {
        TEnumKey tkeyResult;

        if (String.IsNullOrWhiteSpace(name))
        {
            return new(false, null);
        }

        if (ignoreCase)
        {
            EnumValueFromNamesIgnoresCase.Value.TryGetValue(name, out tkeyResult);

            return new(true, tkeyResult);
        }

        EnumValueFromNames.Value.TryGetValue(name, out tkeyResult);

        return new(true, tkeyResult);
    }

    /// <summary>
    /// Attempts to parse out an existing, and matching <see cref="EnumerationBase{TEnumKey, TEnumValue}"/> from the supplied <typeparamref name="TEnumValue"/> <paramref name="value"/>
    /// </summary>
    /// <param name="value">The <typeparamref name="TEnumValue"/> we're trying to parse</param>
    /// <returns>The <typeparamref name="TEnumKey"/></returns>
    /// <exception cref="ArgumentNullException">If the provided <typeparamref name="TEnumValue"/> is null</exception>
    /// <exception cref="KeyNotFoundException">If there is no match to the provided <paramref name="value"/></exception>
    public static TEnumKey ParseFromValue(TEnumValue value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (!EnumFromValue.Value.TryGetValue(value, out var result))
        {
            throw new KeyNotFoundException(nameof(value));
        }

        return result;
    }

    /// <summary>
    /// Attempts to parse out an existing, and matching <see cref="EnumerationBase{TEnumKey, TEnumValue}"/> from the supplied <typeparamref name="TEnumValue"/> <paramref name="value"/>
    /// </summary>
    /// <param name="value">The <typeparamref name="TEnumValue"/> we're trying to parse</param>
    /// <param name="defaultValue">The value we can fall back on in case the provided <paramref name="value"/> fails</param>
    /// <returns>The <typeparamref name="TEnumKey"/></returns>
    /// <exception cref="ArgumentNullException">If the provided <typeparamref name="TEnumValue"/> is null</exception>
    public static TEnumKey ParseFromValueOrDefault(TEnumValue value, TEnumKey defaultValue)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return !EnumFromValue.Value.TryGetValue(value, out var result)
            ? defaultValue
            : result;
    }

    /// <summary>
    /// Searches for the <paramref name="enumValue"/> within the collection of <typeparamref name="TEnumKey"/>s - DOES NOT THROW
    /// </summary>
    /// <param name="enumValue">The name to search under</param>
    /// <param name="defaultValue">Our fallback value in case parsing fails</param>
    /// <returns>
    /// <para><see cref="ValueTuple{T1, T2}"/> of (<see cref="Boolean"/>, <typeparamref name="TEnumKey"/>)</para>
    ///<para><c>Result</c> is <see langword="true"/> when parsing is successful; <see langword="false"/> otherwise</para>
    /// </returns>
    public static (Boolean result, TEnumKey value) TryParseFromValue(TEnumValue? enumValue, TEnumKey defaultValue)
    {
        if (enumValue is null)
        {
            return new(false, defaultValue);
        }

        EnumFromValue.Value.TryGetValue(enumValue, out var result);

        return new(true, result);
    }
    #endregion
    #region Conditions and Consequences
    /// <summary>
    /// A set of "When...Then..." statements to be executed for pattern matching and control flow
    /// </summary>
    /// <param name="enumerationCondition">The supplied condition we want to evaluate against</param>
    /// <returns>A <see cref="Consequence{TKey, TEnumeration}"/> for further "When...Then..." pattern chaining</returns>
    public Consequence<TEnumKey, TEnumValue> When(EnumerationBase<TEnumKey, TEnumValue> enumerationCondition) 
        =>new (Equals(enumerationCondition), false, this);
    /// <summary>
    /// A set of "When...Then..." statements to be executed for pattern matching and control flow
    /// </summary>
    /// <param name="genericEnumerationConditions">The supplied conditions we want to evaluate against</param>
    /// <returns>A <see cref="Consequence{TKey, TEnumeration}"/> for further "When...Then..." pattern chaining</returns>
    public Consequence<TEnumKey, TEnumValue> When(IEnumerable<EnumerationBase<TEnumKey, TEnumValue>> genericEnumerationConditions)
    => new (genericEnumerationConditions.Contains(this), false, this);
    /// <summary>
    /// A set of "When...Then..." statements to be executed for pattern matching and control flow
    /// </summary>
    /// <param name="genericEnumerationConditions">The supplied conditions we want to evaluate against</param>
    /// <returns>A <see cref="Consequence{TKey, TEnumeration}"/> for further "When...Then..." pattern chaining</returns>
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
    public static implicit operator TEnumValue(EnumerationBase<TEnumKey, TEnumValue>? enumeration)
        => enumeration is not null ?
            enumeration.Value
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumerationBase<TEnumKey, TEnumValue>(TEnumValue genericEnumeration)
        => ParseFromValue(genericEnumeration);
    #endregion
}
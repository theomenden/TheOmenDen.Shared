using TheOmenDen.Shared.Enumerations.Structs;
using TheOmenDen.Shared.Exceptions;
using TheOmenDen.Shared.Exceptions.Templates;

namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract record EnumerationBaseFlag<TKey> :
    EnumerationBaseFlag<TKey, Int32>
    where TKey : EnumerationBaseFlag<TKey, Int32>
{
    protected EnumerationBaseFlag(String name, Int32 value)
    :base(name, value)
    {

    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract record EnumerationBaseFlag<TKey, TValue> :
    EnumerationBaseFlagAbstraction<TKey, TValue>,
    IEnumerationBase,
    IComparable<EnumerationBaseFlag<TKey, TValue>>
    where TKey : EnumerationBaseFlag<TKey, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    #region Constructors
    protected EnumerationBaseFlag(String name, TValue value)
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

    public TValue Value { get; }
    #endregion
    #region Overrides
    public sealed override string ToString() => Name;

    public override int GetHashCode() => Value.GetHashCode();
    #endregion
    #region Private Static Members
    private static readonly Lazy<TKey[]> Enumerations =
        new(GetAllEnumerations, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<Dictionary<String, TKey>> EnumValueFromNames =
        new(() => Enumerations.Value.ToDictionary(item => item.Name));

    private static readonly Lazy<Dictionary<String, TKey>> EnumValueFromNamesIgnoresCase =
        new(() => Enumerations.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    private static readonly Lazy<Dictionary<TValue, TKey>> EnumFromValue =
        new(() =>
        {
            var enumDictionary = new Dictionary<TValue, TKey>()
            ;
            foreach (var item in Enumerations.Value.Where(item => enumDictionary.ContainsKey(item.Value)))
            {
                enumDictionary.Add(item.Value, item);
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
            var message = String.Format(Messages.EnumerationNotFoundByName, typeof(TKey).Name, name);

            throw new EnumerationCouldNotBeParsedByValueException(message);
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
            var message = String.Format(Messages.CannotBeNullOrEmpty, nameof(name));

            throw new ArgumentNullException(message);
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

    public static TKey ParseFromValue(TValue enumeration)
    {
        var message = String.Empty;

        if (enumeration is null)
        {
            message = String.Format(Messages.CannotBeNullOrEmpty, nameof(enumeration));
            throw new ArgumentNullException(message);
        }

        if (!EnumFromValue.Value.TryGetValue(enumeration, out var result))
        {
            message = String.Format(Messages.EnumerationNotFoundByValue, typeof(TKey).Name, nameof(enumeration));

            throw new EnumerationCouldNotBeParsedByValueException(message);
        }

        return result;
    }

    public static TKey ParseFromValue(TValue enumeration, TKey defaultValue)
    {
        if (enumeration is null)
        {
            var message = String.Format(Messages.CannotBeNullOrEmpty, nameof(enumeration));

            throw new ArgumentNullException(message);
        }

        return !EnumFromValue.Value.TryGetValue(enumeration, out var result)
            ? defaultValue
            : result;
    }

    public static (Boolean result, TKey value) TryParseFromValue(TValue? enumeration, TKey defaultValue)
    {
        if (enumeration is null)
        {
            return new(false, defaultValue);
        }

        EnumFromValue.Value.TryGetValue(enumeration, out var result);

        return new(true, result);
    }

    public static TKey DeserializeValue(TValue value)
    {
        var enumerations = GetAllEnumerations();

        return enumerations.FirstOrDefault(enumeration => enumeration.Value.Equals(value))
            ?? throw new EnumerationNotFoundException(String.Format(Messages.EnumerationNotFoundByValue,typeof(TKey).Name, nameof(value)));
    }
    #endregion
    #region Conditions and Consequences
    public Consequence<TKey, TValue> When(EnumerationBaseFlag<TKey, TValue> enumerationCondition)
        => new(Equals(enumerationCondition), false, this);

    public Consequence<TKey, TValue> When(IEnumerable<EnumerationBaseFlag<TKey, TValue>> genericEnumerationConditions)
    => new(genericEnumerationConditions.Contains(this), false, this);

    public Consequence<TKey, TValue> When(params EnumerationBaseFlag<TKey, TValue>[] genericEnumerationConditions)
        => new(genericEnumerationConditions.Contains(this), false, this);
    #endregion
    #region Implementations
    public virtual bool Equals(EnumerationBaseFlag<TKey, TValue>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null
               && Value.Equals(other.Value);
    }

    public int CompareTo(EnumerationBaseFlag<TKey, TValue> other)
    {
        return Value.CompareTo(other.Value);
    }
    #endregion
    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(EnumerationBaseFlag<TKey, TValue> lhs,
        EnumerationBaseFlag<TKey, TValue> rhs)
        => lhs.CompareTo(rhs) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(EnumerationBaseFlag<TKey, TValue> lhs,
        EnumerationBaseFlag<TKey, TValue> rhs)
        => lhs.CompareTo(rhs) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(EnumerationBaseFlag<TKey, TValue> lhs,
        EnumerationBaseFlag<TKey, TValue> rhs)
        => lhs.CompareTo(rhs) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(EnumerationBaseFlag<TKey, TValue> lhs,
        EnumerationBaseFlag<TKey, TValue> rhs)
        => lhs.CompareTo(rhs) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TValue(EnumerationBaseFlag<TKey, TValue> enumeration)
        => enumeration is not null ?
            enumeration.Value
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumerationBaseFlag<TKey, TValue>(TValue genericEnumeration)
        => ParseFromValue(genericEnumeration);
    #endregion
}

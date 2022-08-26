using TheOmenDen.Shared.Enumerations.Structs;
using TheOmenDen.Shared.Exceptions;
using TheOmenDen.Shared.Exceptions.Templates;

namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEnumKey"></typeparam>
public abstract record EnumerationBaseFlag<TEnumKey> :
    EnumerationBaseFlag<TEnumKey, Int32>
    where TEnumKey : EnumerationBaseFlag<TEnumKey, Int32>
{
    protected EnumerationBaseFlag(String name, Int32 value)
    : base(name, value)
    {
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEnumKey"></typeparam>
/// <typeparam name="TEnumValue"></typeparam>
public abstract record EnumerationBaseFlag<TEnumKey, TEnumValue> :
    EnumerationBaseFlagAbstraction<TEnumKey, TEnumValue>,
    IEnumerationBase,
    IComparable<EnumerationBaseFlag<TEnumKey, TEnumValue>>
    where TEnumKey : EnumerationBaseFlag<TEnumKey, TEnumValue>
    where TEnumValue : IEquatable<TEnumValue>, IComparable<TEnumValue>
{
    #region Constructors
    protected EnumerationBaseFlag(String name, TEnumValue value)
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
    #region Overrides
    public sealed override string ToString() => Name;

    public override int GetHashCode() => Value.GetHashCode();
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
            var enumDictionary = new Dictionary<TEnumValue, TEnumKey>()
            ;
            foreach (var item in Enumerations.Value.Where(item => enumDictionary.ContainsKey(item.Value)))
            {
                enumDictionary.Add(item.Value, item);
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
            var message = String.Format(Messages.EnumerationNotFoundByName, typeof(TEnumKey).Name, name);

            throw new EnumerationCouldNotBeParsedByValueException(message);
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
        if (!String.IsNullOrWhiteSpace(name))
        {
            return GetByName(name,
                ignoreCase
                    ? EnumValueFromNamesIgnoresCase.Value
                    : EnumValueFromNames.Value);
        }

        var message = String.Format(Messages.CannotBeNullOrEmpty, nameof(name));

        throw new ArgumentNullException(message);
    }

    public static (Boolean result, TEnumKey enumeration) TryParse(String name, Boolean ignoreCase)
    {
        TEnumKey keyResult;

        if (String.IsNullOrWhiteSpace(name))
        {
            return new(false, null);
        }

        if (ignoreCase)
        {
            EnumValueFromNamesIgnoresCase.Value.TryGetValue(name, out keyResult);

            return new(true, keyResult);
        }

        EnumValueFromNames.Value.TryGetValue(name, out keyResult);

        return new(true, keyResult);
    }

    public static TEnumKey ParseFromValue(TEnumValue enumeration)
    {
        string message;

        if (enumeration is null)
        {
            message = String.Format(Messages.CannotBeNullOrEmpty, nameof(enumeration));

            throw new ArgumentNullException(message);
        }

        if (EnumFromValue.Value.TryGetValue(enumeration, out var result))
        {
            return result;
        }

        message = String.Format(Messages.EnumerationNotFoundByValue, typeof(TEnumKey).Name, nameof(enumeration));

        throw new EnumerationCouldNotBeParsedByValueException(message);
    }

    public static TEnumKey ParseFromValue(TEnumValue? enumeration, TEnumKey defaultValue)
    {
        if (enumeration is not null)
        {
            return !EnumFromValue.Value.TryGetValue(enumeration, out var result)
                ? defaultValue
                : result;
        }

        var message = String.Format(Messages.CannotBeNullOrEmpty, nameof(enumeration));

        throw new ArgumentNullException(message);
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

    public static TEnumKey DeserializeValue(TEnumValue value)
    {
        var enumerations = GetAllEnumerations();

        return enumerations.FirstOrDefault(enumeration => enumeration.Value.Equals(value))
            ?? throw new EnumerationNotFoundException(String.Format(Messages.EnumerationNotFoundByValue, typeof(TEnumKey).Name, nameof(value)));
    }
    #endregion
    #region Conditions and Consequences
    public Consequence<TEnumKey, TEnumValue> When(EnumerationBaseFlag<TEnumKey, TEnumValue> enumerationCondition)
    => new(Equals(enumerationCondition), false, this);

    public Consequence<TEnumKey, TEnumValue> When(IEnumerable<EnumerationBaseFlag<TEnumKey, TEnumValue>> genericEnumerationConditions)
    => new(genericEnumerationConditions.Contains(this), false, this);

    public Consequence<TEnumKey, TEnumValue> When(params EnumerationBaseFlag<TEnumKey, TEnumValue>[] genericEnumerationConditions)
    => new(genericEnumerationConditions.Contains(this), false, this);
    #endregion
    #region Implementations
    public virtual bool Equals(EnumerationBaseFlag<TEnumKey, TEnumValue>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null
               && Value.Equals(other.Value);
    }

    public int CompareTo(EnumerationBaseFlag<TEnumKey, TEnumValue> other)
    => Value.CompareTo(other.Value);
    #endregion
    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(EnumerationBaseFlag<TEnumKey, TEnumValue> lhs,
        EnumerationBaseFlag<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(EnumerationBaseFlag<TEnumKey, TEnumValue> lhs,
        EnumerationBaseFlag<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(EnumerationBaseFlag<TEnumKey, TEnumValue> lhs,
        EnumerationBaseFlag<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(EnumerationBaseFlag<TEnumKey, TEnumValue> lhs,
        EnumerationBaseFlag<TEnumKey, TEnumValue> rhs)
        => lhs.CompareTo(rhs) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TEnumValue(EnumerationBaseFlag<TEnumKey, TEnumValue> enumeration)
        => enumeration is not null ?
            enumeration.Value
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator EnumerationBaseFlag<TEnumKey, TEnumValue>(TEnumValue genericEnumeration)
        => ParseFromValue(genericEnumeration);
    #endregion
}

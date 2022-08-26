using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Exceptions;
using TheOmenDen.Shared.Exceptions.Templates;
using TheOmenDen.Shared.Guards;

namespace TheOmenDen.Shared.Enumerations;
/// <summary>
/// Defines a way to retrieve and work with Flagged based enumerations within powers of 2.
/// </summary>
/// <typeparam name="TEnumKey">The type of key</typeparam>
/// <typeparam name="TEnumValue">The underlying value</typeparam>
public abstract record EnumerationBaseFlagAbstraction<TEnumKey, TEnumValue>
where TEnumKey : EnumerationBaseFlag<TEnumKey, TEnumValue>
where TEnumValue : IEquatable<TEnumValue>, IComparable<TEnumValue>
{
    #region Constructors
    protected EnumerationBaseFlagAbstraction()
    {
    }
    #endregion
    #region Protected Static Methods
    /// <summary>
    /// Constructs an <see cref="IEnumerable{T}"/> that can allow for multiple flagged values.
    /// </summary>
    /// <param name="enumeration">The enumeration's value we're looking to retrieve</param>
    /// <param name="allEnumerationKeys">An <see cref="IEnumerable{T}"/> <seealso cref="EnumerationBase{TKey, TEnumeration}"/></param>
    /// <returns><see cref="IEnumerable{T}"/> that represents a given <paramref name="enumeration"/> with a set of values</returns>
    protected static IEnumerable<TEnumKey> GetFlagValues(TEnumValue enumeration, IEnumerable<TEnumKey> allEnumerationKeys)
    {
        Guard.FromNull(enumeration, nameof(enumeration));
        Guard.FromInvalidInput(enumeration, nameof(enumeration));
        Guard.FromNegativeValues(enumeration, nameof(enumeration));

        var inputValue = Int32.Parse(enumeration.ToString()!);

        var enumerationFlagStates = new Dictionary<TEnumKey, Boolean>(5);

        var inputKeys = allEnumerationKeys.ToList();

        AllowUnsafeFlags(inputKeys);

        var maximumKeyValueAllowed = CalculateHighestAllowedFlag(inputKeys);

        var typedMaximumValue = GetMaximumValue();

        foreach (var key in inputKeys)
        {
            var currentEnumerationAsInt = Int32.Parse(key.Value.ToString() ?? String.Empty);

            CheckEnumerationForValuesLessThanNegativeOne(currentEnumerationAsInt);

            if (currentEnumerationAsInt == inputValue)
            {
                return new List<TEnumKey> { key };
            }

            if (inputValue == -1 || enumeration.Equals(typedMaximumValue))
            {
                return inputKeys
                    .Where(x => Int64.Parse(x.Value.ToString() ?? String.Empty) > 0);
            }

            AssignFlagStateValues(inputValue, currentEnumerationAsInt, key, enumerationFlagStates);
        }

        return inputValue > maximumKeyValueAllowed ? Enumerable.Empty<TEnumKey>() : CreateKeysList(enumerationFlagStates);
    }
    #endregion
    #region Private Static Methods
    private static Int32 CalculateHighestAllowedFlag(IReadOnlyList<TEnumKey> inputList)
    => GetHighestFlagValue(inputList) * 2 - 1;

    private static void CheckEnumerationForValuesLessThanNegativeOne(Int32 value)
    {
        if (value >= -1)
        {
            return;
        }

        var message = String.Format(Messages.EnumerationContainsNegativeValue, typeof(TEnumKey).Name);

        throw new ArgumentValueWasNegativeException(message);
    }

    private static IEnumerable<TEnumKey> CreateKeysList(Dictionary<TEnumKey, Boolean> enumerationFlagStates)
    => enumerationFlagStates
            .Where(entry => entry.Value)
            .Select(entry => entry.Key)
            .ToArray() ?? Enumerable.Empty<TEnumKey>();

    private static void AllowUnsafeFlags(IEnumerable<TEnumKey> keys)
    {
        var attribute =
            (AllowUnsafeEnumerationKeysAttribute)Attribute.GetCustomAttribute(typeof(TEnumKey),
                typeof(AllowUnsafeEnumerationKeysAttribute));

        if (attribute is null)
        {
            CheckProvidedEnumerationsForPowersOfTwo(keys);
        }
    }

    private static void AssignFlagStateValues(Int32 inputValue, Int32 currentEnumerationValue, TEnumKey key, IDictionary<TEnumKey, Boolean> enumerationFlagStates)
    {
        if (enumerationFlagStates.ContainsKey(key) || currentEnumerationValue is 0)
        {
            return;
        }

        var flagState = (inputValue & currentEnumerationValue) == currentEnumerationValue;

        enumerationFlagStates.Add(key, flagState);
    }

    private static void CheckProvidedEnumerationsForPowersOfTwo(IEnumerable<TEnumKey> enumerations)
    {
        var enumerationsAsList = enumerations.ToList();

        var enumerationValues = enumerationsAsList
            .Select(enumeration => Int32.Parse(enumeration.Value.ToString() ?? string.Empty))
            .ToArray();

        if (Int32.Parse(enumerationsAsList[0].Value.ToString() ?? String.Empty) == 0)
        {
            enumerationsAsList.RemoveAt(0);
        }

        var initialPower = enumerationsAsList
            .Select(enumeration => Int32.Parse(enumeration.Value.ToString() ?? String.Empty))
            .FirstOrDefault(IsAPowerOfTwo);

        var highestValue = GetHighestFlagValue(enumerationsAsList);

        var currentValue = initialPower;

        while (currentValue != highestValue)
        {
            var nextPower = currentValue * 2;

            var result = Array.BinarySearch(enumerationValues, nextPower);

            if (result < 0)
            {
                var message = String.Format(Messages.EnumerationNotConsecutivePowerOfTwo,
                    result, currentValue);

                throw new EnumerationFlagWIthNoPowersOfTwoException(message);
            }

            currentValue = nextPower;
        }
    }

    private static Boolean IsAPowerOfTwo(Int32 numberToCheck)
        => numberToCheck != 0
        && (numberToCheck & (numberToCheck - 1)) == 0;

    private static Int32 GetHighestFlagValue(IReadOnlyList<TEnumKey> keyList)
    {
        var greatestIndex = keyList.Count - 1;
        var greatestValue = Int32.Parse(keyList[^1].Value.ToString() ?? string.Empty);

        if (IsAPowerOfTwo(greatestValue))
        {
            return greatestValue;
        }

        for (var i = greatestIndex; i >= 0; i--)
        {
            var currentValue = Int32.Parse(keyList[^1].Value.ToString() ?? string.Empty);

            if (!IsAPowerOfTwo(currentValue))
            {
                continue;
            }

            greatestValue = currentValue;
            break;
        }

        return greatestValue;
    }

    private static TEnumValue GetMaximumValue()
    {
        var maximumField = typeof(TEnumValue)
            .GetField("MaxValue", BindingFlags.Public | BindingFlags.Static);

        if (maximumField is null)
        {
            var message = String.Format(Messages.CannotBeNullOrEmpty, nameof(maximumField));
            throw new ArgumentException(message);
        }

        var maxValue = (TEnumValue)maximumField.GetValue(null);

        return maxValue;
    }
    #endregion
}
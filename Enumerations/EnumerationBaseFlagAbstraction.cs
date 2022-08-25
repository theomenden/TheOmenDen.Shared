using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Exceptions;
using TheOmenDen.Shared.Exceptions.Templates;
using TheOmenDen.Shared.Guards;

namespace TheOmenDen.Shared.Enumerations;

public abstract record EnumerationBaseFlagAbstraction<TKey, TEnumeration>
where TKey : EnumerationBaseFlag<TKey, TEnumeration>
where TEnumeration : IEquatable<TEnumeration>, IComparable<TEnumeration>
{
    protected EnumerationBaseFlagAbstraction()
    {
    }

    /// <summary>
    /// Constructs an <see cref="IEnumerable{T}"/> that can allow for multiple flagged values.
    /// </summary>
    /// <param name="enumeration">The enumeration's value we're looking to retrieve</param>
    /// <param name="allEnumerationKeys">An <see cref="IEnumerable{T}"/> <seealso cref="EnumerationBase{TKey, TEnumeration}"/></param>
    /// <returns><see cref="IEnumerable{T}"/> that represents a given <paramref name="enumeration"/> with a set of values</returns>
    protected static IEnumerable<TKey> GetFlagValues(TEnumeration enumeration, IEnumerable<TKey> allEnumerationKeys)
    {
        Guard.FromNull(enumeration, nameof(enumeration));
        Guard.FromInvalidInput(enumeration, nameof(enumeration));
        Guard.FromNegativeValues(enumeration, nameof(enumeration));

        var inputValue = Int32.Parse(enumeration.ToString());

        var enumerationFlagStates = new Dictionary<TKey, Boolean>(5);

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
                return new List<TKey>{key};
            }

            if (inputValue == -1 || enumeration.Equals(typedMaximumValue))
            {
                return inputKeys.Where(x => Int64.Parse(x.Value.ToString() ?? String.Empty) > 0);
            }

            AssignFlagStateValues(inputValue, currentEnumerationAsInt, key, enumerationFlagStates);
        }
        
        return inputValue > maximumKeyValueAllowed ? Enumerable.Empty<TKey>() : CreateKeysList(enumerationFlagStates);
    }

    private static Int32 CalculateHighestAllowedFlag(List<TKey> inputList)
    => GetHighestFlagValue(inputList) * 2 - 1;
    

    private static void CheckEnumerationForValuesLessThanNegativeOne(Int32 value)
    {
        if (value < -1)
        {
            var message = String.Format(Messages.EnumerationContainsNegativeValue, typeof(TKey).Name);

            throw new ArgumentValueWasNegativeException(message);
        }
    }

    private static IEnumerable<TKey> CreateKeysList(Dictionary<TKey, Boolean> enumerationFlagStates)
    => enumerationFlagStates
            .Where(entry => entry.Value)
            .Select(entry => entry.Key)
            .ToList() ?? Enumerable.Empty<TKey>();

    private static void AllowUnsafeFlags(IEnumerable<TKey> keys)
    {
        var attribute =
            (AllowUnsafeEnumerationKeysAttribute)Attribute.GetCustomAttribute(typeof(TKey),
                typeof(AllowUnsafeEnumerationKeysAttribute));

        if (attribute is null)
        {
            CheckProvidedEnumerationsForPowersOfTwo(keys);
        }
    }

    private static void AssignFlagStateValues(Int32 inputValue, Int32 currentEnumerationValue, TKey key, IDictionary<TKey, Boolean> enumerationFlagStates)
    {
        if(!enumerationFlagStates.ContainsKey(key) && currentEnumerationValue is not 0)
        {
            var flagState = (inputValue & currentEnumerationValue) == currentEnumerationValue;

            enumerationFlagStates.Add(key, flagState);
        }
    }

    private static void CheckProvidedEnumerationsForPowersOfTwo(IEnumerable<TKey> enumerations)
    {
        var enumerationsAsList = enumerations.ToList();
        var enumerationValues = Enumerable.Empty<Int32>().ToList();

        enumerationValues
            .AddRange(enumerationsAsList.Select(enumeration => Int32.Parse(enumeration.Value.ToString() ?? string.Empty)));

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

            var result = enumerationValues.BinarySearch(nextPower);

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
        => numberToCheck is not 0
               && (numberToCheck & (numberToCheck - 1)) is 0;

    private static Int32 GetHighestFlagValue(IReadOnlyList<TKey> keyList)
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

    private static TEnumeration GetMaximumValue()
    {
        var maximumField = typeof(TEnumeration).GetField("MaxValue", BindingFlags.Public | BindingFlags.Static);

        if (maximumField is null)
        {
            throw new NotSupportedException(typeof(TEnumeration).Name);
        }

        var maxValue = (TEnumeration)maximumField.GetValue(null);

        return maxValue;
    }
}
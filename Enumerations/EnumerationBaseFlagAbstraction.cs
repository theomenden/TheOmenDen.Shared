using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Guards;

namespace TheOmenDen.Shared.Enumerations;

public abstract record EnumerationBaseFlagAbstraction<TKey, TEnumeration>()
where TKey : EnumerationBase<TKey, TEnumeration>
where TEnumeration : class, IEquatable<TEnumeration>, IComparable<TEnumeration>
{
    protected static IEnumerable<TKey> GetFlagValues(TEnumeration enumeration, IEnumerable<TKey> allEnumerationKeys)
    {
        Guard.FromNull(enumeration, nameof(enumeration));
        Guard.FromInvalidInput(enumeration, nameof(enumeration));
        Guard.FromNegativeValues(enumeration, nameof(enumeration));

        var inputValue = Int32.Parse(enumeration.ToString());

        var enumFlagsState = new Dictionary<TKey, Boolean>(5);

        var inputKeys = allEnumerationKeys.ToList();

        AllowUnsafeFlags(inputKeys);

        var maximumKeyValueAllowed = CalculateHighestAllowedFlag(inputKeys);

        var typedMaximumValue = GetMaximumValue();

        foreach (var key in inputKeys)
        {
            var currentEnumerationAsInt = Int32.Parse(key.Value.ToString() ?? String.Empty);

            CheckEnumerationsForNegativeValues(currentEnumerationAsInt);

            if (currentEnumerationAsInt == inputValue)
            {
                return new List<TKey>{key};
            }

            if (inputValue == -1 || enumeration.Equals(typedMaximumValue))
            {
                return inputKeys.Where(x => Int64.Parse(x.Value.ToString() ?? String.Empty) >0);
            }

            
        }
        
        return Enumerable.Empty<TKey>();
    }

    private static Int32 CalculateHighestAllowedFlag(List<TKey> inputList)
    {
        return GetHighestFlagValue(inputList) * 2 - 1;
    }

    private static void CheckEnumerationsForNegativeValues(Int32 value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    private static IEnumerable<TKey> CreateKeysList(Dictionary<TKey, Boolean> enumerationFlagStates)
    {
        var output = Enumerable.Empty<TKey>().ToList();

        output.AddRange(enumerationFlagStates
            .Where(entry => entry.Value)
            .Select(entry => entry.Key)
        );

        return output.DefaultIfEmpty();
    }

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
                var message = String.Format("{0} result did not contain a value that was a power of 2 (two), {1}",
                    result, currentValue);

                throw new InvalidOperationException(message);
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
using TheOmenDen.Shared.Guards;

namespace TheOmenDen.Shared.Enumerations;

public abstract record EnumerationBaseFlagAbstraction<TKey, TEnumeration>()
where TKey: EnumerationBase<TKey, TEnumeration>
where TEnumeration : class, IEquatable<TEnumeration>, IComparable<TEnumeration>
{
    protected static IEnumerable<TKey> GetFlagValues(TEnumeration enumeration, IEnumerable<TKey> allEnumerationKeys)
    {
        Guard.FromNull(enumeration, nameof(enumeration));

        var inputValue = Int32.Parse(enumeration.ToString());

        var enumFlagsState = new Dictionary<TKey, Boolean>(5);

        var inputKeys = allEnumerationKeys.ToList();



        return Enumerable.Empty<TKey>();
    }
}
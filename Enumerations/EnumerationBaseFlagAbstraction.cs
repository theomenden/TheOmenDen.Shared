namespace TheOmenDen.Shared.Enumerations;

public abstract record EnumerationBaseFlagAbstraction<TKey, TEnumeration>()
where TKey: EnumerationBase<TKey, TEnumeration>
where TEnumeration : IEquatable<TEnumeration>, IComparable<TEnumeration>
{
}
namespace TheOmenDen.Shared.Enumerations.Attributes;

/// <summary>
/// Allows for -1 to be used as a "Key" for <see cref="EnumerationBase{TEnumKey, TEnumValue}"/> types
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AllowNegativeEnumerationKeysAttribute : Attribute
{
}
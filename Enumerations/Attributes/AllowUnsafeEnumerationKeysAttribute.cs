namespace TheOmenDen.Shared.Enumerations.Attributes;

/// <summary>
/// Allows for unsafe values to be used as "Keys" for <see cref="EnumerationBase{TEnumKey, TEnumValue}"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AllowUnsafeEnumerationKeysAttribute : Attribute
{
}
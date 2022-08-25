namespace TheOmenDen.Shared.Exceptions.Templates;
internal static class Messages
{
    /// <summary>
    /// Message for <see cref="ArgumentNullException"/> and <see cref="ArgumentNullException"/>
    /// </summary>
    /// <value>Provided [parameter] cannot be null or empty</value>
    public const string CannotBeNullOrEmpty = @"Provided [{0}] cannot be null or empty";
    /// <summary>
    /// Used when an Enumeration cannot be found with a supplied name
    /// </summary>
    /// <value>No Enumeration [EnumType] with Name [Name] could be found</value>
    public const string EnumerationNotFoundByName = @"No Enumeration [{0}] with Name [{1}] could be found";
    /// <summary>
    /// Used when an Enumeration cannot be found with a supplied value
    /// </summary>
    /// <value>"No Enumeration [EnumType] with Value [Value] could be found</value>
    public const string EnumerationNotFoundByValue = @"No Enumeration [{0}] with Value [{1}] could be found";
    /// <summary>
    /// Used when an Enumeration has a value that is below 0
    /// </summary>
    /// <value>The Enumeration [EnumType] contains negative values</value>
    /// <remarks> A value of (-1) can be allowed if overriden, see: <see cref="Enumerations.Attributes.AllowNegativeEnumerationKeysAttribute"/></remarks>
    public const string EnumerationContainsNegativeValue = @"The Enumeration [{0}] contains negative values";
    /// <summary>
    /// Used when an Enumeration has a value that cannot be parsed into a <see cref="Int32"/>
    /// </summary>
    /// <value>The value: [EnumType] could not be parsed into an integer.</value>
    public const string CouldNotParseToIntegerValue = @"The value: [{0}] could not be parsed into an integer.";
    /// <summary>
    /// Used when an Enumeration has powers inconsistent with the <see cref="EnumerationBaseFlag{TKey, TValue}"/> definition
    /// </summary>
    /// <value>The Enumeration [EnumType] has values that are not consistent powers of 2(two)</value>
    public const string EnumerationNotConsecutivePowerOfTwo = @"The Enumeration [{0}] has values that are not consistent powers of 2(two); current value: {1}";
}


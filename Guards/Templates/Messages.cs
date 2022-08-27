namespace TheOmenDen.Shared.Guards.Templates;

internal static class Messages
{
    /// <summary>
    /// Default parameter name
    /// </summary>
    /// <value>parameter</value>
    public const string BaseParameterName = "parameter";
    /// <summary>
    /// Default message for a value that is null that shouldn't be
    /// </summary>
    /// <value>[ParameterName] value cannot be Null</value>
    public const string NullValueTemplate = @"[{0}] value cannot be Null";
    /// <summary>
    /// Default message for when a precondition is not met
    /// </summary>
    /// <value>Precondition not met.</value>
    public const string PreconditionTemplate = "Precondition not met.";
    /// <summary>
    /// Default message for a collection being empty when it shouldn't be
    /// </summary>
    /// <value>[CollectionName] should contain at least one element.</value>
    public const string EmptyCollectionTemplate = @"[{0}] should contain at least one element.";
    /// <summary>
    /// Default message for a string that is null, or empty
    /// </summary>
    /// <value>[ParameterName] cannot be Null or Empty</value>
    public const string NotNullOrEmptyTemplate = @"[{0}] cannot be Null or Empty";
    /// <summary>
    /// Default message for a string that is null, whitespace, or empty
    /// </summary>
    /// <value>[ParameterName] cannot be Null, Whitespace, or Empty</value>
    public const string NotNullOrWhitespaceTemplate = @"[{0}] cannot be Null, Whitespace, or Empty";
    /// <summary>
    /// Default message for a value that doesn't match a regular expression
    /// </summary>
    /// <value>[ParameterName] does not match the pattern provided [value]</value>
    public const string NotMatchedByPatternTemplate = @"[{0}] does not match the pattern provided [{1}]";
    /// <summary>
    /// Default message for a value that falls beneath a threshold
    /// </summary>
    /// <value>[ParameterName] cannot be less than provided threshold [LowerThreshold].</value>
    public const string LessThanValueTemplate = @"[{0}] cannot be less than provided threshold {1}.";
    /// <summary>
    /// Default message for when a value is negative 
    /// </summary>
    public const string NegativeValueTemplate = @"[{0}] cannot be a negative value.";
    /// <summary>
    /// Default message for when a value is invalid
    /// </summary>
    /// <value>[TypeName] was supplied with an invalid value</value>
    public const string InvalidValueTemplate = @"[{0}] was supplied with an invalid value";
    /// <summary>
    /// Default message for when a value doesn't satisfy an equality
    /// </summary>
    /// <value>Equality was not met</value>
    public const string NotEqualToTemplate = @"Equality was not met";
    /// <summary>
    /// Default message for when a value is less than or equal to the lower boundary in a range 
    /// </summary>
    /// <value>[parameterName] cannot be at or below lower boundary [lowerBoundValue]</value>
    public const string AtLowerBoundTemplate = @"[{0}] cannot be at or below lower boundary [{1}]";
    /// <summary>
    /// Default message for when a value is greater than or equal to the upper boundary in a range 
    /// </summary>
    /// <value>[parameterName] cannot be at or above upper boundary [upperBoundValue]</value>
    public const string AtUpperBoundTemplate = @"[{0}] cannot be at or above upper boundary [{1}]";
    /// <summary>
    /// Default message for when a value is at either boundary in a range 
    /// </summary>
    /// <value>[parameterName] cannot be at either boundary [lowerBoundaryValue,upperBoundaryValue]</value>
    public const string AtBoundaryPointsTemplate = @"[{0}] cannot be at or outside either boundary [{1},{2}]";
    /// <summary>
    /// Default message for when a value is outside the boundaries for a range 
    /// </summary>
    /// <value>[parameterName] cannot be outside either boundary [lowerBoundaryValue,upperBoundaryValue]</value>
    public const string OutsideBoundaryPointsTemplate = @"[{0}] cannot be outside either boundary [{1},{2}]";
    /// <summary>
    /// Default message for when a value is within a range 
    /// </summary>
    /// <value>[parameterName] cannot be inside either boundary [lowerBoundaryValue, upperBoundaryValue]</value>
    public const string WithinBoundaryPointsTemplate = @"[{0}] cannot be inside either boundary [{1},{2}]";
}
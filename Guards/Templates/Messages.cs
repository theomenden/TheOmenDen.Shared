namespace TheOmenDen.Shared.Guards.Templates;

internal static class Messages
{
    /// <summary>
    /// 
    /// </summary>
    public const string BaseParameterName = "parameter";
    /// <summary>
    /// 
    /// </summary>
    public const string NullValueTemplate = @"[{0}] value cannot be Null";
    /// <summary>
    /// 
    /// </summary>
    public const string PreconditionTemplate = "Precondition not met.";
    /// <summary>
    /// 
    /// </summary>
    public const string EmptyCollectionTemplate = @"[{0}] should contain at least one element.";
    /// <summary>
    /// 
    /// </summary>
    public const string NotNullOrEmptyTemplate = @"[{0}] cannot be Null or Empty";
    /// <summary>
    /// 
    /// </summary>
    public const string NotNullOrWhitespaceTemplate = @"[{0}] cannot be Null, Whitespace, or Empty";
    /// <summary>
    /// 
    /// </summary>
    public const string NotMatchedByPatternTemplate = @"[{0}] does not match the pattern provided [{1}]";
    /// <summary>
    /// 
    /// </summary>
    public const string LessThanValueTemplate = @"[{0}] cannot be less than provided threshold {1}.";
    /// <summary>
    /// 
    /// </summary>
    public const string NegativeValueTemplate = @"[{0}] cannot be a negative value.";
    /// <summary>
    /// 
    /// </summary>
    /// <value>[TypeName] was supplied with an invalid value</value>
    public const string InvalidValueTemplate = @"[{0}] was supplied with an invalid value";
    /// <summary>
    /// 
    /// </summary>
    /// <value>Equality was not met</value>
    public const string NotEqualToTemplate = @"Equality was not met";
}
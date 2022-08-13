namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// A set of suggested codes that indicate various successful operations
/// </summary>
/// <remarks><inheritdoc cref="EnumerationBase"/></remarks>
public sealed record SuccessCode: EnumerationBase
{
    private SuccessCode(String name, Int32 id)
        : base(name, id)
    {}

    /// <summary>
    /// A code indicating that an entity was successfully modified 
    /// </summary>
    public static SuccessCode ModifiedEntitySuccess = new(nameof(ModifiedEntitySuccess), 1);
    /// <summary>
    /// A code indicating that a validation condition was passed
    /// </summary>
    public static SuccessCode ValidationSuccess = new(nameof(ValidationSuccess), 2);
    /// <summary>
    /// A code indicating that data was successfully synchronized across the domain
    /// </summary>
    public static SuccessCode SynchronizationSuccess = new(nameof(SynchronizationSuccess), 3);
}


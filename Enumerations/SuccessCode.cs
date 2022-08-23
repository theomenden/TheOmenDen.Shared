namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// A set of suggested codes that indicate various successful operations
/// </summary>
/// <remarks><inheritdoc cref="EnumerationBase{TKey}"/></remarks>
public sealed record SuccessCode: EnumerationBase<SuccessCode>
{
    private SuccessCode(String name, Int32 id)
        : base(name, id)
    {}

    /// <summary>
    /// A code indicating that an entity was successfully modified 
    /// </summary>
    public static readonly SuccessCode ModifiedEntitySuccess = new(nameof(ModifiedEntitySuccess), 1);
    /// <summary>
    /// A code indicating that a validation condition was passed
    /// </summary>
    public static readonly SuccessCode ValidationSuccess = new(nameof(ValidationSuccess), 2);
    /// <summary>
    /// A code indicating that data was successfully synchronized across the domain
    /// </summary>
    public static readonly SuccessCode SynchronizationSuccess = new(nameof(SynchronizationSuccess), 3);
}


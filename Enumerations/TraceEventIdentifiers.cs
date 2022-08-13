namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// Contains logging event identifiers for tracing application flow
/// </summary>
public sealed record TraceEventIdentifiers: EnumerationBase
{
    private TraceEventIdentifiers(String name, Int32 id)
        : base(name, id)
    {
    }

    /// <summary>
    /// Represents a profiling of a particular point of the application
    /// </summary>
    public static readonly TraceEventIdentifiers ProfileMessagingTrace = new(nameof(ProfileMessagingTrace),50);
    
    /// <summary>
    /// Occurs before any validation
    /// </summary>
    public static readonly TraceEventIdentifiers BeforeValidatingMessageTrace = new (nameof(BeforeValidatingMessageTrace),51);
    
    /// <summary>
    /// Invalid general escape message
    /// </summary>
    public static readonly TraceEventIdentifiers InvalidMessageTrace = new (nameof(InvalidMessageTrace),52);
    
    /// <summary>
    /// Validation passed
    /// </summary>
    public static readonly TraceEventIdentifiers ValidMessageTrace = new (nameof(ValidMessageTrace),53);
    
    /// <summary>
    /// Indication for a model binder being used
    /// </summary>
    public static readonly TraceEventIdentifiers ModelBinderUsedTrace = new (nameof(ModelBinderUsedTrace),54);
}

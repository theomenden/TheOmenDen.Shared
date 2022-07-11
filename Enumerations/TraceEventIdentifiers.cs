namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// Contains logging event identifiers for tracing application flow
/// </summary>
public enum TraceEventIdentifiers
{
    /// <value>
    /// Represents a profiling of a particular point of the application
    /// </value>
    ProfileMessagingTrace = 50,
    /// <value>
    /// Occurs before any validation
    /// </value>
    BeforeValidatingMessageTrace = 51,
    /// <value>
    /// Invalid general escape message
    /// </value>
    InvalidMessageTrace = 52,
    /// <value>
    /// Validation passed
    /// </value>
    ValidMessageTrace = 53,
    /// <value>
    /// Indication for a model binder being used
    /// </value>
    ModelBinderUsedTrace = 54
}

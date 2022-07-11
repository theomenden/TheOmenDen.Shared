namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// Contains the various types of logging events for errors.
/// </summary>
public enum EventIdIdentifier
{
    /// <value>
    /// Thrown by the application
    /// </value>
    AppThrown = 1,
    /// <value>
    /// Uncaught during a particular action
    /// </value>
    UncaughtInAction = 2,
    /// <value>
    /// Uncaught in the application
    /// </value>
    UncaughtGlobal = 3,
    /// <value>
    /// Thrown from inside a pipeline
    /// </value>
    PipelineThrown = 4,
    /// <value>
    /// Thrown by an HTTP Client
    /// </value>
    HttpClient = 5
}
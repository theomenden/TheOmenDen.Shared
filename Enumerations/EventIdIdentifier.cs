namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// Contains the various types of logging events for errors.
/// </summary>
public sealed record EventIdIdentifier: EnumerationBase
{
    private EventIdIdentifier(String name, Int32 id)
        :base(name, id)
    {
    }

    /// <value>
    /// Thrown by the application
    /// </value>
    public static readonly EventIdIdentifier AppThrown = new(nameof(AppThrown), 1);

    /// <value>
    /// Uncaught during a particular action
    /// </value>
    public static readonly EventIdIdentifier UncaughtInAction = new(nameof(AppThrown), 2);

    /// <value>
    /// Uncaught in the application
    /// </value>
    public static readonly EventIdIdentifier UncaughtGlobal = new(nameof(AppThrown), 3);

    /// <value>
    /// Thrown from inside a pipeline
    /// </value>
    public static readonly EventIdIdentifier PipelineThrown = new(nameof(AppThrown), 4);

    /// <value>
    /// Thrown by an HTTP Client
    /// </value>
    public static readonly EventIdIdentifier HttpClient = new(nameof(AppThrown), 5);
}
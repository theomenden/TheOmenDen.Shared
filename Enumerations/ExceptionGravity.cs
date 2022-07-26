namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// Defines the severity of a particular exception
/// </summary>
public record ExceptionGravity : EnumerationBase
{
    private ExceptionGravity(String name, Int32 id)
        : base(name, id)
    {
    }

    /// <summary>
    /// Standard Error for the application, typically non-crashing
    /// </summary>
    public static readonly ExceptionGravity Error = new(nameof(Error), 1);

    /// <summary>
    /// A crashing level error
    /// </summary>
    public static readonly ExceptionGravity Critical = new(nameof(Critical), 2);
}
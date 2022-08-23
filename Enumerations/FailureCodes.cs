namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// <para>A suggested set of templates for describing various application failures</para>
/// </summary>
/// <remarks><inheritdoc cref="EnumerationBase{TKey}"/></remarks>
public sealed record FailureCode : EnumerationBase<FailureCode>
{
    private FailureCode(string name, int id) 
        : base(name, id)
    {
    }

    /// <summary>
    /// An error that has occurred during a process within the domain
    /// </summary>
    public static readonly FailureCode ProcessingError = new(nameof(ProcessingError), 1);
    /// <summary>
    /// An error that has occurred during a transmission protocol
    /// </summary>
    public static readonly FailureCode TransmissionError = new(nameof(TransmissionError), 2);
    /// <summary>
    /// An error that has occurred from a database interaction
    /// </summary>
    public static readonly FailureCode DatabaseError = new(nameof(DatabaseError), 3);
    /// <summary>
    /// An error that the application doesn't know how to handle internally
    /// </summary>
    public static readonly FailureCode UnknownError = new(nameof(UnknownError), 5);
    /// <summary>
    /// An error that occurred across an application's domain
    /// </summary>
    public static readonly FailureCode GlobalError = new(nameof(GlobalError), 8);
}
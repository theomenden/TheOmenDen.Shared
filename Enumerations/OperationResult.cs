namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// Indicates the outcome of a particular operation
/// </summary>
public sealed record OperationResult : EnumerationBase<OperationResult>
{
    private OperationResult(String name, Int32 id)
    : base(name, id)
    {}

    /// <value>
    /// The operation succeeded
    /// </value>
    public static readonly OperationResult Success = new(nameof(Success), 0);

    /// <value>
    /// The operation returned a failure status
    /// </value>
    public static readonly OperationResult Failure = new(nameof(Failure), 1);
}

namespace TheOmenDen.Shared.Exceptions;

/// <summary>
/// Contains relevant <see cref="OperationOutcome"/> aggregation over threads
/// </summary>
public sealed class OperationOutcomeAggregateException: AggregateException
{

    public static OperationOutcomeAggregateException Create(IAsyncEnumerable<OperationOutcome> operationOutcomes)
    {
        return new();
    }

    public static OperationOutcomeAggregateException Create(IEnumerable<OperationOutcome> operationOutcomes)
    {
        return new();
    }


}


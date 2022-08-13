using TheOmenDen.Shared.Responses.Templates;

namespace TheOmenDen.Shared.Responses;

/// <summary>
/// Definitions for various cross-app operations
/// </summary>
public sealed class OperationOutcome
{
    public OperationOutcome()
    {
        ClientErrorPayload = new ();
        CorrelationId = String.Empty;
        Errors = Enumerable.Empty<String>();
    }

    /// <summary>
    /// <inheritdoc cref="Enumerations.OperationResult"/>
    /// </summary>
    public OperationResult OperationResult { get; set; } = OperationResult.Success; // we love optimism 😅

    /// <summary>
    /// The correlation id for us to search for this outcome
    /// </summary>
    public String CorrelationId { get; set; }

    /// <summary>
    /// The errors that lead to the outcome represented as a collection of strings
    /// </summary>
    public IEnumerable<String> Errors { get; set; }

    /// <summary>
    /// <inheritdoc cref="ClientErrorPayload"/>
    /// </summary>
    public ClientErrorPayload ClientErrorPayload { get; set; }

    /// <summary>
    /// Successful outcome indicator
    /// </summary>
    public static OperationOutcome SuccessfulOutcome => new()
    {
        Errors= Enumerable.Empty<String>(),
        CorrelationId = String.Empty,
        ClientErrorPayload = new (){
        IsError = false,
        IsValidationFailure = false,
        Message = String.Empty,
        Detail = String.Empty
        },
        OperationResult = OperationResult.Success
    };

    /// <summary>
    /// Unsuccessful Outcome indicator
    /// </summary>
    public static OperationOutcome UnsuccessfulOutcome => new()
    {
        Errors = Enumerable.Empty<String>(),
        CorrelationId = String.Empty,
        ClientErrorPayload = new()
        {
            IsError = true,
            IsValidationFailure = false,
            Message = MessageTemplates.DefaultLog,
            Detail = String.Empty
        },
        OperationResult = OperationResult.Failure
    };

    /// <summary>
    /// Validation failures indicator
    /// </summary>
    /// <param name="errors">Validation errors</param>
    /// <param name="message">A simple message indicating basic failure conditions</param>
    /// <returns><see cref="OperationOutcome"/></returns>
    public static OperationOutcome ValidationFailureOutcome(IEnumerable<String> errors, String message = null) => new()
    {
        Errors = errors ?? Enumerable.Empty<String>(),
        CorrelationId = String.Empty,
        ClientErrorPayload = new()
        {
            IsError = false,
            IsValidationFailure = true,
            Message = message ?? String.Empty,
            Detail = String.Empty
        },
        OperationResult = OperationResult.Failure
    };
}


namespace TheOmenDen.Shared.Responses;
/// <summary>
/// Information relevant to our processing of Apis throughout the app
/// </summary>
/// <remarks>See the <see cref="ApiResponseContext"/> for serialization information</remarks>
public class ApiResponse
{
    /// <summary>
    /// Indicates if the resulting operation was an Error, or a success
    /// </summary>
    /// <value>
    /// <see cref="OperationOutcome"/>
    /// </value>
    /// <remarks>Defaults to a <see cref="OperationOutcome.SuccessfulOutcome"/> because we want to be optimistic 😉</remarks>
    public OperationOutcome Outcome { get; set; } = OperationOutcome.SuccessfulOutcome;

    /// <summary>
    /// Status code belonging to a particular response
    /// </summary>
    public Int32 StatusCode { get; set; }
}

/// <summary>
/// Generic wrapper for <inheritdoc cref="ApiResponse"/>
/// </summary>
/// <typeparam name="T">The type returned by a successful outcome</typeparam>
public class ApiResponse<T> : ApiResponse
{
    /// <summary>
    /// The provided data in a response
    /// </summary>
    public T Data { get; set; }
}

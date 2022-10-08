namespace TheOmenDen.Shared.Responses;

/// <summary>
/// A simplified response that is returned to the client
/// </summary>
public class ClientErrorPayload
{
    /// <summary>
    /// A human readable message
    /// </summary>
    /// <value>
    /// Typically provided from an exception, or known error
    /// </value>
    public string Message { get; set; } = String.Empty;

    /// <summary>
    /// The error is not a validation error1
    /// </summary>
    /// <value>
    /// <c>True</c> when not caused by validation, <c>False</c> otherwise
    /// </value>
    public Boolean IsError { get; set; }

    /// <summary>
    /// Indicates that the failure was by business logic/validation
    /// </summary>
    /// <value>
    /// <c>True</c> when caused by validation, <c>False</c> otherwise
    /// </value>
    public Boolean IsValidationFailure { get; set; }

    /// <summary>
    /// Provides more insight into what went wrong and why we delivered this payload
    /// </summary>
    /// <value>
    /// Typically populated by a StackTrace, or some sort of caller traversal 
    /// </value>
    public String Detail { get; set; } = String.Empty;
}

/// <summary>
/// Generic wrapper for <see cref="ClientErrorPayload"/>
/// </summary>
/// <typeparam name="T">Type of data contained by the payload</typeparam>
public class ClientErrorPayload<T>: ClientErrorPayload
{
    /// <summary>
    /// The data within the error payload
    /// </summary>
    /// <value>
    /// <typeparamref name="T"/>
    /// </value>
    public T Data { get; set; }
}


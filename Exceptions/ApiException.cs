using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;
#nullable enable
/// <summary>
/// Provides a way to describe issues involving APIs, both external and internal 
/// <inheritdoc cref="Exception"/>
/// </summary>
[Serializable]
public sealed class ApiException : Exception
{
    public ApiException(string message)
        : base(message) { }

    public ApiException(string message, Exception innerException)
        : base(message, innerException) { }

    private ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ApiException(string message, ExceptionGravity? exceptionGravity, int statusCode = 500)
        : base(message)
    {
        ExceptionGravity = exceptionGravity ?? ExceptionGravity.Error;
        StatusCode = statusCode;
    }

    public ApiException(string message, Exception innerException, ExceptionGravity? exceptionGravity, int statusCode = 500)
        : base(message, innerException)
    {
        ExceptionGravity = exceptionGravity ?? ExceptionGravity.Error;
        StatusCode = statusCode;
    }

    /// <summary>
    /// Represents the general severity of an exception 
    /// </summary>
    /// <value>
    /// <see cref="ExceptionGravity"/>
    /// </value>
    public ExceptionGravity ExceptionGravity { get; set; } = ExceptionGravity.Error;

    /// <summary>
    /// A human readable status code
    /// </summary>
    /// <value>
    /// Integer value, typically set by the <see cref="HttpResponseMessage"/>
    /// </value>
    public int StatusCode { get; set; }
}


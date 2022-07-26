namespace TheOmenDen.Shared.Responses.Templates;

/// <summary>
/// Templates for logging statements throughout the application
/// </summary>
public static class MessageTemplates
{
    /// <summary>
    /// Standard error log statement with correlation id
    /// </summary>
    /// <value>
    /// {errorMessage} -- {correlationId}
    /// </value>
    public const string DefaultLog = "{errorMessage} -- {correlationId}";

    /// <summary>
    /// HttpClient logging statement for GET requests
    /// </summary>
    /// <value>
    /// GET request to {@absolutePath}
    /// </value>
    public const string HttpClientGet = "GET request to {@absolutePath}";

    /// <summary>
    /// Uncaught exception logging statement, with correlation id
    /// </summary>
    /// <value>
    /// Uncaught Exception. {resolvedExceptionMessage} -- {correlationId}
    /// </value>
    public const string UncaughtGlobal = "Uncaught Exception. {resolvedExceptionMessage} -- {correlationId}";

    /// <summary>
    /// Validation failure logging statement
    /// </summary>
    /// <value>
    /// Validation Fail. {errors}. User Id {userName}.
    /// </value>
    public const string ValidationErrorsLog = "Validation Fail. {errors}. User Id {userName}.";
}


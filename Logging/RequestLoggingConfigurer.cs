using Microsoft.AspNetCore.Http;

namespace TheOmenDen.Shared.Logging;
public static class RequestLoggingConfigurer
{
    /// <summary>
    /// Tells the provided <see cref="IDiagnosticContext"/> to log properties from the provided <seealso cref="HttpContext" />
    /// </summary>
    /// <param name="diagnosticContext"><inheritdoc cref="IDiagnosticContext"/></param>
    /// <param name="context"><inheritdoc cref="HttpContext"/></param>
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext context)
    {
        var request = context.Request;

        diagnosticContext.Set("Host", request.Host);
        diagnosticContext.Set("Protocol", request.Protocol);
        diagnosticContext.Set("Scheme", request.Scheme);

        if (request.QueryString.HasValue)
        {
            diagnosticContext.Set("QueryString", request.QueryString.Value);
        }

        diagnosticContext.Set("ContentType", context.Response.ContentType);

        var endpoint = context.GetEndpoint();

        if (endpoint is not null)
        {
            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
        }
    }
}

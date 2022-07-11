namespace TheOmenDen.Shared.Configuration;

/// <summary>
/// Configuration class for injected <see cref="IHttpClientFactory"/> 
/// </summary>
public sealed class HttpClientConfiguration
{
    /// <summary>
    /// The name of the client we're trying to register
    /// </summary>
    public String Name { get; set; }

    /// <summary>
    /// The client's base address
    /// </summary>
    public String BaseAddress { get; set; }
}


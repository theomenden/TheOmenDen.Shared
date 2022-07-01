namespace TheOmenDen.Shared.Configuration;

/// <summary>
/// Configuration class for injected <see cref="IHttpClientFactory"/> 
/// </summary>
public sealed class HttpClientConfiguration
{
    public String Name { get; set; }

    public String BaseAddress { get; set; }
}


namespace TheOmenDen.Shared.Infrastructure;

/// <summary>
/// <inheritdoc cref="IApiService{T}"/>
/// </summary>
/// <typeparam name="TResponse">The type we expect to retrieve from the API</typeparam>
public abstract class ApiServiceBase<TResponse> : IApiService<TResponse>
{
    protected readonly IHttpClientFactory ClientFactory;
    protected readonly HttpClientConfiguration HttpClientConfiguration;

    protected ApiServiceBase(IHttpClientFactory clientFactory, IOptions<HttpClientConfiguration> options)
    {
        ClientFactory = clientFactory;

        HttpClientConfiguration = options.Value;
    }

    public virtual async Task<ApiResponse<TResponse>> GetContentAsync(String uri, CancellationToken cancellationToken = default)
    {
        using var client = ClientFactory.CreateClient(HttpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var statusCode = (int)response.StatusCode;

        return new()
        {
            Data = await DeserializeFromStreamAsync<TResponse>(stream, cancellationToken),
            StatusCode = statusCode,
            Outcome = response.IsSuccessStatusCode ? OperationOutcome.SuccessfulOutcome : OperationOutcome.UnsuccessfulOutcome,
        };
    }

    public virtual async Task<ApiResponse<IEnumerable<TResponse>>> GetContentStreamAsync(String uri, CancellationToken cancellationToken = default)
    {
        using var client = ClientFactory.CreateClient(HttpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        return new()
        {
            Data = await DeserializeFromStreamAsync<IEnumerable<TResponse>>(stream, cancellationToken),
            StatusCode = statusCode,
            Outcome = response.IsSuccessStatusCode ? OperationOutcome.SuccessfulOutcome : OperationOutcome.UnsuccessfulOutcome,
        };
    }

    public virtual async Task<ApiResponse<String>> PostContentAsync(String uri, TResponse body, CancellationToken cancellationToken = default)
    {
        using var client = ClientFactory.CreateClient(HttpClientConfiguration.Name);

        var payload = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{client.BaseAddress}{uri}");

        request.Content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

        using var httpResponse = await client.SendAsync(request, cancellationToken);

        var statusCode = (int)httpResponse.StatusCode;

        await using var content = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

        return new()
        {
            Data = await DeserializeStreamToStringAsync(content),
            Outcome = httpResponse.IsSuccessStatusCode ? OperationOutcome.SuccessfulOutcome : OperationOutcome.UnsuccessfulOutcome,
            StatusCode = statusCode
        };
    }

    public virtual async Task<ApiResponse<String>> DeleteContentAsync(String uri,
        CancellationToken cancellationToken = default)
    {
        using var client = ClientFactory.CreateClient(HttpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{client.BaseAddress}{uri}");

        using var httpResponse = await client.SendAsync(request, cancellationToken);

        var statusCode = (int)httpResponse.StatusCode;

        await using var content = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

        return new()
        {
            Data = await DeserializeStreamToStringAsync(content),
            Outcome = httpResponse.IsSuccessStatusCode ? OperationOutcome.SuccessfulOutcome : OperationOutcome.UnsuccessfulOutcome,
            StatusCode = statusCode
        };
    }

    protected virtual async Task<TDeserialize> DeserializeFromStreamAsync<TDeserialize>(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null || stream.CanRead is false)
        {
            return default;
        }

        var searchResult = await JsonSerializer.DeserializeAsync<TDeserialize>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);

        return searchResult;
    }

    protected virtual async Task<String> DeserializeStreamToStringAsync(Stream stream)
    {
        var content = String.Empty;

        if (stream is null)
        {
            return content;
        }

        using var sr = new StreamReader(stream);

        content = await sr.ReadToEndAsync();

        return content;
    }
}

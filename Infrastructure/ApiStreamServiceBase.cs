namespace TheOmenDen.Shared.Infrastructure;

/// <summary>
/// <inheritdoc cref="IApiStreamService{T}"/>
/// </summary>
/// <typeparam name="TStreamResponse">The type we expect to stream from the API</typeparam>
public abstract class ApiStreamServiceBase<TStreamResponse>: IApiStreamService<TStreamResponse>
{
    protected readonly IHttpClientFactory HttpClientFactory;
    protected readonly HttpClientConfiguration HttpClientConfiguration;

    protected ApiStreamServiceBase(IHttpClientFactory clientFactory, IOptions<HttpClientConfiguration> httpClientConfiguration)
    {
        HttpClientFactory = clientFactory;
        HttpClientConfiguration = httpClientConfiguration.Value;
    }

    public virtual async IAsyncEnumerable<ApiResponse<TStreamResponse>> StreamApiResultsAsync(string uri, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
    {
        using var client = HttpClientFactory.CreateClient(HttpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        if (!response.IsSuccessStatusCode)
        {
            yield break;
        }

        await foreach (var item in DeserializeAsAsyncStream<TStreamResponse>(stream, cancellationToken))
        {
            yield return new()
            {
                Data = item,
                StatusCode = statusCode,
                Outcome = OperationOutcome.SuccessfulOutcome
            };
        }
    }

    public virtual async Task<ApiResponse<IEnumerable<TStreamResponse>>> EnumerateApiResultsAsync(string uri, CancellationToken cancellationToken = new CancellationToken())
    {
        using var client = HttpClientFactory.CreateClient(HttpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        switch (response.IsSuccessStatusCode)
        {
            case true:
            {
                var responseContent = await DeserializeAsAsyncStream<TStreamResponse>(stream, cancellationToken).ToArrayAsync(cancellationToken);

                return new()
                {
                    Data = responseContent,
                    Outcome = OperationOutcome.SuccessfulOutcome,
                    StatusCode = statusCode
                };
            }
            default:
                return new()
                {
                    Outcome = OperationOutcome.UnsuccessfulOutcome,
                    StatusCode = statusCode
                };
        }
    }

    protected virtual IAsyncEnumerable<TDeserialize> DeserializeAsAsyncStream<TDeserialize>(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null || stream.CanRead is false)
        {
            return AsyncEnumerable.Empty<TDeserialize>();
        }

        return JsonSerializer.DeserializeAsyncEnumerable<TDeserialize>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
    }
}


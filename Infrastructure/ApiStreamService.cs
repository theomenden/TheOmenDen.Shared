using TheOmenDen.Shared.Responses;
using TheOmenDen.Shared.Services;

namespace TheOmenDen.Shared.Infrastructure;

internal sealed class ApiStreamService: IApiStreamService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiStreamService(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }

    public async IAsyncEnumerable<ApiResponse<T>> StreamApiResultsAsync<T>(string uri, [EnumeratorCancellation] CancellationToken cancellationToken = new CancellationToken())
    {
        var apiResponse = new ApiResponse<IAsyncEnumerable<T>>();

        using var client = _httpClientFactory.CreateClient();

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            await foreach (var item in DeserializeAsAsyncStream<T>(stream, cancellationToken))
            {
                yield return new()
                {
                    Data = item,
                    StatusCode = statusCode,
                    Outcome = OperationOutcome.SuccessfulOutcome
                };
            }
        }
    }

    public async Task<ApiResponse<IEnumerable<T>>> EnumerateApiResultsAsync<T>(string uri, CancellationToken cancellationToken = new CancellationToken())
    {
        using var client = _httpClientFactory.CreateClient();

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await DeserializeAsAsyncStream<T>(stream, cancellationToken).ToArrayAsync(cancellationToken);

            return new()
            {
                Data = responseContent,
                Outcome = OperationOutcome.SuccessfulOutcome,
                StatusCode = statusCode
            };
        }

        return new()
        {
            Outcome = OperationOutcome.UnsuccessfulOutcome,
            StatusCode = statusCode
        };
    }

    private static IAsyncEnumerable<T> DeserializeAsAsyncStream<T>(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null || stream.CanRead is false)
        {
            return AsyncEnumerable.Empty<T>();
        }

        return JsonSerializer.DeserializeAsyncEnumerable<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
    }
}


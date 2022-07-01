using TheOmenDen.Shared.Responses;
using TheOmenDen.Shared.Services;

namespace TheOmenDen.Shared.Infrastructure;

internal sealed class ApiService : IApiService
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client ?? throw new ArgumentException(nameof(client));
    }

    public async Task<ApiResponse<T>> GetContentAsync<T>(String uri, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_client.BaseAddress}{uri}");

        using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var statusCode = (int)response.StatusCode;
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await DeserializeFromStreamAsync<T>(stream, cancellationToken);

            return new ()
            {
                Data = await DeserializeFromStreamAsync<T>(stream, cancellationToken),
                StatusCode = statusCode,
                Outcome = OperationOutcome.SuccessfulOutcome
            };
        }

        return new ()
        {
            Data = default,
            StatusCode = statusCode,
            Outcome = OperationOutcome.UnsuccessfulOutcome
        };
    }

    public async Task<ApiResponse<IEnumerable<T>>> GetContentStreamAsync<T>(String uri, CancellationToken cancellationToken = new CancellationToken())
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_client.BaseAddress}{uri}");
        
        using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await DeserializeFromStreamAsync<T>(stream, cancellationToken);

            return new()
            {
                Data = await DeserializeFromStreamAsync<IEnumerable<T>>(stream, cancellationToken),
                StatusCode = statusCode,
                Outcome = OperationOutcome.SuccessfulOutcome
            };
        }

        return new()
        {
            Data = default,
            StatusCode = statusCode,
            Outcome = OperationOutcome.UnsuccessfulOutcome
        };
    }

    public async Task<HttpResponseMessage> PostContentAsync<T>(String uri, T body, CancellationToken cancellationToken = new CancellationToken())
    {
        var payload = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_client.BaseAddress}{uri}");

        request.Content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

        using var httpResponse = await _client.SendAsync(request, cancellationToken);

        return httpResponse;
    }

    private static async Task<T> DeserializeFromStreamAsync<T>(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null || stream.CanRead is false)
        {
            return default;
        }

        var searchResult = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);

        return searchResult;
    }

    private static async Task<String> DeserializeStreamToStringAsync(Stream stream)
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

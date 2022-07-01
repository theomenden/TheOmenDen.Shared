using Microsoft.Extensions.Options;
using TheOmenDen.Shared.Configuration;
using TheOmenDen.Shared.Responses;
using TheOmenDen.Shared.Services;

namespace TheOmenDen.Shared.Infrastructure;

internal sealed class ApiService<T> : IApiService<T>
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClientConfiguration _httpClientConfiguration;

    public ApiService(IHttpClientFactory clientFactory, IOptions<HttpClientConfiguration> options)
    {
        _clientFactory = clientFactory;

        _httpClientConfiguration = options.Value;
    }

    public async Task<ApiResponse<T>> GetContentAsync(String uri, CancellationToken cancellationToken = default)
    {
        using var client = _clientFactory.CreateClient(_httpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var statusCode = (int)response.StatusCode;
        
        if (response.IsSuccessStatusCode)
        {
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

    public async Task<ApiResponse<IEnumerable<T>>> GetContentStreamAsync(String uri, CancellationToken cancellationToken = new CancellationToken())
    {
        using var client = _clientFactory.CreateClient(_httpClientConfiguration.Name);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}{uri}");
        
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var statusCode = (int)response.StatusCode;

        if (response.IsSuccessStatusCode)
        {
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

    public async Task<HttpResponseMessage> PostContentAsync(String uri, T body, CancellationToken cancellationToken = new CancellationToken())
    {
        using var client = _clientFactory.CreateClient(_httpClientConfiguration.Name);

        var payload = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{client.BaseAddress}{uri}");

        request.Content = new StringContent(payload, Encoding.UTF8, MediaTypeNames.Application.Json);

        using var httpResponse = await client.SendAsync(request, cancellationToken);

        return httpResponse;
    }

    private static async Task<TDeserialize> DeserializeFromStreamAsync<TDeserialize>(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is null || stream.CanRead is false)
        {
            return default;
        }

        var searchResult = await JsonSerializer.DeserializeAsync<TDeserialize>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);

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

using TheOmenDen.Shared.Responses;

namespace TheOmenDen.Shared.Services;
/// <summary>
/// 
/// </summary>
public interface IApiService
{
    Task<ApiResponse<T>> GetContentAsync<T>(String uri, CancellationToken cancellationToken = new());

    Task<ApiResponse<IEnumerable<T>>> GetContentStreamAsync<T>(String uri, CancellationToken cancellationToken = new CancellationToken());

    Task<HttpResponseMessage> PostContentAsync<T>(String uri, T body,
        CancellationToken cancellationToken = new CancellationToken());
}

using TheOmenDen.Shared.Responses;

namespace TheOmenDen.Shared.Services;

public interface IApiService
{

}

/// <summary>
/// 
/// </summary>
public interface IApiService<T>: IApiService
{
    Task<ApiResponse<T>> GetContentAsync(String uri, CancellationToken cancellationToken = new());

    Task<ApiResponse<IEnumerable<T>>> GetContentStreamAsync(String uri, CancellationToken cancellationToken = new CancellationToken());

    Task<HttpResponseMessage> PostContentAsync(String uri, T body,
        CancellationToken cancellationToken = new CancellationToken());
}

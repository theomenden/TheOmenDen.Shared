using TheOmenDen.Shared.Responses;

namespace TheOmenDen.Shared.Services;

public interface IApiStreamService
{
    IAsyncEnumerable<ApiResponse<T>> StreamApiResultsAsync<T>(String uri, CancellationToken cancellationToken = new());
    
    Task<ApiResponse<IEnumerable<T>>> EnumerateApiResultsAsync<T>(String uri, CancellationToken cancellationToken = new());
}


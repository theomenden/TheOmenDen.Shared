using TheOmenDen.Shared.Responses;

namespace TheOmenDen.Shared.Services;

public interface IApiStreamService
{

}

public interface IApiStreamService<T>: IApiStreamService
{
    IAsyncEnumerable<ApiResponse<T>> StreamApiResultsAsync(String uri, CancellationToken cancellationToken = new());
    
    Task<ApiResponse<IEnumerable<T>>> EnumerateApiResultsAsync(String uri, CancellationToken cancellationToken = new());
}


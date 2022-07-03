using TheOmenDen.Shared.Responses;

namespace TheOmenDen.Shared.Services;

/// <summary>
/// Marker interface for DI containers
/// </summary>
public interface IApiStreamService
{
}

/// <summary>
/// Contains Methods for streaming data back from an API that supports <see cref="IAsyncEnumerable{T}"/>, or long-running tasks
/// </summary>
/// <typeparam name="T">The type we expect to stream from the API</typeparam>
/// <remarks>Defines only <see cref="HttpMethod.Get"/> based methods</remarks>
public interface IApiStreamService<T>: IApiStreamService
{
    /// <summary>
    /// Streams the results back from the API and allows for individual responses to fail
    /// </summary>
    /// <param name="uri">The endpoint where the content we want to stream is located</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An asynchronous stream <see cref="IAsyncEnumerable{T}"/> of <see cref="ApiResponse{T}"/></returns>
    IAsyncEnumerable<ApiResponse<T>> StreamApiResultsAsync(String uri, CancellationToken cancellationToken = new());
    
    /// <summary>
    /// Retrieves all the content from an API before the headers are read completely
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>An <see cref="ApiResponse{T}"/> of <see cref="IEnumerable{T}"/> that can all fail or succeed</returns>
    Task<ApiResponse<IEnumerable<T>>> EnumerateApiResultsAsync(String uri, CancellationToken cancellationToken = new());
}


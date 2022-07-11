using TheOmenDen.Shared.Responses;

namespace TheOmenDen.Shared.Services;

/// <summary>
/// Marker Interface for DI registrations
/// </summary>
public interface IApiService
{

}


/// <summary>
/// Contains methods that involve CRUD operations via an external API
/// </summary>
/// <typeparam name="T">The type that is being modified by the API actions</typeparam>
/// <remarks>Defines all <see cref="HttpMethod"/> based methods, using <see cref="HttpClient.SendAsync(HttpRequestMessage)"/> under the hood</remarks>
public interface IApiService<T>: IApiService
{
    /// <summary>
    /// Retrieves content from the endpoint referenced by the given <paramref name="uri"/>
    /// </summary>
    /// <param name="uri">The relative path pointing to the content we want to retrieve</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An <see cref="ApiResponse{T}"/> indicating success or failure</returns>
    Task<ApiResponse<T>> GetContentAsync(String uri, CancellationToken cancellationToken = new());

    /// <summary>
    /// Retrieves content from the endpoint referenced by the given <paramref name="uri"/> as the content comes in (before response content is read completely)
    /// </summary>
    /// <param name="uri">The relative path pointing to the content we want to retrieve</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ApiResponse{T}"/> indicating success or failure</returns>
    Task<ApiResponse<IEnumerable<T>>> GetContentStreamAsync(String uri, CancellationToken cancellationToken = new ());

    /// <summary>
    /// Initiates a <see cref="HttpMethod.Post"/> request to a particular endpoint provided by <paramref name="uri"/>, with the 
    /// </summary>
    /// <param name="uri">The endpoint we're aiming to hit</param>
    /// <param name="body">The content we want to post</param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="ApiResponse{T}"/> for processing further</returns>
    Task<ApiResponse<String>> PostContentAsync(String uri, T body, CancellationToken cancellationToken = new ());

    /// <summary>
    /// Initiates a <see cref="HttpMethod.Delete"/> to a particular endpoint provided by <paramref name="uri"/>, with the 
    /// </summary>
    /// <param name="uri">The endpoint we're aiming to hit</param>
    /// <param name="body">The content we want to post</param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="ApiResponse{T}"/> for processing further</returns>
    Task<ApiResponse<String>> DeleteContentAsync(String uri, CancellationToken cancellationToken = new());
}

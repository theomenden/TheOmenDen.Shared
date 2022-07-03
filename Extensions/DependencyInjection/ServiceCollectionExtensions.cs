using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace TheOmenDen.Shared.Extensions.DependencyInjection;
/// <summary>
/// Service registrations for types that implement <see cref="IApiService"/> and <see cref="IApiStreamService"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTheOmenDenHttpServices(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        services.AddOptions<HttpClientConfiguration>()
            .Configure(options =>
            {
                options.Name = httpClientConfiguration.Name;
                options.BaseAddress = httpClientConfiguration.BaseAddress;
            });

        services.AddHttpClient<IApiService>(httpClientConfiguration.Name, options =>
            {
                options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped(typeof(IApiService<>), typeof(ApiServiceBase<>));

        return services;
    }

    /// <summary>
    /// Adds Basic Api interaction via <see cref="ApiServiceBase{T}"/>
    /// </summary>
    /// <typeparam name="T">The containing DI type</typeparam>
    /// <param name="services">The existing service collection</param>
    /// <param name="httpClientConfiguration">Configuration for the named client</param>
    /// <returns>The same <see cref="IServiceCollection"/> for further chaining</returns>
    public static IServiceCollection AddTheOmenDenHttpServices<T>(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        var containerType = typeof(T);

        services.AddOptions<HttpClientConfiguration>()
            .Configure(options =>
            {
                options.Name = httpClientConfiguration.Name;
                options.BaseAddress = httpClientConfiguration.BaseAddress;
            });

        services.AddHttpClient<IApiService>(httpClientConfiguration.Name, options =>
            {
                options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IApiService), containerType)
            .AddClasses(c => c.AssignableTo(typeof(ApiServiceBase<>)))
            .AsImplementedInterfaces());

        return services;
    }

    public static IServiceCollection AddTheOmenDenHttpStreamingServices(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        services.AddOptions<HttpClientConfiguration>()
            .Configure(options =>
            {
                options.Name = httpClientConfiguration.Name;
                options.BaseAddress = httpClientConfiguration.BaseAddress;
            });

        services.AddHttpClient<IApiStreamService>(httpClientConfiguration.Name, options =>
        {
            options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress);
        })
        .AddPolicyHandler(GetRetryPolicy())
        .AddPolicyHandler(GetCircuitBreakerPolicy());


        services.AddScoped(typeof(IApiStreamService<>), typeof(ApiStreamServiceBase<>));

        return services;
    }

    /// <summary>
    /// Adds Basic Streaming Api interaction via <see cref="ApiStreamServiceBase{T}"/>
    /// </summary>
    /// <typeparam name="T">The containing DI type</typeparam>
    /// <param name="services">The existing service collection</param>
    /// <param name="httpClientConfiguration">Configuration for the named client</param>
    /// <returns>The same <see cref="IServiceCollection"/> for further chaining</returns>
    public static IServiceCollection AddTheOmenDenHttpStreamingServices<T>(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        var containerType = typeof(T);

        services.AddOptions<HttpClientConfiguration>()
            .Configure(options =>
            {
                options.Name = httpClientConfiguration.Name;
                options.BaseAddress = httpClientConfiguration.BaseAddress;
            });

        services.AddHttpClient<IApiStreamService>(httpClientConfiguration.Name, options =>
            {
                options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IApiStreamService), containerType)
            .AddClasses(c => c.AssignableTo(typeof(ApiServiceBase<>)))
            .AsImplementedInterfaces());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}

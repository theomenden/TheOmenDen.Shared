using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TheOmenDen.Shared.Configuration;
using TheOmenDen.Shared.Infrastructure;
using TheOmenDen.Shared.Services;

namespace TheOmenDen.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTheOmenDenHttpServices(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        services.AddHttpClient<IApiService>(httpClientConfiguration.Name, options => options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped(typeof(IApiService<>), typeof(ApiService<>));
        
        return services;
    }


    public static IServiceCollection AddTheOmenDenHttpServices<T>(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        var containerType = typeof(T);

        services.AddHttpClient<IApiService>(httpClientConfiguration.Name, options => options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped(typeof(IApiService<>), typeof(ApiService<>));

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IApiService), containerType)
            .AddClasses(c => c.AssignableTo(typeof(IApiService<>)))
            .AsImplementedInterfaces());

        return services;
    }

    public static IServiceCollection AddTheOmenDenHttpStreamingServices(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        services.AddHttpClient<IApiStreamService>(httpClientConfiguration.Name, options => options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped(typeof(IApiStreamService<>), typeof(ApiStreamService<>));

        return services;
    }

    public static IServiceCollection AddTheOmenDenHttpStreamingServices<T>(this IServiceCollection services, HttpClientConfiguration httpClientConfiguration)
    {
        var containerType = typeof(T);

        services.AddHttpClient<IApiStreamService>(httpClientConfiguration.Name, options => options.BaseAddress = new Uri(httpClientConfiguration.BaseAddress))
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped(typeof(IApiStreamService<>), typeof(ApiStreamService<>));

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IApiStreamService), containerType)
            .AddClasses(c => c.AssignableTo(typeof(IApiStreamService<>)))
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

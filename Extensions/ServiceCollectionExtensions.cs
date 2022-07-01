using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TheOmenDen.Shared.Infrastructure;
using TheOmenDen.Shared.Services;

namespace TheOmenDen.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTheOmenDenHttpServices(this IServiceCollection services, String clientName,
        String baseUri)
    {
        services.AddHttpClient<IApiService>(clientName, options => { options.BaseAddress = new(baseUri); })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped<IApiService, ApiService>();
        
        return services;
    }


    public static IServiceCollection AddTheOmenDenHttpServices<T>(this IServiceCollection services, String clientName,
        String baseUri)
    {
        var containerType = typeof(T);

        services.AddHttpClient<IApiService>(clientName, options => { options.BaseAddress = new(baseUri); })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped<IApiService, ApiService>();

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IApiService), containerType)
            .AddClasses()
            .AsImplementedInterfaces());

        return services;
    }

    public static IServiceCollection AddTheOmenDenHttpStreamingServices(this IServiceCollection services,
        String clientName,
        String baseUri)
    {
        services.AddHttpClient<IApiStreamService>(clientName, options => { options.BaseAddress = new(baseUri); })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped<IApiStreamService, ApiStreamService>();


        return services;
    }

    public static IServiceCollection AddTheOmenDenHttpStreamingServices<T>(this IServiceCollection services,
        String clientName,
        String baseUri)
    {
        var containerType = typeof(T);

        services.AddHttpClient<IApiStreamService>(clientName, options => { options.BaseAddress = new(baseUri); })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy()); ;


        services.AddScoped<IApiStreamService, ApiStreamService>();

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IApiStreamService), containerType)
            .AddClasses()
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

using Microsoft.Extensions.DependencyInjection;

namespace GuardianClient.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds GuardianApiClient to the service collection with HttpClient factory support
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="apiKey">Your Guardian API key</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddGuardianApiClient(this IServiceCollection services, string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        // Register the HttpClient with IHttpClientFactory
        services.AddHttpClient<GuardianApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://content.guardianapis.com");
            client.DefaultRequestHeaders.Add("User-Agent", "GuardianClient.NET/0.1.0-alpha");
        });

        // Register GuardianApiClient as scoped
        services.AddScoped<GuardianApiClient>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(GuardianApiClient));
            return new GuardianApiClient(httpClient, apiKey);
        });

        return services;
    }

    /// <summary>
    /// Adds GuardianApiClient to the service collection with configuration action
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="apiKey">Your Guardian API key</param>
    /// <param name="configureClient">Action to configure the HttpClient</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddGuardianApiClient(
        this IServiceCollection services,
        string apiKey,
        Action<HttpClient> configureClient)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key cannot be null or empty", nameof(apiKey));

        // Register the HttpClient with IHttpClientFactory and custom configuration
        services.AddHttpClient<GuardianApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://content.guardianapis.com");
            client.DefaultRequestHeaders.Add("User-Agent", "GuardianClient.NET/0.1.0-alpha");

            // Apply custom configuration
            configureClient?.Invoke(client);
        });

        // Register GuardianApiClient as scoped
        services.AddScoped<GuardianApiClient>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(GuardianApiClient));
            return new GuardianApiClient(httpClient, apiKey);
        });

        return services;
    }
}

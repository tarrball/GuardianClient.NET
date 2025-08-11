using Microsoft.Extensions.DependencyInjection;

namespace GuardianClient.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="GuardianApiClient"/> in the service collection,
    /// configuring it to use <see cref="IHttpClientFactory"/> for efficient
    /// <see cref="HttpClient"/> management.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="apiKey">The Guardian API key to use for all requests.</param>
    /// <returns>The same <paramref name="services"/> instance for chaining.</returns>
    public static IServiceCollection AddGuardianApiClient(this IServiceCollection services, string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        services.AddHttpClient<GuardianApiClient>();

        services.AddScoped<GuardianApiClient>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(GuardianApiClient));

            return new GuardianApiClient(httpClient, apiKey);
        });

        return services;
    }
}

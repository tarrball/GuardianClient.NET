using System.Reflection;
using System.Text.Json;
using GuardianClient.Models;

namespace GuardianClient;

public class GuardianApiClient : IDisposable
{
    private readonly HttpClient _httpClient;

    private readonly string _apiKey;

    private readonly bool _ownsHttpClient;

    private bool _disposed;

    private const string BaseUrl = "https://content.guardianapis.com";

    private static readonly JsonSerializerOptions _JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of GuardianApiClient with dependency-injected HttpClient (recommended)
    /// </summary>
    /// <param name="httpClient">HttpClient instance (should be managed by DI container or HttpClientFactory)</param>
    /// <param name="apiKey">Your Guardian API key</param>
    public GuardianApiClient(HttpClient httpClient, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _httpClient = httpClient;
        _apiKey = apiKey;
        _ownsHttpClient = false;

        ConfigureHttpClient();
    }

    /// <summary>
    /// Initializes a new instance of GuardianApiClient with internal HttpClient
    /// Note: For production apps, consider using the HttpClient constructor with DI instead
    /// </summary>
    /// <param name="apiKey">Your Guardian API key</param>
    public GuardianApiClient(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _ownsHttpClient = true;

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        var packageVersion = GetPackageVersion();

        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", $"GuardianClient.NET/{packageVersion}");
    }

    private string GetPackageVersion()
    {
        var packageVersion = Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
            .InformationalVersion;

        return packageVersion;
    }

    /// <summary>
    /// Search for Guardian content
    /// </summary>
    /// <param name="query">Search query (supports AND, OR, NOT operators)</param>
    /// <param name="pageSize">Number of results per page (1-50)</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="options">API options for including additional response data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Content search results</returns>
    public async Task<ContentSearchResponse?> SearchAsync(
        string? query = null,
        int? pageSize = null,
        int? page = null,
        GuardianApiOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        if (!string.IsNullOrWhiteSpace(query))
        {
            parameters.Add($"q={Uri.EscapeDataString(query)}");
        }

        if (pageSize.HasValue)
        {
            parameters.Add($"page-size={pageSize.Value}");
        }

        if (page.HasValue)
        {
            parameters.Add($"page={page.Value}");
        }

        if (options?.ShowFields?.Length > 0)
        {
            parameters.Add($"show-fields={string.Join(",", options.ShowFields)}");
        }

        if (options?.ShowTags?.Length > 0)
        {
            parameters.Add($"show-tags={string.Join(",", options.ShowTags)}");
        }

        if (options?.ShowElements?.Length > 0)
        {
            parameters.Add($"show-elements={string.Join(",", options.ShowElements)}");
        }

        if (options?.ShowReferences?.Length > 0)
        {
            parameters.Add($"show-references={string.Join(",", options.ShowReferences)}");
        }

        if (options?.ShowBlocks?.Length > 0)
        {
            parameters.Add($"show-blocks={string.Join(",", options.ShowBlocks)}");
        }

        var url = $"/search?{string.Join("&", parameters)}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var wrapper = JsonSerializer.Deserialize<ResponseWrapper<ContentSearchResponse>>(content, _JsonOptions);

        return wrapper?.Response;
    }

    /// <summary>
    /// Get a single content item by its ID/path
    /// </summary>
    /// <param name="itemId">The content item ID (path from Guardian API)</param>
    /// <param name="options">API options for including additional response data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Single item response with content details</returns>
    public async Task<SingleItemResponse?> GetItemAsync(
        string itemId,
        GuardianApiOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);

        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        if (options?.ShowFields?.Length > 0)
        {
            parameters.Add($"show-fields={string.Join(",", options.ShowFields)}");
        }

        if (options?.ShowTags?.Length > 0)
        {
            parameters.Add($"show-tags={string.Join(",", options.ShowTags)}");
        }

        if (options?.ShowElements?.Length > 0)
        {
            parameters.Add($"show-elements={string.Join(",", options.ShowElements)}");
        }

        if (options?.ShowReferences?.Length > 0)
        {
            parameters.Add($"show-references={string.Join(",", options.ShowReferences)}");
        }

        if (options?.ShowBlocks?.Length > 0)
        {
            parameters.Add($"show-blocks={string.Join(",", options.ShowBlocks)}");
        }

        var url = $"/{itemId}?{string.Join("&", parameters)}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var wrapper = JsonSerializer.Deserialize<ResponseWrapper<SingleItemResponse>>(content, _JsonOptions);

        return wrapper?.Response;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the <see cref="HttpClient"/> if:
    /// 1. This instance owns it, and
    /// 2. Disposal is being triggered explicitly (not already disposed or disposing).
    /// This prevents disposing of HttpClient instances managed by dependency injection.
    /// </summary>
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing)
        {
            return;
        }

        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }

        _disposed = true;
    }
}

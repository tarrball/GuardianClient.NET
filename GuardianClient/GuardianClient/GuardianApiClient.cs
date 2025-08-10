using System.Text.Json;
using GuardianClient.Models;

namespace GuardianClient;

public class GuardianApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly bool _ownsHttpClient;
    private bool _disposed = false;

    private const string BaseUrl = "https://content.guardianapis.com";

    /// <summary>
    /// Initializes a new instance of GuardianApiClient with dependency-injected HttpClient (recommended)
    /// </summary>
    /// <param name="httpClient">HttpClient instance (should be managed by DI container or HttpClientFactory)</param>
    /// <param name="apiKey">Your Guardian API key</param>
    public GuardianApiClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
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
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _httpClient = new HttpClient();
        _ownsHttpClient = true;

        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "GuardianClient.NET/0.1.0-alpha");
    }

    /// <summary>
    /// Search for Guardian content
    /// </summary>
    /// <param name="query">Search query (supports AND, OR, NOT operators)</param>
    /// <param name="pageSize">Number of results per page (1-50)</param>
    /// <param name="page">Page number for pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Content search results</returns>
    public async Task<ContentSearchResponse?> SearchAsync(
        string? query = null,
        int? pageSize = null,
        int? page = null,
        CancellationToken cancellationToken = default)
    {
        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        if (!string.IsNullOrWhiteSpace(query))
            parameters.Add($"q={Uri.EscapeDataString(query)}");

        if (pageSize.HasValue)
            parameters.Add($"page-size={pageSize.Value}");

        if (page.HasValue)
            parameters.Add($"page={page.Value}");

        var url = $"/search?{string.Join("&", parameters)}";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var wrapper = JsonSerializer.Deserialize<ApiResponseWrapper<ContentSearchResponse>>(content, options);
        return wrapper?.Response;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            if (_ownsHttpClient)
            {
                _httpClient?.Dispose();
            }

            _disposed = true;
        }
    }
}

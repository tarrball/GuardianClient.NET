using System.Text.Json;
using GuardianClient.Internal;
using GuardianClient.Models;
using GuardianClient.Options.Search;

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

    /// <summary>
    /// Search for Guardian content using comprehensive search options.
    /// </summary>
    /// <param name="options">
    /// Search options including query terms, filters, pagination, ordering, date ranges, and additional information to include.
    /// If null, returns all content with default settings (newest first, page 1, 10 items per page).
    /// </param>
    /// <param name="cancellationToken">Cancellation token to cancel the HTTP request</param>
    /// <returns>
    /// A <see cref="ContentSearchResponse"/> containing the search results, pagination info, and metadata.
    /// Returns null if the API response cannot be deserialized.
    /// </returns>
    /// <exception cref="HttpRequestException">Thrown when the API request fails or returns a non-success status code</exception>
    /// <exception cref="TaskCanceledException">Thrown when the request is cancelled via the cancellation token</exception>
    /// <remarks>
    /// <para>This method provides access to the full Guardian Content API search functionality, including:</para>
    /// <list type="bullet">
    /// <item><description>Free text search with boolean operators (AND, OR, NOT)</description></item>
    /// <item><description>Filtering by section, tags, references, production office, language, star rating, and more</description></item>
    /// <item><description>Date range filtering with different date types (published, first-publication, newspaper-edition, last-modified)</description></item>
    /// <item><description>Pagination and result ordering options</description></item>
    /// <item><description>Additional fields, tags, elements, references, and blocks in responses</description></item>
    /// </list>
    /// <para>For simple searches, you can create basic options: <c>new GuardianApiContentSearchOptions { Query = "your search terms" }</c></para>
    /// </remarks>
    public async Task<ContentSearchResponse?> SearchAsync(
        GuardianApiContentSearchOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        options ??= new GuardianApiContentSearchOptions();

        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        UrlParameterBuilder.AddQueryParameters(options, parameters);
        UrlParameterBuilder.AddFilterParameters(options.FilterOptions, parameters);
        UrlParameterBuilder.AddDateParameters(options.DateOptions, parameters);
        UrlParameterBuilder.AddPageParameters(options.PageOptions, parameters);
        UrlParameterBuilder.AddOrderParameters(options.OrderOptions, parameters);
        UrlParameterBuilder.AddAdditionalInformationParameters(options.AdditionalInformationOptions, parameters);

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
        GuardianApiContentAdditionalInformationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);

        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        UrlParameterBuilder.AddParameterIfAny(parameters, "show-fields", options?.ShowFields);
        UrlParameterBuilder.AddParameterIfAny(parameters, "show-tags", options?.ShowTags);
        UrlParameterBuilder.AddParameterIfAny(parameters, "show-elements", options?.ShowElements);
        UrlParameterBuilder.AddParameterIfAny(parameters, "show-references", options?.ShowReferences);
        UrlParameterBuilder.AddParameterIfAny(parameters, "show-blocks", options?.ShowBlocks);

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

    private void ConfigureHttpClient()
    {
        var packageVersion = AssemblyInfo.GetPackageVersion();

        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", $"GuardianClient.NET/{packageVersion}");
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

using System.Diagnostics;
using System.Text.Json;
using GuardianClient.Internal;
using GuardianClient.Models;
using GuardianClient.Options.Search;

namespace GuardianClient;

public class GuardianApiClient : IGuardianApiClient, IDisposable
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
        var packageVersion = AssemblyInfo.GetPackageVersion();

        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", $"GuardianClient.NET/{packageVersion}");
    }

    public async Task<ContentSearchResponse?> SearchAsync(
        SearchOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        options ??= new SearchOptions();

        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        UrlParameterBuilder.AddQueryParameters(options, parameters);
        UrlParameterBuilder.AddFilterParameters(options.FilterOptions, parameters);
        UrlParameterBuilder.AddDateParameters(options.DateOptions, parameters);
        UrlParameterBuilder.AddPageParameters(options.PageOptions, parameters);
        UrlParameterBuilder.AddOrderParameters(options.OrderOptions, parameters);
        UrlParameterBuilder.AddAdditionalInformationParameters(options.AdditionalInformationOptions, parameters);

        var url = $"/search?{string.Join("&", parameters)}";
        DebugWriteLine($"Guardian API URL: {BaseUrl}{url}");
        var response = await _httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var wrapper = JsonSerializer.Deserialize<ResponseWrapper<ContentSearchResponse>>(content, _JsonOptions);

        return wrapper?.Response;
    }

    public async Task<SingleItemResponse?> GetItemAsync(
        string itemId,
        AdditionalInformationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(itemId);

        var parameters = new List<string> { $"api-key={Uri.EscapeDataString(_apiKey)}" };

        UrlParameterBuilder.AddParameterIfAny(
            parameters,
            "show-fields",
            options?.ShowFields,
            option => option.ToApiString()
        );

        UrlParameterBuilder.AddParameterIfAny(
            parameters,
            "show-tags",
            options?.ShowTags,
            option => option.ToApiString()
        );

        UrlParameterBuilder.AddParameterIfAny(
            parameters,
            "show-elements",
            options?.ShowElements,
            option => option.ToApiString()
        );

        UrlParameterBuilder.AddParameterIfAny(
            parameters,
            "show-references",
            options?.ShowReferences,
            option => option.ToApiString()
        );

        UrlParameterBuilder.AddParameterIfAny(
            parameters,
            "show-blocks",
            options?.ShowBlocks
        );

        var url = $"/{itemId}?{string.Join("&", parameters)}";
        DebugWriteLine($"Guardian API URL: {BaseUrl}{url}");
        var response = await _httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var wrapper = JsonSerializer.Deserialize<ResponseWrapper<SingleItemResponse>>(content, _JsonOptions);

        return wrapper?.Response;
    }

    [Conditional("DEBUG")]
    private static void DebugWriteLine(string message)
    {
        Debug.WriteLine(message);
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

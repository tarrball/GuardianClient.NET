using GuardianClient.Models;
using GuardianClient.Options.Search;

namespace GuardianClient;

/// <summary>
/// Interface for the Guardian API client, providing access to Guardian content search and retrieval.
/// </summary>
public interface IGuardianApiClient
{
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
    Task<ContentSearchResponse?> SearchAsync(
        GuardianApiContentSearchOptions? options = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get a single content item by its ID/path
    /// </summary>
    /// <param name="itemId">The content item ID (path from Guardian API)</param>
    /// <param name="options">API options for including additional response data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Single item response with content details</returns>
    Task<SingleItemResponse?> GetItemAsync(
        string itemId,
        GuardianApiContentAdditionalInformationOptions? options = null,
        CancellationToken cancellationToken = default
    );
}

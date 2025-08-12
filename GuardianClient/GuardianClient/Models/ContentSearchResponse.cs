using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class ContentSearchResponse : BaseResponse
{
    /// <summary>
    /// The starting index for the current result set.
    /// </summary>
    [JsonPropertyName("startIndex")]
    public int StartIndex { get; set; }

    /// <summary>
    /// The number of items returned in this call.
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// The number of the page you are browsing.
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// The total amount of pages that are in this call.
    /// </summary>
    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    /// <summary>
    /// The sort order used.
    /// </summary>
    [JsonPropertyName("orderBy")]
    public string? OrderBy { get; set; }

    /// <summary>
    /// The collection of content items returned by the search.
    /// </summary>
    [JsonPropertyName("results")]
    public ICollection<ContentItem> Results { get; set; } = new HashSet<ContentItem>();
}

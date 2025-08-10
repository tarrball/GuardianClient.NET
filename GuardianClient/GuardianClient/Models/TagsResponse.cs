using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class TagsResponse : BaseResponse
{
    [JsonPropertyName("startIndex")]
    public int StartIndex { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    [JsonPropertyName("results")]
    public List<Tag> Results { get; set; } = new();
}